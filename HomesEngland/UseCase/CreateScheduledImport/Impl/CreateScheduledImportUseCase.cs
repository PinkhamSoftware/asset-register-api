using System.Threading;
using System.Threading.Tasks;
using HomesEngland.UseCase.CreateScheduledImport.Models;
using HomesEngland.UseCase.SaveFile;
using HomesEngland.UseCase.SaveFile.Model;

namespace HomesEngland.UseCase.CreateScheduledImport.Impl
{
    public class CreateScheduledImportUseCase : ICreateScheduledImportUseCase
    {
        private readonly ISaveAssetRegisterFileUseCase _saveAssetRegisterFileUseCase;

        public CreateScheduledImportUseCase(ISaveAssetRegisterFileUseCase saveAssetRegisterFileUseCase)
        {
            _saveAssetRegisterFileUseCase = saveAssetRegisterFileUseCase;
        }

        public async Task<CreateScheduledImportResponse> ExecuteAsync(CreateScheduledImportRequest requests, CancellationToken cancellationToken)
        {
            var saveFileRequest = new SaveAssetRegisterFileRequest
            {
                FileName = requests?.FileName,
                Text = requests?.Text
            };

            var saveFileResponse = await _saveAssetRegisterFileUseCase.ExecuteAsync(saveFileRequest, cancellationToken).ConfigureAwait(false);



            var createScheduledResponse = new CreateScheduledImportResponse
            {

            };
            return createScheduledResponse;
        }
    }
}
