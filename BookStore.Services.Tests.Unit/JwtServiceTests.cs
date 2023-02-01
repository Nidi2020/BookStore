using BookStore.Common.Settings;
using BookStore.Entities;
using BookStore.Services.Tests.Unit.ClassFixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Services.Tests.Unit
{
    public class JwtServiceTests : IClassFixture<JwtFixture>
    {
        private JwtService _jwtService;
        private Mock<IOptionsSnapshot<SiteSettings>> _siteSetting;
        private Mock<SignInManager<User>> _signInManager;
        public JwtServiceTests(JwtFixture jwtFixture)
        {
            _jwtService = jwtFixture.jwtService;
            _siteSetting = jwtFixture.siteSetting;
            _signInManager = jwtFixture.signInManager;
        }

        [Fact]
        public async Task WhenGenerateAsyncCallShouldReturnTokenString()
        {
            //arrange
            var user = new User() { Id = 1, Name = "neda", Age = 33, Email = "neda@gmail.com" };
            var options = new SiteSettings()
            {
                JwtSettings = new JwtSettings()
                {
                    SecretKey = "LongerThan-16Char-SecretKey",
                    Encryptkey = "16CharEncryptKey",
                    Audience = "MyWebsite",
                    ExpirationMinutes = 60,
                    Issuer = "MyWebsite",
                    NotBeforeMinutes = 0
                }
            };
            _siteSetting.Setup(m => m.Value).Returns(options);

            //act
            var result = await _jwtService.GenerateAsync(user);

            //assert
            result.Should().NotBeNull();
        }
    }
}
