using System;

namespace HomesEngland.UseCase.GetAssetRegisterVersions.Models
{
    public class AssetRegisterVersionOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
