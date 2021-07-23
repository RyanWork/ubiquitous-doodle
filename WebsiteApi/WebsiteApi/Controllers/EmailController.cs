using System;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using WebsiteApi.Model;

namespace WebsiteApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmailController
    {
        private static readonly string[] Scopes = { GmailService.Scope.GmailCompose, GmailService.Scope.GmailSend };
        
        private readonly GmailService _gmailService;

        private readonly AppSettings _appSettings;
        
        public EmailController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            ServiceAccountCredential serviceAccountCredential;
            using (var stream = new FileStream(_appSettings.KeyFilePath, FileMode.Open, FileAccess.Read))
            {
                serviceAccountCredential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes)
                    .CreateWithUser(_appSettings.ImpersonationUser)
                    .UnderlyingCredential as ServiceAccountCredential;
            }
            
            _gmailService = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = serviceAccountCredential,
                ApplicationName = "Ryan Portfolio",
            });
        }
        
        /// <summary>
        /// Send an email to the predefined email address
        /// </summary>
        /// <param name="email">The <see cref="Email"/> object containing the email address and the body</param>
        /// <param name="cancellationToken">Cancellation token if the request is cancelled</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SendEmailAsync([FromBody] Email email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email.EmailAddress) || string.IsNullOrEmpty(email.EmailBody))
            {
                return new BadRequestResult();
            }

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_appSettings.ServiceAccountEmail);
            mailMessage.To.Add(_appSettings.DefaultSendAddress);
            mailMessage.Subject = $"Profile: { email.EmailAddress }";
            mailMessage.Body = email.EmailBody;
            mailMessage.IsBodyHtml = false;

            var mimeMessage = MimeMessage.CreateFromMailMessage(mailMessage);
            await using (var ms = new MemoryStream())
            {
                await mimeMessage.WriteToAsync(ms, cancellationToken);
                var sendRequest = _gmailService.Users.Messages.Send(new Message()
                {
                    Raw = Convert.ToBase64String(ms.ToArray())
                }, "me");

                await sendRequest.ExecuteAsync(cancellationToken);
            }
            
            return new NoContentResult();
        }
    }
}