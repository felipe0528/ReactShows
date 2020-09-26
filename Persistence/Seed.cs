using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;

namespace Persistence
{
    public class Seed
    {
        public static async System.Threading.Tasks.Task SeedDataAsync(DataContext ctx,
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager)
        {
            //SEED ROLES
            if (!ctx.Roles.Any())
            {
                var roleUser = new IdentityRole
                {
                    Name = "User"
                };
                var resultUser = roleManager.CreateAsync(roleUser).Result;

                var roleAdmin = new IdentityRole
                {
                    Name = "Administrator"
                };
                var resultAdmin = roleManager.CreateAsync(roleAdmin).Result;
            }


            //SEED ADMIN
            string adminEmail = "admin@reactshows.com";
            string adminPass = "@Dmin01";

            if (userManager.FindByNameAsync(adminEmail).Result == null)
            {
                var user = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };
                var result = userManager.CreateAsync(user, adminPass).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                    await userManager.ConfirmEmailAsync(user, code);
                }
            }

        }
    }
}
