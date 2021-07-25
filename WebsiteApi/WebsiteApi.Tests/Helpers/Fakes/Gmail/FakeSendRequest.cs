using System.Diagnostics.CodeAnalysis;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;

namespace WebsiteApi.Tests.Helpers.Fakes.Gmail
{
    [ExcludeFromCodeCoverage]
    public class FakeSendRequest : UsersResource.MessagesResource.SendRequest
    {
        public FakeSendRequest(IClientService service, Message body, string userId) : base(service, body, userId)
        {
        }
    }
}