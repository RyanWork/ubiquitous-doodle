using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Gmail.v1;

namespace WebsiteApi.Common.ServiceAccountFactory
{
    public interface IServiceAccountFactory
    {
        Task<GmailService> CreateGmailServiceAsync(string keyFilePath, string applicationName, string impersonationUser, IEnumerable<string> scopes, CancellationToken cancellationToken);
    }
}