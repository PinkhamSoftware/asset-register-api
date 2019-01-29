using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.GetAssetRegisterVersions.Models;
using HomesEngland.UseCase.SaveUploadedAssetRegisterFile;
using HomesEnglandTest.UseCase.SaveUploadedAssetRegisterFile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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



    }
}
