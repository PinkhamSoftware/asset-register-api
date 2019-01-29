namespace HomesEngland.UseCase.SaveUploadedAssetRegisterFile.Models
{
    public class SaveAssetRegisterFileRequest
    {
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
    }
}
