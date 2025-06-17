namespace TanTamApi.Models.Response
{
    public class UploadFileResponse
    {
        public UploadFileResponseCode Code { get; set; }
        public string Path { get; set; }
        public string Message { get; set; }
    }

    public enum UploadFileResponseCode
    {
        Success = 0,
        Fail = 1
    }
}