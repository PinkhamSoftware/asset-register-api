namespace HomesEngland.UseCase.GetAssetRegisterVersions.Models
{
    public class GetAssetRegisterVersionsRequest
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public GetAssetRegisterVersionsRequest()
        {
            Page = 1;
            PageSize = 25;
        }
    }
}
