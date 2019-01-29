using HomesEngland.Boundary.UseCase;
using HomesEnglandTest.UseCase.SaveUploadedAssetRegisterFile.Models;

namespace HomesEngland.UseCase.SaveUploadedAssetRegisterFile
{
    public interface ISaveUploadedAssetRegisterFileUseCase:IAsyncUseCaseTask<SaveAssetRegisterFileRequest,SaveAssetRegisterFileResponse>
    {

    }
}
