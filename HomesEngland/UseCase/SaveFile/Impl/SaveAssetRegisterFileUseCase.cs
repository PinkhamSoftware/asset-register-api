using System.Threading;
using System.Threading.Tasks;
using HomesEngland.UseCase.SaveFile.Model;

namespace HomesEngland.UseCase.SaveFile.Impl
{
    public class SaveAssetRegisterFileUseCase: ISaveAssetRegisterFileUseCase
    {
        public Task<SaveAssetRegisterFileResponse> ExecuteAsync(SaveAssetRegisterFileRequest requests, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
