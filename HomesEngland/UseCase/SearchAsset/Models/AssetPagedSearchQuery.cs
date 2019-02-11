using HomesEngland.Domain;

namespace HomesEngland.UseCase.SearchAsset.Models
{
    public class AssetPagedSearchQuery : IAssetPagedSearchQuery
    {
        public int? SchemeId { get; set; }
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 25;
        public string Address { get; set; }
        public int? AssetRegisterVersionId { get; set; }
        public string Region { get; set; }
    }
}
