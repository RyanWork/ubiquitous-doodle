using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;

namespace WebsiteApi.Common.ServiceAccountFactory
{
    public class ServiceAccountFactory : IServiceAccountFactory
    {
        private readonly SemaphoreSlim _cacheLock = new(1, 1);

        private readonly IDictionary<string, GmailService> _gmailServiceCache = new Dictionary<string, GmailService>();
        
        public async Task<GmailService> CreateGmailServiceAsync(string keyFilePath, string applicationName, string impersonationUser, IEnumerable<string> scopes, CancellationToken cancellationToken)
        {
            await _cacheLock.WaitAsync(cancellationToken);
            try
            {
                if (_gmailServiceCache.TryGetValue(keyFilePath, out GmailService? gmailService))
                    return gmailService;
                
                await using FileStream keyFileStream = new(keyFilePath, FileMode.Open, FileAccess.Read);
                ServiceAccountCredential? serviceAccountCredential = await CreateServiceAccountCredentialAsync(keyFileStream, impersonationUser, scopes, cancellationToken);
                gmailService = new GmailService(
                    new BaseClientService.Initializer
                    {
                        HttpClientInitializer = serviceAccountCredential,
                        ApplicationName = applicationName
                    });
                
                _gmailServiceCache.Add(keyFilePath, gmailService);

                return gmailService;
            }
            finally
            {
                _cacheLock.Release();
            }
        }
        
        private async Task<ServiceAccountCredential?> CreateServiceAccountCredentialAsync(Stream keyFileStream, string impersonationUser, IEnumerable<string> scopes, CancellationToken cancellationToken)
        {
            var googleCredential = await GoogleCredential.FromStreamAsync(keyFileStream, cancellationToken);
            var serviceAccountCredential = googleCredential.CreateScoped(scopes)
                .CreateWithUser(impersonationUser)
                .UnderlyingCredential as ServiceAccountCredential;

            return serviceAccountCredential;
        }
    }
}