using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.GetAssetRegisterVersions.Models;
using HomesEngland.UseCase.ImportAssets;
using HomesEngland.UseCase.ImportAssets.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using WebApi.BackgroundProcessing;
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
        private readonly IBackgroundProcessor _backgroundProcessor;

        public AssetRegisterVersionController(IGetAssetRegisterVersionsUseCase registerVersionsUseCase, IImportAssetsUseCase importAssetsUseCase, ITextSplitter textSplitter, IBackgroundProcessor backgroundProcessor)
        {
            _getAssetRegisterVersionsUseCase = registerVersionsUseCase;
            _importAssetsUseCase = importAssetsUseCase;
            _textSplitter = textSplitter;
            _backgroundProcessor = backgroundProcessor;
        }

        [HttpGet]
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ResponseData<GetAssetResponse>), 200)]
        public async Task<IActionResult> Get([FromQuery]GetAssetRegisterVersionsRequest request)
        {
            if (!request.IsValid())
                return StatusCode(400);

            return this.StandardiseResponse<GetAssetRegisterVersionsResponse, AssetRegisterVersionOutputModel>(
                await _getAssetRegisterVersionsUseCase.ExecuteAsync(request, CancellationToken.None).ConfigureAwait(false));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseData<ImportAssetsResponse>), 200)]
        public async Task<IActionResult> Post(IList<IFormFile> files)
        {
            if (files == null || !files.Any())
                return BadRequest();

            var request = await CreateSaveAssetRegisterFileRequest(files);

            await _backgroundProcessor.QueueBackgroundTask(async () => await _importAssetsUseCase.ExecuteAsync(request, this.GetCancellationToken()).ConfigureAwait(false));

            return Ok();
        }

        private async Task<ImportAssetsRequest> CreateSaveAssetRegisterFileRequest(IList<IFormFile> files)
        {
            var memoryStream = new MemoryStream();
            await files[0].CopyToAsync(memoryStream, this.GetCancellationToken()).ConfigureAwait(false);
            var text = Encoding.UTF8.GetString(memoryStream.GetBuffer());
            var fileName = files[0]?.FileName;

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
