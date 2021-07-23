using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Services;

namespace WebsiteApi.Common.ServiceAccountFactory
{
    public interface IServiceAccountFactory
    {
        Task<IClientService> CreateGmailServiceAsync(string keyFilePath, string applicationName, string impersonationUser, IEnumerable<string> scopes, CancellationToken cancellationToken);
    }
}