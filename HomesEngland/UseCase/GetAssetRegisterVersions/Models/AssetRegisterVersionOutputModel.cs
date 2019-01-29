using System;
using HomesEngland.UseCase.CreateAssetRegisterVersion.Models;

namespace HomesEngland.UseCase.GetAssetRegisterVersions.Models
{
    public class AssetRegisterVersionOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public AssetRegisterVersionOutputModel(){}

        public AssetRegisterVersionOutputModel(IAssetRegisterVersion assetRegisterVersion)
        {
            Id = assetRegisterVersion.Id;
            CreatedAt = assetRegisterVersion.ModifiedDateTime;
        }
    }
}
