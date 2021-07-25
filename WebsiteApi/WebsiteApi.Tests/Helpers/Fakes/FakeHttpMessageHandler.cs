using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebsiteApi.Tests.Helpers.Fakes
{
    [ExcludeFromCodeCoverage]
    public abstract class FakeHttpMessageHandler : HttpMessageHandler
    {
        public virtual HttpResponseMessage Send(HttpRequestMessage _)
        {
            throw new NotImplementedException("Class must be mocked and Send method must be setup.");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }
    }
}