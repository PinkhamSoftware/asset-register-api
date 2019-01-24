using System.Collections.Generic;
using System.Linq;
using HomesEngland.Domain;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.GetAsset.Models;

namespace TestHelper
{
    public static class AssetRegisterVersionExtensions
    {
        public static int GetAssetRegisterVersionId(this IList<IAsset> assets)
        {
            return assets.Select(s => s.AssetRegisterVersionId.Value).FirstOrDefault();
        }

        public static int GetAssetRegisterVersionId(this IList<AssetOutputModel> assets)
        {
            return assets.Select(s => s.AssetRegisterVersionId.Value).FirstOrDefault();
        }

        public static int GetAssetRegisterVersionId(this IList<CreateAssetResponse> assets)
        {
            return assets.Select(s => s.Asset.AssetRegisterVersionId.Value).FirstOrDefault();
        }
    }
}
