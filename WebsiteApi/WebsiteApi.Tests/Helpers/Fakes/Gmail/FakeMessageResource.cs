using System.Diagnostics.CodeAnalysis;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;

namespace WebsiteApi.Tests.Helpers.Fakes.Gmail
{
    [ExcludeFromCodeCoverage]
    public class FakeMessageResource : UsersResource.MessagesResource
    {
        private readonly IClientService _clientService;
        
        public FakeMessageResource(IClientService clientService) : base(clientService)
        {
            _clientService = clientService;
        }

        public override SendRequest Send(Message body, string userId)
        {
            return new FakeSendRequest(_clientService, body, userId);
        }
    }
}