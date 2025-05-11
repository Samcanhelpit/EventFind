using EventFind.Models;
using Microsoft.AspNetCore.Identity;

namespace EventFind.Services
{
    public class AdminAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminAccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SetupAdminAccountAsync()
        
        {
            string adminEmail = "samuelc3627@outlook.com";
            string adminUsername = "Admin@gmail.com";
            string adminRole = "Admin";
            string fixedPassword = "Admin@12345"; // Fixed password

            // Check if the admin user already exists
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                // Create the admin user with the fixed password
                adminUser = new ApplicationUser
                {
                    FullName = adminEmail,
                    UserName = adminUsername,
                    Email = adminEmail,
                    EmailConfirmed = true // Optional: Confirm email automatically
                };

                var result = await _userManager.CreateAsync(adminUser, fixedPassword);
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                // Create the Admin role if it doesn't exist
                if (!await _roleManager.RoleExistsAsync(adminRole))
                {
                    var role = new IdentityRole(adminRole);
                    var roleResult = await _roleManager.CreateAsync(role);
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception($"Failed to create admin role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                }

                // Assign the Admin role to the user
                var roleAssignResult = await _userManager.AddToRoleAsync(adminUser, adminRole);
                if (!roleAssignResult.Succeeded)
                {
                    throw new Exception($"Failed to assign admin role: {string.Join(", ", roleAssignResult.Errors.Select(e => e.Description))}");
                }

                Console.WriteLine($"Admin account created with email: {adminEmail}, username: {adminUsername}, password: {fixedPassword}");
            }
        }
    }
}
