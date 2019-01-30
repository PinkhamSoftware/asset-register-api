using System.Collections.Generic;

namespace HomesEngland.UseCase.ImportAssets.Models
{
    public class ImportAssetsRequest
    {
        public IList<string> AssetLines { get; set; }
        public string Delimiter { get; set; }
        public string FileName { get; set; }
    }
}
