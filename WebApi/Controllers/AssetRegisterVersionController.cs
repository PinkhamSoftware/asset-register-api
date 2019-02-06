using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain.Impl;
using HomesEngland.Gateway.Notifications;
using HomesEngland.BackgroundProcessing;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.GetAssetRegisterVersions.Models;
using HomesEngland.UseCase.ImportAssets;
using HomesEngland.UseCase.ImportAssets.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Primitives;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AssetRegisterVersionController : ControllerBase
    {
        private readonly IGetAssetRegisterVersionsUseCase _getAssetRegisterVersionsUseCase;
        private readonly IImportAssetsUseCase _importAssetsUseCase;
        private readonly ITextSplitter _textSplitter;
        private readonly IAssetRegisterUploadProcessedNotifier _assetRegisterUploadProcessedNotifier;
        private readonly IBackgroundProcessor _backgroundProcessor;

        public AssetRegisterVersionController(IGetAssetRegisterVersionsUseCase registerVersionsUseCase,
            IImportAssetsUseCase importAssetsUseCase, ITextSplitter textSplitter,
            IAssetRegisterUploadProcessedNotifier assetRegisterUploadProcessedNotifier, IBackgroundProcessor backgroundProcessor)
        {
            _getAssetRegisterVersionsUseCase = registerVersionsUseCase;
            _importAssetsUseCase = importAssetsUseCase;
            _textSplitter = textSplitter;
            _assetRegisterUploadProcessedNotifier = assetRegisterUploadProcessedNotifier;
            _backgroundProcessor = backgroundProcessor;
        }

        [HttpGet]
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ResponseData<GetAssetResponse>), 200)]
        public async Task<IActionResult> Get([FromQuery] GetAssetRegisterVersionsRequest request)
        {
            if (!request.IsValid())
                return StatusCode(400);

            return this.StandardiseResponse<GetAssetRegisterVersionsResponse, AssetRegisterVersionOutputModel>(
                await _getAssetRegisterVersionsUseCase.ExecuteAsync(request, CancellationToken.None)
                    .ConfigureAwait(false));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseData<ImportAssetsResponse>), 200)]
        public async Task<IActionResult> Post(IList<IFormFile> files)
        {
            if (files == null || !EnumerableExtensions.Any(files))
                return BadRequest();

            StringValues authorisationHeader = Request.Headers["Authorization"];
            string email = GetEmailFromAuthorizationHeader(authorisationHeader);

            var request = await CreateSaveAssetRegisterFileRequest(files);

            var response = await _importAssetsUseCase.ExecuteAsync(request, this.GetCancellationToken())
                .ConfigureAwait(false);

            await _assetRegisterUploadProcessedNotifier.SendUploadProcessedNotification(new UploadProcessedNotification
                {
                    Email = email,
                    UploadSuccessfullyProcessed = true
                },
                this.GetCancellationToken());
            await _backgroundProcessor.QueueBackgroundTask(
                async ()=> await _importAssetsUseCase.ExecuteAsync(request, this.GetCancellationToken()).ConfigureAwait(false));

            return Ok();
        }

        private static string GetEmailFromAuthorizationHeader(StringValues authorisationHeader)
        {
            string token = Regex.Matches(authorisationHeader, @"Bearer (\S+)").First().Groups[1].Value;
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            string email = handler.ReadJwtToken(token).Claims.First(c => c.Type.Equals("email")).Value;
            return email;
        }

        private async Task<ImportAssetsRequest> CreateSaveAssetRegisterFileRequest(IList<IFormFile> files)
        {
            var memoryStream = new MemoryStream();
            await files[0].CopyToAsync(memoryStream, this.GetCancellationToken()).ConfigureAwait(false);
            var text = Encoding.UTF8.GetString(memoryStream.GetBuffer());

            var assetLines = _textSplitter.SplitIntoLines(text);
            var importAssetsRequest = new ImportAssetsRequest
            {
                Delimiter = ";",
                AssetLines = assetLines,
                FileName = files[0]?.FileName
            };
            return importAssetsRequest;
        }
    }
}
