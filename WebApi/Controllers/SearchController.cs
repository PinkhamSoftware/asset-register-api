using System.Collections.Generic;
using System.Threading.Tasks;
using HomesEngland.Boundary.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    using AssetsDictionary = Dictionary<string, Dictionary<string, string>[]>;

    [Route("[controller]")]
    [ApiController]
    public class SearchController : Controller
    {
        private ISearchAssetsUseCase UseCase { get; }
        public SearchController(ISearchAssetsUseCase useCase)
        {
            UseCase = useCase;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<AssetsDictionary>> Get(string query)
        {
            return GetWrappedAssets(await UseCase.Execute(query));
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
