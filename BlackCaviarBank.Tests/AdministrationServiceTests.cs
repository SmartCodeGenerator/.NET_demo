using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Business;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace BlackCaviarBank.Tests
{
    public class AdministrationServiceTests
    {
        [Fact]
        public async void BanUserProfile_SuccessForNoAdmin()
        {
            var testUser = new UserProfile { IsBanned = false };
            var store = new Mock<IUserRoleStore<UserProfile>>();
            store.Setup(s => s.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(testUser);
            store.Setup(s => s.GetRolesAsync(It.IsAny<UserProfile>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<string>() { "user" });
            store.Setup(s => s.UpdateAsync(It.IsAny<UserProfile>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Success).Callback(() => testUser.IsBanned = true);
            var userManager = new UserManager<UserProfile>(store.Object, null, null, null, null, null, null, null, null);
            var administrationService = new AdministrationService(null, userManager);

            await administrationService.BanUserProfile("");

            Assert.True(testUser.IsBanned);
        }

        [Fact]
        public async void BanUserProfile_NoChangesForAdmin()
        {
            var testUser = new UserProfile { IsBanned = false };
            var store = new Mock<IUserRoleStore<UserProfile>>();
            store.Setup(s => s.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(testUser);
            store.Setup(s => s.GetRolesAsync(It.IsAny<UserProfile>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<string>() { "admin" });
            store.Setup(s => s.UpdateAsync(It.IsAny<UserProfile>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Success).Callback(() => testUser.IsBanned = true);
            var userManager = new UserManager<UserProfile>(store.Object, null, null, null, null, null, null, null, null);
            var administrationService = new AdministrationService(null, userManager);

            await administrationService.BanUserProfile("");

            Assert.False(testUser.IsBanned);
        }

        [Fact]
        public async void BanUserProfile_UserNotFound()
        {
            var store = new Mock<IUserRoleStore<UserProfile>>();
            store.Setup(s => s.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);
            store.Setup(s => s.GetRolesAsync(null, It.IsAny<CancellationToken>())).ThrowsAsync(new ArgumentNullException());
            store.Setup(s => s.UpdateAsync(null, It.IsAny<CancellationToken>())).ThrowsAsync(new ArgumentNullException());
            var userManager = new UserManager<UserProfile>(store.Object, null, null, null, null, null, null, null, null);
            var administrationService = new AdministrationService(null, userManager);

            await Assert.ThrowsAsync(new ArgumentNullException().GetType(), () => administrationService.BanUserProfile(""));
        }

        [Fact]
        public async void DeleteUserProfile_UserNotAdmin()
        {
            var users = new List<UserProfile>()
            {
                new UserProfile { Id = "6315DEBD-4EFB-47D9-80B4-B3C42DB72059" },
                new UserProfile { Id = "FCB70CC5-D9E3-4396-B38E-1F9BB4DB2381" }
            };
            var prevCount = users.Count;
            var store = new Mock<IUserRoleStore<UserProfile>>();
            var userToDelete = users.Last();
            store.Setup(s => s.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(userToDelete);
            store.Setup(s => s.GetRolesAsync(It.IsAny<UserProfile>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<string>() { "user" });
            store.Setup(s => s.DeleteAsync(It.IsAny<UserProfile>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Success).Callback(() => users.Remove(userToDelete));
            var userManager = new UserManager<UserProfile>(store.Object, null, null, null, null, null, null, null, null);
            var administrationService = new AdministrationService(null, userManager);

            await administrationService.DeleteUserProfile("");

            Assert.NotEqual(prevCount, users.Count);
            Assert.DoesNotContain(userToDelete, users);
        }

        [Fact]
        public async void DeleteUserProfile_UserAdmin()
        {
            var users = new List<UserProfile>()
            {
                new UserProfile { Id = "D3EF52BC-58FD-42EE-97E0-D110A0A9707A" },
                new UserProfile { Id = "4F3743D9-214D-4CC4-BA71-204F34D17EAC" }
            };
            var prevCount = users.Count;
            var store = new Mock<IUserRoleStore<UserProfile>>();
            var userToDelete = users.Last();
            store.Setup(s => s.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(userToDelete);
            store.Setup(s => s.GetRolesAsync(It.IsAny<UserProfile>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<string>() { "admin" });
            store.Setup(s => s.DeleteAsync(It.IsAny<UserProfile>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Success).Callback(() => users.Remove(userToDelete));
            var userManager = new UserManager<UserProfile>(store.Object, null, null, null, null, null, null, null, null);
            var administrationService = new AdministrationService(null, userManager);

            await administrationService.DeleteUserProfile("");

            Assert.Equal(prevCount, users.Count);
            Assert.Contains(userToDelete, users);
        }

        [Fact]
        public async void DeleteUserProfile_UserNotFound()
        {
            var store = new Mock<IUserRoleStore<UserProfile>>();
            store.Setup(s => s.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);
            store.Setup(s => s.GetRolesAsync(null, It.IsAny<CancellationToken>())).ThrowsAsync(new ArgumentNullException());
            store.Setup(s => s.DeleteAsync(null, It.IsAny<CancellationToken>())).ThrowsAsync(new ArgumentNullException());
            var userManager = new UserManager<UserProfile>(store.Object, null, null, null, null, null, null, null, null);
            var administrationService = new AdministrationService(null, userManager);

            await Assert.ThrowsAsync(new ArgumentNullException().GetType(), () => administrationService.DeleteUserProfile(""));
        }

        [Fact]
        public async void GetUserProfileInfo_UserFound()
        {
            var users = new List<UserProfile>()
            {
                new UserProfile { Id = "D809E7C3-4E25-4A07-9CCD-6823EF34FDEE" }
            };
            var repo = new Mock<IRepository<UserProfile>>();
            repo.Setup(r => r.GetById(Guid.Parse("D809E7C3-4E25-4A07-9CCD-6823EF34FDEE"))).ReturnsAsync(users.FirstOrDefault(u => u.Id.Equals("D809E7C3-4E25-4A07-9CCD-6823EF34FDEE")));
            var service = new AdministrationService(repo.Object, null);

            var result = await service.GetUserProfileInfo("D809E7C3-4E25-4A07-9CCD-6823EF34FDEE");

            Assert.NotNull(result);
        }

        [Fact]
        public async void GetUserProfileInfo_UserNotFound()
        {
            var users = new List<UserProfile>()
            {
                new UserProfile { Id = "AADA83F1-A63C-4F2A-AF22-DA7E5E57C6F6" }
            };
            var repo = new Mock<IRepository<UserProfile>>();
            repo.Setup(r => r.GetById(Guid.Parse("62EE5B40-DDBC-4C19-953A-D465A6294B98"))).ReturnsAsync(users.FirstOrDefault(u => u.Id.Equals("62EE5B40-DDBC-4C19-953A-D465A6294B98")));
            var service = new AdministrationService(repo.Object, null);

            var result = await service.GetUserProfileInfo("62EE5B40-DDBC-4C19-953A-D465A6294B98");

            Assert.Null(result);
        }

        private static List<UserProfile> AppUsers()
        {
            return new List<UserProfile>()
            {
                new UserProfile { UserName = "user_01" },
                new UserProfile { UserName = "user_02" },
                new UserProfile { UserName = "user_03" },
                new UserProfile { UserName = "user_04" },
                new UserProfile { UserName = "user_05" },
                new UserProfile { UserName = "user_06" },
                new UserProfile { UserName = "user_07" },
                new UserProfile { UserName = "user_08" },
                new UserProfile { UserName = "user_09" },
                new UserProfile { UserName = "user_10" }
            };
        }

        [Fact]
        public async void GetUserProfiles_UsersPagedListCorrectParams()
        {
            var repo = new Mock<IRepository<UserProfile>>();
            repo.Setup(r => r.GetAll(It.IsAny<UserProfileParams>())).ReturnsAsync(new PagedList<UserProfile>(AppUsers(), AppUsers().Count, 2, 3));
            var service = new AdministrationService(repo.Object, null);

            var result = await service.GetUserProfiles(new UserProfileParams());

            Assert.NotEmpty(result);
            Assert.Equal(AppUsers().Count, result.TotalCount);
            Assert.True(result.HasNext);
            Assert.True(result.HasPrevious);
            Assert.Equal(4, result.TotalPages);
        }

        [Fact]
        public async void GetUserProfiles_UsersPagedListWrongParams()
        {
            var repo = new Mock<IRepository<UserProfile>>();
            repo.Setup(r => r.GetAll(It.IsAny<UserProfileParams>())).ReturnsAsync(new PagedList<UserProfile>(AppUsers(), AppUsers().Count, 4, 11));
            var service = new AdministrationService(repo.Object, null);

            var result = await service.GetUserProfiles(new UserProfileParams());

            Assert.NotEmpty(result);
            Assert.Equal(AppUsers().Count, result.TotalCount);
            Assert.False(result.HasNext);
            Assert.True(result.HasPrevious);
            Assert.Equal(1, result.TotalPages);
        }

        [Fact]
        public async void UnbanUserProfile_UserFound()
        {
            var testUser = new UserProfile { IsBanned = true };
            var store = new Mock<IUserRoleStore<UserProfile>>();
            store.Setup(s => s.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(testUser);
            store.Setup(s => s.UpdateAsync(It.IsAny<UserProfile>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Success).Callback(() => testUser.IsBanned = false);
            var userManager = new UserManager<UserProfile>(store.Object, null, null, null, null, null, null, null, null);
            var administrationService = new AdministrationService(null, userManager);

            await administrationService.UnbanUserProfile("");

            Assert.False(testUser.IsBanned);
        }

        [Fact]
        public async void UnbanUserProfile_UserNotFound()
        {
            var store = new Mock<IUserRoleStore<UserProfile>>();
            store.Setup(s => s.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);
            store.Setup(s => s.UpdateAsync(null, It.IsAny<CancellationToken>())).ThrowsAsync(new NullReferenceException());
            var userManager = new UserManager<UserProfile>(store.Object, null, null, null, null, null, null, null, null);
            var administrationService = new AdministrationService(null, userManager);

            await Assert.ThrowsAsync(new NullReferenceException().GetType(), () => administrationService.UnbanUserProfile(""));
        }
    }
}
