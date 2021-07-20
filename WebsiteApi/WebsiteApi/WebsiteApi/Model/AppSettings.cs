namespace WebsiteApi.Model
{
    public class AppSettings
    {
        public string KeyFilePath { get; set; } = string.Empty;

        public string ImpersonationUser { get; set; } = string.Empty;

        public string ServiceAccountEmail { get; set; } = string.Empty;

        public string DefaultSendAddress { get; set; } = string.Empty;
    }
}