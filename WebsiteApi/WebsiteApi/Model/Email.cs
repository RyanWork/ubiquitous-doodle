namespace WebsiteApi.Model
{
    public record Email
    {
        public Email(string emailAddress, string emailBody)
        {
            EmailAddress = emailAddress;
            EmailBody = emailBody;
        }

        public string EmailAddress { get; }
        
        public string EmailBody { get; }
    }
}