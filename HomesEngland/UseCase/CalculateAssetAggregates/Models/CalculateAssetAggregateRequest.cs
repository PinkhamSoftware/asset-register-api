namespace HomesEngland.UseCase.CalculateAssetAggregates.Models
{
    public class CalculateAssetAggregateRequest
    {
        public int? SchemeId { get; set; }
        public string Address { get; set; }
        public int? AssetRegisterVersionId { get; set; }
    }
}
