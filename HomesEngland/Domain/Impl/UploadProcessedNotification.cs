namespace HomesEngland.Domain.Impl
{
    public class UploadProcessedNotification : IUploadProcessedNotification
    {
        public string Email { get; set; }
        public bool UploadSuccessfullyProcessed { get; set; }
    }
}
