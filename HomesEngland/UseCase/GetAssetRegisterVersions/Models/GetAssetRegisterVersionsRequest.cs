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

        public bool IsValid()
        {
            if (!Page.HasValue || Page <= 0)
                return false;
            if (!PageSize.HasValue || PageSize <= 0)
                return false;
            return true;
        }
    }
}
