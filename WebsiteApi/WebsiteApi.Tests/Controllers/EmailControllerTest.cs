using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using WebsiteApi.Common.ServiceAccountFactory;
using WebsiteApi.Controllers;
using WebsiteApi.Model;
using Xunit;

namespace WebsiteApi.Tests.Controllers
{
    public class EmailControllerTest
    {
        private readonly IFixture _fixture;
        
        private readonly EmailController _sut;
        
        private readonly Mock<IServiceAccountFactory> _mockServiceAccountFactory;
        
        public EmailControllerTest()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            
            AppSettings fakeAppSettings = _fixture.Build<AppSettings>()
                .With(x => x.ImpersonationUser, "fakeImpersonationUser")
                .With(x => x.KeyFilePath, "fakePath")
                .With(x => x.DefaultSendAddress, "fakeDefaultSendAddress@test.com")
                .With(x => x.ServiceAccountEmail, "fakeServiceAccountEmail@test.com")
                .Create();
            _fixture.Inject(Options.Create(fakeAppSettings));

            _mockServiceAccountFactory = _fixture.Create<Mock<IServiceAccountFactory>>();
            _fixture.Inject(_mockServiceAccountFactory.Object);

            _sut = _fixture.Create<EmailController>();
        }
        
        [Fact]
        public async Task SendEmailAsync_ValidEmail_ShouldSendEmail()
        {
            var fakeEmail = CreateValidEmail();
            _mockServiceAccountFactory.Setup(x => x.CreateGmailServiceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_fixture.Create<GmailService>());
            
            var result = await _sut.SendEmailAsync(fakeEmail, It.IsAny<CancellationToken>());
            
            _mockServiceAccountFactory.Verify(x => x.CreateGmailServiceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(new NoContentResult());
        }

        [Fact]
        public async Task SendEmailAsync_BadGmailService_ReturnsInternalServerError()
        {
            var fakeEmail = CreateValidEmail();
            _mockServiceAccountFactory.Setup(x => x.CreateGmailServiceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(value: null);
            
            var result = await _sut.SendEmailAsync(fakeEmail, It.IsAny<CancellationToken>());

            result.Should().BeEquivalentTo(new StatusCodeResult((int)HttpStatusCode.InternalServerError));
        }

        private Email CreateValidEmail() =>
            _fixture.Build<Email>()
                .With(x => x.EmailAddress, "someInterestedPerson@test.com")
                .With(x => x.EmailBody, "some interesting piece of information!")
                .Create();
    }
}