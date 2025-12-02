using BookStoreMVCUI.Constants;
using Microsoft.AspNetCore.Identity;

namespace BookStoreMVCUI.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider serviceProvider)
        {
            try
            {
                var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
                var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

                var adminRoleExist = await roleManager.RoleExistsAsync(Roles.Admin.ToString());
                if(!adminRoleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                }

                var userRoleExist = await roleManager.RoleExistsAsync(Roles.User.ToString());
                if (!userRoleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
                }


                var admin = new IdentityUser()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true
                };

                var adminInDb = await userManager.FindByEmailAsync(admin.Email);
                if (adminInDb == null)
                {
                    await userManager.CreateAsync(admin, "Admin@123"); // Best Practice To Put The Password at The Enviroment Variables
                    await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }




        }
    }
}
