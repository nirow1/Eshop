using Eshop.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Eshop.Extentions
{
    public static class WebApplicationExtension
    {
        private static async Task RegisterAdmin( this WebApplication wedApplication, string userEmail, string userPassword)
        {

        }
        private static async Task<ApplicationUser> CreateUser(UserManager<ApplicationUser> userManager, string userEmail, string password)
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
