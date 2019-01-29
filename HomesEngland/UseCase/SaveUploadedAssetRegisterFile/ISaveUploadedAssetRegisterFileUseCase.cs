using HomesEngland.Boundary.UseCase;
using HomesEngland.UseCase.SaveUploadedAssetRegisterFile.Models;

namespace HomesEngland.UseCase.SaveUploadedAssetRegisterFile
{
    public interface ISaveUploadedAssetRegisterFileUseCase:IAsyncUseCaseTask<SaveAssetRegisterFileRequest,SaveAssetRegisterFileResponse>
    {

    }
}
