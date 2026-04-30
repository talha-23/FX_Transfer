using FXTransfer.Data;
using FXTransfer.Models.Entities;
using FXTransfer.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FXTransfer.Seeders;

/// <summary>
/// SRP: Handles seeding initial admin user and demo accounts
/// </summary>
public static class AdminSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Create roles if they don't exist
        foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
        {
            var roleName = role.ToString();
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Seed Admin User
        var adminEmail = "admin@fxtransfer.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Administrator",
                EmailConfirmed = true,
                RegisteredAt = DateTime.UtcNow,
                ReferralCode = GenerateReferralCode(),
                IsPremium = true,
                PremiumExpiry = DateTime.UtcNow.AddYears(10)
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, UserRole.Admin.ToString());
                Console.WriteLine("Admin user created successfully");
            }
        }

        // Seed Premium User
        var premiumEmail = "premium@fxtransfer.com";
        var premiumUser = await userManager.FindByEmailAsync(premiumEmail);

        if (premiumUser == null)
        {
            premiumUser = new ApplicationUser
            {
                UserName = premiumEmail,
                Email = premiumEmail,
                FullName = "Premium Demo User",
                EmailConfirmed = true,
                RegisteredAt = DateTime.UtcNow,
                ReferralCode = GenerateReferralCode(),
                IsPremium = true,
                PremiumExpiry = DateTime.UtcNow.AddMonths(6)
            };

            var result = await userManager.CreateAsync(premiumUser, "Premium@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(premiumUser, UserRole.Premium.ToString());
                await SeedUserWallets(context, premiumUser.Id);
                Console.WriteLine("Premium user created successfully");
            }
        }

        // Seed Regular User
        var regularEmail = "user@fxtransfer.com";
        var regularUser = await userManager.FindByEmailAsync(regularEmail);

        if (regularUser == null)
        {
            regularUser = new ApplicationUser
            {
                UserName = regularEmail,
                Email = regularEmail,
                FullName = "Regular Demo User",
                EmailConfirmed = true,
                RegisteredAt = DateTime.UtcNow,
                ReferralCode = GenerateReferralCode(),
                IsPremium = false
            };

            var result = await userManager.CreateAsync(regularUser, "User@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(regularUser, UserRole.Regular.ToString());
                await SeedUserWallets(context, regularUser.Id);
                Console.WriteLine("Regular user created successfully");
            }
        }

        // Seed Suspended User
        var suspendedEmail = "suspended@fxtransfer.com";
        var suspendedUser = await userManager.FindByEmailAsync(suspendedEmail);

        if (suspendedUser == null)
        {
            suspendedUser = new ApplicationUser
            {
                UserName = suspendedEmail,
                Email = suspendedEmail,
                FullName = "Suspended Demo User",
                EmailConfirmed = true,
                RegisteredAt = DateTime.UtcNow,
                ReferralCode = GenerateReferralCode(),
                IsSuspended = true,
                IsPremium = false
            };

            var result = await userManager.CreateAsync(suspendedUser, "Suspended@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(suspendedUser, UserRole.Suspended.ToString());
                Console.WriteLine("Suspended user created successfully");
            }
        }

        await context.SaveChangesAsync();
    }

    private static string GenerateReferralCode()
    {
        return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
    }

    private static async Task SeedUserWallets(ApplicationDbContext context, string userId)
    {
        var defaultCurrencies = new[] { "USD", "EUR", "GBP", "PKR" };

        foreach (var currency in defaultCurrencies)
        {
            if (!context.Wallets.Any(w => w.UserId == userId && w.CurrencyCode == currency))
            {
                context.Wallets.Add(new Wallet
                {
                    UserId = userId,
                    CurrencyCode = currency,
                    Balance = currency == "USD" ? 5000 : 0
                });
            }
        }

        await context.SaveChangesAsync();
    }
}