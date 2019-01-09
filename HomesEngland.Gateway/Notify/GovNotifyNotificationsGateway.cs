using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.Notifications;
using Notify.Client;
using Notify.Interfaces;

namespace HomesEngland.Gateway.Notify
{
    public class GovNotifyNotificationsGateway : IOneTimeLinkNotifier
    {
        public async Task<bool> SendOneTimeLinkAsync(IOneTimeLinkNotification notification)
        {
            var client = NotificationClient();

            client.SendEmail(
                notification.Email,
                "8f02be8c-32db-4f18-97fe-1d60152e9b06",
                NotificationPersonalisation(notification)
            );

            return true;
        }

        private static INotificationClient NotificationClient()
        {
            string baseUrl = Environment.GetEnvironmentVariable("GOV_NOTIFY_URL");
            string apiKey = Environment.GetEnvironmentVariable("GOV_NOTIFY_API_KEY");

            INotificationClient client = new NotificationClient(baseUrl, apiKey);
            return client;
        }

        private static Dictionary<string, dynamic> NotificationPersonalisation(IOneTimeLinkNotification notification)
        {
            return new Dictionary<string, dynamic>
                {{"access_url", $"{notification.Url}?token={notification.Token}"}};
        }
    }
}
