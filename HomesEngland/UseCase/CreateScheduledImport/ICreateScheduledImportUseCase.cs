using HomesEngland.Boundary.UseCase;
using HomesEngland.UseCase.CreateScheduledImport.Models;

namespace HomesEngland.UseCase.CreateScheduledImport
{
    public interface ICreateScheduledImportUseCase:IAsyncUseCaseTask<CreateScheduledImportRequest, CreateScheduledImportResponse>
    {

    }
}
