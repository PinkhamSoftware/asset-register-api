using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.Notifications;

namespace HomesEngland.Gateway.Sql
{
    public class DummyNotificationGateway : IOneTimeLinkNotifier
    {
        public Task<bool> SendOneTimeLinkAsync(IOneTimeLinkNotification notification, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
