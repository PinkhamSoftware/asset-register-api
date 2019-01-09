namespace HomesEngland.Domain
{
    public interface IOneTimeLinkNotification
    {
        string Email { get; }
        string Url { get; set; }
        string Token { get; }
    }
}
