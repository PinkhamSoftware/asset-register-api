using System;

namespace HomesEngland.Domain
{
    public class AssetRegisterFile : IAssetRegisterFile
    {
        public int Id { get; set; }

        public string Text { get; set; }
        public string FileName { get; set; }

        public DateTime ModifiedDateTime { get; set; }
    }
}