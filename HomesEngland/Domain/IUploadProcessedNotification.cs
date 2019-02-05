namespace HomesEngland.Domain
{
    public interface IUploadProcessedNotification
    {
        string Email { get; set; }
        bool UploadSuccessfullyProcessed { get; set; }
    }
}
