using BlackCaviarBank.Domain.Core;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Data.DbSeeders
{
    public class DbSeeder 
    {
        public static async Task SeedDb(UserManager<UserProfile> userManager, RoleManager<IdentityRole> roleManager)
        {
            var admin = new UserProfile { UserName = "admin1", Email = "alexjfr112@gmail.com", FirstName = "Alex", LastName = "Mytryniuk" };
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync("admin1") == null)
            {
                admin.ProfileImage = System.IO.File.ReadAllBytes("./MyStaticFiles/images/user_icon.svg");
                var result = await userManager.CreateAsync(admin, "admin11");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
