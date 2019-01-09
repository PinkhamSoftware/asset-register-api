using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;

namespace HomesEngland.Gateway.Notifications
{
    public interface IOneTimeLinkNotifier
    {
        Task<bool> SendOneTimeLinkAsync(IOneTimeLinkNotification notification, CancellationToken cancellationToken);
    }
}
