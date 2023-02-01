using BookStore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace BookStore.Services.Tests.Unit.ClassFixtures
{
    public class UserFixture : IDisposable
    {
        public UserService userService;
        public Mock<UserManager<User>> userManager;
        public Mock<RoleManager<Role>> roleManager;
        public Mock<ILogger<User>> logger;
        public Mock<IJwtService> jwtService;
        public Mock<IMemoryCache> memoryCache;
        public UserFixture()
        {
            userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            roleManager = new Mock<RoleManager<Role>>(Mock.Of<IRoleStore<Role>>(), null, null, null, null);
            logger = new Mock<ILogger<User>>();
            jwtService = new Mock<IJwtService>();
            memoryCache = new Mock<IMemoryCache>();
            userService = new UserService(userManager.Object, roleManager.Object, logger.Object, jwtService.Object, memoryCache.Object);
        }
        public void Dispose()
        {
            userManager.Object.Dispose();
            roleManager.Object.Dispose();
            memoryCache.Object.Dispose();
            userService.Dispose();
        }
    }
}
