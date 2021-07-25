using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis;
using Google.Apis.Http;
using Google.Apis.Json;
using Google.Apis.Requests;
using Google.Apis.Services;
using Moq;

namespace WebsiteApi.Tests.Helpers.Fakes.Gmail
{
    [ExcludeFromCodeCoverage]
    public class FakeClientService : IClientService
    {
        public ConfigurableHttpClient HttpClient { get; set; } = null!;
        
        public IConfigurableHttpClientInitializer HttpClientInitializer => null!;
        
        public string Name => "MockClientService";
        
        public string BaseUri => "https://ryanha.dev";
        
        public string BasePath => "MockClientService";
        
        public IList<string> Features { get; } = new List<string>();
        
        public bool GZipEnabled => false;
        
        public string ApiKey => string.Empty;
        
        public string ApplicationName => "MockClientService";
        
        public ISerializer Serializer { get; } = new NewtonsoftJsonSerializer();
        
        public void SetRequestSerailizedContent(HttpRequestMessage request, object body)
        {
            // NO-OP
        }

        public string SerializeObject(object data)
        {
            return string.Empty;
        }

        public Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            return Task.FromResult(It.IsAny<T>());
        }

        public Task<RequestError> DeserializeError(HttpResponseMessage response)
        {
            return null!;
        }
        
        public void Dispose()
        {
            // NO-OP
        }
    }
}