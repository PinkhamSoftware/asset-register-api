using System.Threading;
using System.Threading.Tasks;
using HomesEngland.UseCase.SaveUploadedAssetRegisterFile.Models;

namespace HomesEngland.UseCase.SaveUploadedAssetRegisterFile
{
    public class SaveUploadedAssetRegisterFileUseCase : ISaveUploadedAssetRegisterFileUseCase
    {
        public SaveUploadedAssetRegisterFileUseCase()
        {

        }

        public Task<SaveAssetRegisterFileResponse> ExecuteAsync(SaveAssetRegisterFileRequest requests, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
