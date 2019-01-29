using System.Threading;
using System.Threading.Tasks;
using HomesEnglandTest.UseCase.SaveUploadedAssetRegisterFile.Models;

namespace HomesEngland.UseCase.SaveUploadedAssetRegisterFile
{
    public class SaveUploadedAssetRegisterFileUseCase : ISaveUploadedAssetRegisterFileUseCase
    {
        public Task<SaveAssetRegisterFileResponse> ExecuteAsync(SaveAssetRegisterFileRequest requests, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
