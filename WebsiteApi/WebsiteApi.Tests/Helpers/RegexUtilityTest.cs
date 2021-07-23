using FluentAssertions;
using WebsiteApi.Common;
using Xunit;

namespace WebsiteApi.Tests.Helpers
{
    public class RegexUtilityTest
    {
        [Theory]
        [InlineData("ryanha@test.com")]
        [InlineData("a@a.com")]
        [InlineData("InternåtionålCharacters@test.cöm")]
        public void IsValidEmail_ValidEmail_ShouldReturnTrue(string emailAddress)
        {
            var result = emailAddress.IsValidEmail();

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("ClearlyNotAnEmail")]
        [InlineData("@")]
        [InlineData(".com")]
        [InlineData("@.com")]
        [InlineData("onlyEmailAddress@.com")]
        [InlineData("@onlyDomainName.com")]
        public void IsValidEmail_BadEmail_ShouldReturnFalse(string emailAddress)
        {
            var result = emailAddress.IsValidEmail();

            result.Should().BeFalse();
        }
    }
}