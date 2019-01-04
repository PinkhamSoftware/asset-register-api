using System.Collections.Generic;
using HomesEngland.UseCase.GetAsset.Models;

namespace HomesEngland.UseCase.SearchAsset.Models
{
    public class SearchAssetResponse: IResponse<AssetOutputModel>
    {
        public IList<AssetOutputModel> Assets { get; set; }
        public int Pages { get; set; }
        public int TotalCount { get; set; }
        public IList<AssetOutputModel> ToCsv()
        {
            return Assets;
        }
    }

    public interface IResponse<T>
    {
        IList<T> ToCsv();
    }
}
