namespace WebsiteApi.Model
{
    public record Email
    {
        public string? EmailAddress { get; set; } = string.Empty;
        
        public string? EmailBody { get; set; } = string.Empty;
    }
}