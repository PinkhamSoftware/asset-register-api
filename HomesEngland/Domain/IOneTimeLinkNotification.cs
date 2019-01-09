namespace HomesEngland.Domain
{
    public interface IOneTimeLinkNotification
    {
        string Email { get; set; }
        string Token { get; set; }
    }
}
