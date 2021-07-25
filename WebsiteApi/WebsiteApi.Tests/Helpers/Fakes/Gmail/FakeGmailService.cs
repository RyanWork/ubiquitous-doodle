using System.Diagnostics.CodeAnalysis;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;

namespace WebsiteApi.Tests.Helpers.Fakes.Gmail
{
    [ExcludeFromCodeCoverage]
    public class FakeGmailService : GmailService
    {
        public override UsersResource Users { get; }

        public FakeGmailService(IClientService clientService)
        {
            Users = new FakeUsersResource(clientService);
        }
    }
}