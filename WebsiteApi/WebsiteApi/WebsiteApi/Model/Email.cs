namespace WebsiteApi.Model
{
    public record Email
    {
        public string EmailAddress { get; init; }
        
        public string EmailBody { get; init; }
    }
}