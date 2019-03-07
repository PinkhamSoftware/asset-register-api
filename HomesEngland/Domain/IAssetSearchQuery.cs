namespace HomesEngland.Domain
{
    public interface IAssetSearchQuery
    {
        int? SchemeId { get; set; }
        string Address { get; set; }
        int? AssetRegisterVersionId { get; set; }
        string Region { get; set; }
        string Developer { get; set; }
    }
}
