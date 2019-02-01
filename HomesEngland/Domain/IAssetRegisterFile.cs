using System.Collections.Generic;
using System.Text;

namespace HomesEngland.Domain
{
    public interface IAssetRegisterFile:IDatabaseEntity<int>
    {
        string Text { get; set; }
        string FileName { get; set; }
    }
}
