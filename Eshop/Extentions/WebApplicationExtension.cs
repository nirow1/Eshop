using Eshop.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Eshop.Extentions
{
    public static class WebApplicationExtension
    {
        public static async Task RegisterAdmin( this WebApplication wedApplication, string userEmail, string userPassword)
        {
            var adminRoleName = "Admin";
            using(var scope = wedApplication.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                 if (!await roleManager.RoleExistsAsync(adminRoleName))
                    await roleManager.CreateAsync(new IdentityRole(adminRoleName));

                ApplicationUser user = await userManager.FindByEmailAsync(userEmail);

                if (user is null)
                {
                    user = await CreateUser(userManager, userEmail, userPassword);
                }

                if(!await userManager.IsInRoleAsync(user, adminRoleName))
                    await userManager.AddToRoleAsync(user, adminRoleName);
            }
        }
        public static async Task<ApplicationUser> CreateUser(UserManager<ApplicationUser> userManager, string userEmail, string password)
        {
            ApplicationUser user = null;
            var result  = await userManager.CreateAsync(new ApplicationUser { UserName = userEmail, Email = userEmail}, password);
            if (result.Succeeded)
            {
                user = await userManager.FindByEmailAsync(userEmail);
            }
            return user;
        }
    }
}
