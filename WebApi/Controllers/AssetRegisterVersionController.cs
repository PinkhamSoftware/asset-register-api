using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.GetAssetRegisterVersions.Models;
using HomesEngland.UseCase.SaveUploadedAssetRegisterFile;
using HomesEngland.UseCase.SaveUploadedAssetRegisterFile.Models;
using HomesEnglandTest.UseCase.SaveUploadedAssetRegisterFile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AssetRegisterVersionController : ControllerBase
    {
        private readonly IGetAssetRegisterVersionsUseCase _getAssetRegisterVersionsUseCase;
        private readonly ISaveUploadedAssetRegisterFileUseCase _saveUploadedAssetRegisterFileUseCase;

        public AssetRegisterVersionController(IGetAssetRegisterVersionsUseCase registerVersionsUseCase, ISaveUploadedAssetRegisterFileUseCase saveUploadedAssetRegisterFileUseCase)
        {
            _getAssetRegisterVersionsUseCase = registerVersionsUseCase;
            _saveUploadedAssetRegisterFileUseCase = saveUploadedAssetRegisterFileUseCase;
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
        [ProducesResponseType(typeof(ResponseData<SaveAssetRegisterFileResponse>), 200)]
        public async Task<IActionResult> Post(IList<IFormFile> files)
        {
            if (files == null || !EnumerableExtensions.Any(files))
                return BadRequest();

            var request = await CreateSaveAssetRegisterFileRequest(files);

            var response = await _saveUploadedAssetRegisterFileUseCase.ExecuteAsync(request, Request.HttpContext.RequestAborted).ConfigureAwait(false);

            return StatusCode(200, new ResponseData<SaveAssetRegisterFileResponse>(response));
        }

        private async Task<SaveAssetRegisterFileRequest> CreateSaveAssetRegisterFileRequest(IList<IFormFile> files)
        {
            var memoryStream = new MemoryStream();
            await files[0].CopyToAsync(memoryStream, Request.HttpContext.RequestAborted).ConfigureAwait(false);
            var request = new SaveAssetRegisterFileRequest
            {
                FileName = files.ElementAtOrDefault(0).FileName,
                FileBytes = memoryStream.GetBuffer()
            };
            return request;
        }
    }
}
