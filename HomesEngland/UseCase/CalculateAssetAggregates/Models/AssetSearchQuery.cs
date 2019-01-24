using HomesEngland.Domain;

namespace HomesEngland.UseCase.CalculateAssetAggregates.Models
{
    public class AssetSearchQuery : IAssetSearchQuery
    {
        public int? SchemeId { get; set; }
        public string Address { get; set; }
        public int? AssetRegisterVersionId { get; set; }
    }
}
