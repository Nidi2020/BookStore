using BookStore.Entities;
using BookStore.Models.Dtos;
using BookStore.Services.Tests.Unit.ClassFixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NSubstitute;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;
using Xunit;

namespace BookStore.Services.Tests.Unit
{
    //[Collection("")]
    public class UserServiceTests : IClassFixture<UserFixture>
    {
        private UserService _userService;
        private Mock<UserManager<User>> _userManager;
        private Mock<RoleManager<Role>> _roleManager;
        private Mock<IJwtService> _jwtService;

        public UserServiceTests(UserFixture userFixture)
        {
            _userManager = userFixture.userManager;
            _roleManager = userFixture.roleManager;
            _userService = userFixture.userService;
            _jwtService = userFixture.jwtService;
        }

        [Fact]
        public async Task Should_Register()
        {
            //arrange
            var command = SomeCreateUser();
            _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), command.Password)).Returns(Task.FromResult(IdentityResult.Success));
            _roleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(new Role()));
            _roleManager.Setup(x => x.CreateAsync(It.IsAny<Role>())).Returns(Task.FromResult(IdentityResult.Success));
            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));

            //act
            var expected = await _userService.Register(command, System.Threading.CancellationToken.None);

            //assert
            expected.IsSuccess.Should().Be(true);
        }

        private static RegisterDto SomeCreateUser()
        {
            var filler = new Filler<RegisterDto>();
            filler.Setup().OnProperty(x => x.Email).Use("neda@gmail.com");
            filler.Setup().OnProperty(x => x.Password).Use("123456");
            filler.Setup().OnProperty(x => x.Mobile).Use("09365036239");
            filler.Setup().OnProperty(x => x.Age).Use(33);
            return filler.Create();
        }

        [Fact]
        public async Task Should_GetUserById()
        {
            //arrange
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new User()));

            //act
            var result = await _userService.GetUserById(Arg.Any<int>());

            //assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Login_ReturenJwtToken()
        {
            //arrange
            const string userName = "neda.mohammadi88@gmail.com";
            const string password = "123456a";
            var user = new User() { Id = 1, Name = "neda", Age = 33, Email = userName, PasswordHash = password };

            _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(new User()));
            _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(true));

            _jwtService.Setup(x => x.GenerateAsync(user)).Returns((string s) => Task.FromResult(s));

            //act
            var result = await _userService.Login(userName, password, System.Threading.CancellationToken.None);

            //assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Should_GetUserScore_ReturnScore()
        {
            //arrange
            const int age = 33;

            //act
            var result = _userService.GetUserScore(age);

            //assert
            result.Should().Be(30);
        }
    }
}