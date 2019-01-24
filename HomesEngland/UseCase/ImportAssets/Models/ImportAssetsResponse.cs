using System;
using System.Collections.Generic;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.Models;
using HomesEngland.UseCase.SearchAsset.Models;

namespace HomesEngland.UseCase.ImportAssets.Models
{
    public class ImportAssetsResponse : IResponse<AssetOutputModel>
    {
        public IList<AssetOutputModel> AssetsImported { get; set; }

        public IList<AssetOutputModel> ToCsv()
        {
            throw new NotImplementedException();
        }
    }
}
