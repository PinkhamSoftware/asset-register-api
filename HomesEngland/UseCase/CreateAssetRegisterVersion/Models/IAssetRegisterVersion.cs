using System.Collections.Generic;
using HomesEngland.Domain;

namespace HomesEngland.UseCase.CreateAssetRegisterVersion.Models
{
    public interface IAssetRegisterVersion : IDatabaseEntity<int>
    {
        IList<IAsset> Assets { get; set; }
    }
}
