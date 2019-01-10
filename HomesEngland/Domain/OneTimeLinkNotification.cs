namespace HomesEngland.Domain
{
    public class OneTimeLinkNotification : IOneTimeLinkNotification
    {
        public string Email { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
    }
}
