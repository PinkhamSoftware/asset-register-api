using System;
using System.Collections.Generic;
using HomesEngland.Domain;

namespace HomesEngland.UseCase.BulkCreateAsset.Models
{
    public class AssetRegisterVersion : IAssetRegisterVersion
    {
        public int Id { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public virtual IList<IAsset> Assets { get; set; }

        public AssetRegisterVersion() { }

        public AssetRegisterVersion(IAssetRegisterVersion assetRegisterVersion)
        {
            Id = assetRegisterVersion.Id;
            ModifiedDateTime = assetRegisterVersion.ModifiedDateTime;
            Assets = assetRegisterVersion.Assets;
        }
    }
}