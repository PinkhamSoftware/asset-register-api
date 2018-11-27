﻿using HomesEngland.Domain;

namespace HomesEngland.UseCase.SearchAsset.Models
{
    public class AssetSearchQuery:IAssetSearchQuery
    {
        public int? SchemeId { get; set; }
        public string Address { get; set; }
    }
}
