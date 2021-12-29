using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Google.Apis.Gmail.v1;
using Google.Apis.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using WebsiteApi.Common.ServiceAccountFactory;
using WebsiteApi.Controllers;
using WebsiteApi.Model;
using WebsiteApi.Tests.Helpers.Fakes;
using WebsiteApi.Tests.Helpers.Fakes.Gmail;
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

            _mockServiceAccountFactory = _fixture.Freeze<Mock<IServiceAccountFactory>>();

            _sut = _fixture.Create<EmailController>();
        }
        
        [Fact]
        public async Task SendEmailAsync_ValidEmail_ShouldSendEmailAndReturnNoContent()
        {
            var fakeEmail = CreateValidEmail();
            var fakeGmailService = CreateFakeGmailService();
            _mockServiceAccountFactory.Setup(x => x.CreateGmailServiceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeGmailService);
            
            var result = await _sut.SendEmailAsync(fakeEmail, It.IsAny<CancellationToken>());
            
            _mockServiceAccountFactory.Verify(x => x.CreateGmailServiceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(new NoContentResult());
        }

        [Fact]
        public async Task SendEmailAsync_BadEmail_ReturnsBadRequest()
        {
            Email? fakeEmail = null;

            var result = await _sut.SendEmailAsync(fakeEmail!, It.IsAny<CancellationToken>());

            result.Should().BeEquivalentTo(new BadRequestResult());
        }

        private Email CreateValidEmail() =>
            _fixture.Build<Email>()
                .With(x => x.EmailAddress, "someInterestedPerson@test.com")
                .With(x => x.EmailBody, "some interesting piece of information!")
                .Create();

        private GmailService CreateFakeGmailService()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = _fixture.Build<Mock<FakeHttpMessageHandler>>()
                .With(x => x.CallBase, true)
                .Create();
            fakeHttpMessageHandler.Setup(x => x.Send(It.IsAny<HttpRequestMessage>()))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"success\": true,\"error-codes\": [\"\"]}")
                });
            
            FakeClientService fakeClientService = _fixture.Build<FakeClientService>()
                .With(x => x.HttpClient, new ConfigurableHttpClient(new ConfigurableMessageHandler(fakeHttpMessageHandler.Object)))
                .Create();

            return new FakeGmailService(fakeClientService);
        }
    }
}