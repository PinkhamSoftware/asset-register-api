using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;

namespace HomesEngland.Gateway.Notifications
{
    public interface IAssetRegisterUploadProcessedNotifier
    {
        Task<bool> SendUploadProcessedNotification(IUploadProcessedNotification notification,
            CancellationToken cancellationToken);
    }
}
