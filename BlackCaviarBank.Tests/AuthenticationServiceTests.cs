using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Business;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace BlackCaviarBank.Tests
{
    public class AuthenticationServiceTests
    {
        [Fact]
        public async void Authenticate_PasswordVerificationSucceedTokenNotEmpty()
        {
            var user = new UserProfile { Id = "4C768FDC-B4BD-4481-B7B9-9960951938EA", UserName = "Alex" };
            var userRoleStore = new Mock<IUserRoleStore<UserProfile>>();
            userRoleStore.Setup(st => st.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            userRoleStore.Setup(m => m.GetRolesAsync(It.IsAny<UserProfile>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<string>() { "admin" });
            var passHasher = new Mock<IPasswordHasher<UserProfile>>();
            passHasher.Setup(ph=>ph.VerifyHashedPassword(It.IsAny<UserProfile>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Success);
            var userManager = new UserManager<UserProfile>(userRoleStore.Object, null, passHasher.Object, null, new List<PasswordValidator<UserProfile>>(), null, null, null, null);
            var authService = new AuthenticationService(userManager);
            
            var token = await authService.Authenticate(new LoginUserDTO { UserName = "Alex", Password = "mypass12345" });

            Assert.NotEqual("", token);
        }

        [Fact]
        public async void Authenticate_PasswordVerificationFailedTokenEmpty()
        {
            var user = new UserProfile { Id = "4C768FDC-B4BD-4481-B7B9-9960951938EA", UserName = "Alex" };
            var userRoleStore = new Mock<IUserRoleStore<UserProfile>>();
            userRoleStore.Setup(st => st.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            userRoleStore.Setup(m => m.GetRolesAsync(It.IsAny<UserProfile>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<string>() { "admin" });
            var passHasher = new Mock<IPasswordHasher<UserProfile>>();
            passHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<UserProfile>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Failed);
            var userManager = new UserManager<UserProfile>(userRoleStore.Object, null, passHasher.Object, null, new List<PasswordValidator<UserProfile>>(), null, null, null, null);
            var authService = new AuthenticationService(userManager);

            var token = await authService.Authenticate(new LoginUserDTO { UserName = "Alex", Password = "mypass12345" });

            Assert.Equal("", token);
        }
    }
}
