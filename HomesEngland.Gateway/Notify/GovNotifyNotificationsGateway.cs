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
            string baseUrl = Environment.GetEnvironmentVariable("GOV_NOTIFY_URL");
            string apiKey = Environment.GetEnvironmentVariable("GOV_NOTIFY_API_KEY");
            INotificationClient client = new NotificationClient(baseUrl, apiKey);
            var personalisation = new Dictionary<string, dynamic> {{"token", notification.Token}};
            
            client.SendEmail(notification.Email, "asdasd", personalisation);
            
            return true;
        }
    }
}
