using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
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
using WebsiteApi.Common;
using WebsiteApi.Common.ServiceAccountFactory;
using WebsiteApi.Model;

namespace WebsiteApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmailController
    {
        private static readonly string[] Scopes = { GmailService.Scope.GmailCompose, GmailService.Scope.GmailSend };

        private const string ApplicationName = "Ryan Portfolio";
        
        private readonly AppSettings _appSettings;

        private readonly IServiceAccountFactory _serviceAccountFactory;
        
        public EmailController(IOptions<AppSettings> appSettings, IServiceAccountFactory serviceAccountFactory)
        {
            _appSettings = appSettings.Value;
            _serviceAccountFactory = serviceAccountFactory;
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
        public async Task<IActionResult> SendEmailAsync([FromBody] Email? email, CancellationToken cancellationToken)
        {
            if (!ValidatePostedEmail(email))
                return new BadRequestResult();
            
            IClientService? clientService = await _serviceAccountFactory.CreateGmailServiceAsync(_appSettings.KeyFilePath, ApplicationName, _appSettings.ImpersonationUser, Scopes, cancellationToken);
            if (clientService is not GmailService gmailService)
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_appSettings.ServiceAccountEmail);
            mailMessage.To.Add(_appSettings.DefaultSendAddress);
            mailMessage.Subject = $"Profile: {email.EmailAddress}";
            mailMessage.Body = email.EmailBody;
            mailMessage.IsBodyHtml = false;

            var mimeMessage = MimeMessage.CreateFromMailMessage(mailMessage);
            await using (var ms = new MemoryStream())
            {
                await mimeMessage.WriteToAsync(ms, cancellationToken);
                var sendRequest = gmailService.Users.Messages.Send(new Message
                {
                    Raw = Convert.ToBase64String(ms.ToArray())
                }, "me");

                await sendRequest.ExecuteAsync(cancellationToken);
            }

            return new NoContentResult();
        }

        private static bool ValidatePostedEmail([NotNullWhen(true)] Email? email)
        {
            if (string.IsNullOrEmpty(email?.EmailAddress) || string.IsNullOrEmpty(email?.EmailBody))
                return false;

            return email.EmailAddress.IsValidEmail();
        }
    }
}