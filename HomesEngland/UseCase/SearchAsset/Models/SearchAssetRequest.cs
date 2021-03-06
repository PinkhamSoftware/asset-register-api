﻿namespace HomesEngland.UseCase.SearchAsset.Models
{
    public class SearchAssetRequest 
    {
        public int? SchemeId { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Address { get; set; }
        public int? AssetRegisterVersionId { get; set; }
        public string Region { get; set; }
        public string Developer { get; set; }

        public SearchAssetRequest()
        {
            Page = 1;
            PageSize = 25;
        }
    }
}
