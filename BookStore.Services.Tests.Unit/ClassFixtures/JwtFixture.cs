using BookStore.Common.Settings;
using BookStore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System;

namespace BookStore.Services.Tests.Unit.ClassFixtures
{
    public class JwtFixture : IDisposable
    {
        public JwtService jwtService;
        public Mock<IOptionsSnapshot<SiteSettings>> siteSetting;
        public Mock<SignInManager<User>> signInManager;
        public JwtFixture()
        {
            siteSetting = new Mock<IOptionsSnapshot<SiteSettings>>();
            var userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            signInManager = new Mock<SignInManager<User>>(userManager.Object,
                                                          Mock.Of<IHttpContextAccessor>(),
                                                          Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null);

            jwtService = new JwtService(siteSetting.Object, signInManager.Object);
        }
        public void Dispose()
        {
            
        }
    }
}
