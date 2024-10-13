using Microsoft.AspNetCore.Identity;

namespace UserService.Data.Seeders;

public class RoleSeeder
{
    private static readonly string[] Roles = new[] { "User", "Admin", "RestaurantOwner", "Courier", "Support" };

    public static async Task SeedRoes(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}