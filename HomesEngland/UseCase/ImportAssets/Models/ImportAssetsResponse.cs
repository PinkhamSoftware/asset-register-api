using System.Collections.Generic;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.Models;

namespace HomesEngland.UseCase.ImportAssets.Models
{
    public class ImportAssetsResponse : IResponse<AssetOutputModel>
    {
        public IList<AssetOutputModel> AssetsImported { get; set; }

        public IList<AssetOutputModel> ToCsv()
        {
            return AssetsImported;
        }
    }
}
