using System.Collections.Generic;
using System.Threading.Tasks;
using HomesEngland.Boundary.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    using AssetsDictionary = Dictionary<string, Dictionary<string, string>[]>;

    [Route("[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IGetAssetsUseCase _assetsUseCase;
        public AssetsController(IGetAssetsUseCase useCase)
        {
            _assetsUseCase = useCase;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<AssetsDictionary>> Get(int[] ids)
        {
            return GetWrappedAssets( await _assetsUseCase.Execute(ids));
        }

        private static AssetsDictionary GetWrappedAssets(Dictionary<string, string>[] results)
        {
            return new AssetsDictionary
            {
                {
                    "Assets", results
                }
            };
        }
    }
}