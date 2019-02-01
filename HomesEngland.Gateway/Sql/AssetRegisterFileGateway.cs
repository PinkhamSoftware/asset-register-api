using System;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterFiles;

namespace HomesEngland.Gateway.Sql
{
    public class AssetRegisterFileGateway: IAssetRegisterFileCreator
    {
        private readonly string _databaseUrl;

        public AssetRegisterFileGateway(string databaseUrl)
        {
            _databaseUrl = databaseUrl;
        }


        public async Task<IAssetRegisterFile> CreateAsync(IAssetRegisterFile entity)
        {
            throw new NotImplementedException();
        }
    }
}
