using FXTransfer.Data;
using FXTransfer.Models.Entities;
using FXTransfer.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FXTransfer.Seeders;

/// <summary>
/// SRP: Handles seeding initial admin user and demo accounts with wallets
/// </summary>
public static class AdminSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Create roles if they don't exist
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                var roleName = role.ToString();
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    logger.LogInformation($"Created role: {roleName}");
                }
            }

            // Seed Admin User
            await CreateUserWithWallets(
                userManager, context, logger,
                "admin@fxtransfer.com", "Admin@123",
                "General Asim Munir", UserRole.Admin,
                isPremium: true,
                premiumExpiry: DateTime.UtcNow.AddYears(10),
                walletBalance: 50000);

            // Seed Premium User
            await CreateUserWithWallets(
                userManager, context, logger,
                "premium@fxtransfer.com", "Premium@123",
                "Abdul Moeed", UserRole.Premium,
                isPremium: true,
                premiumExpiry: DateTime.UtcNow.AddMonths(6),
                walletBalance: 10000);

            // Seed Regular User
            await CreateUserWithWallets(
                userManager, context, logger,
                "user@fxtransfer.com", "User@123",
                "Talha", UserRole.Regular,
                isPremium: false,
                walletBalance: 5000);

            // Seed Suspended User (for testing)
            await CreateUserWithWallets(
                userManager, context, logger,
                "suspended@fxtransfer.com", "Suspended@123",
                "Suspended Demo User", UserRole.Suspended,
                isPremium: false,
                isSuspended: true,
                walletBalance: 0);

            // Seed additional test user with different currencies
            await CreateUserWithWallets(
                userManager, context, logger,
                "test@fxtransfer.com", "Test@123",
                "Test User", UserRole.Regular,
                isPremium: false,
                walletBalance: 2500);

            await context.SaveChangesAsync();
            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private static async Task CreateUserWithWallets(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        ILogger logger,
        string email,
        string password,
        string fullName,
        UserRole role,
        bool isPremium = false,
        DateTime? premiumExpiry = null,
        bool isSuspended = false,
        decimal walletBalance = 5000)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true,
                RegisteredAt = DateTime.UtcNow,
                ReferralCode = GenerateReferralCode(),
                IsPremium = isPremium,
                PremiumExpiry = premiumExpiry,
                IsSuspended = isSuspended
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role.ToString());
                logger.LogInformation($"Created user: {email} with role: {role}");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                logger.LogError($"Failed to create user {email}: {errors}");
                return;
            }
        }

        // Create wallets for the user if they don't exist
        var defaultCurrencies = new[] { "USD", "EUR", "GBP", "PKR", "AED", "SAR" };

        foreach (var currency in defaultCurrencies)
        {
            if (!context.Wallets.Any(w => w.UserId == user.Id && w.CurrencyCode == currency))
            {
                decimal initialBalance = 0;

                // Set initial balances based on currency and user role
                if (currency == "USD")
                {
                    initialBalance = walletBalance;
                }
                else if (currency == "EUR" && (role == UserRole.Premium || role == UserRole.Admin))
                {
                    initialBalance = 2000;
                }
                else if (currency == "GBP" && (role == UserRole.Premium || role == UserRole.Admin))
                {
                    initialBalance = 1500;
                }
                else if (currency == "PKR")
                {
                    initialBalance = 500000; // 500,000 PKR for testing
                }
                else if (currency == "AED" && role == UserRole.Admin)
                {
                    initialBalance = 10000;
                }
                else if (currency == "SAR" && role == UserRole.Admin)
                {
                    initialBalance = 10000;
                }
                else
                {
                    initialBalance = 0;
                }

                context.Wallets.Add(new Wallet
                {
                    UserId = user.Id,
                    CurrencyCode = currency,
                    Balance = initialBalance,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                });

                if (initialBalance > 0)
                {
                    logger.LogInformation($"Created {currency} wallet for user {email} with balance: {initialBalance}");
                }
            }
        }
    }

    private static string GenerateReferralCode()
    {
        return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
    }
}