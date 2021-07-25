using System.Diagnostics.CodeAnalysis;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;

namespace WebsiteApi.Tests.Helpers.Fakes.Gmail
{
    [ExcludeFromCodeCoverage]
    public class FakeUsersResource : UsersResource
    {
        public override MessagesResource Messages { get; }

        public FakeUsersResource(IClientService clientService) : base(clientService)
        {
            Messages = new FakeMessageResource(clientService);
        }
    }
}