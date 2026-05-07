using FXTransfer.Data;
using FXTransfer.Models.Entities;
using FXTransfer.Services;
using FXTransfer.Services.Implementations;
using FXTransfer.Services.Interfaces;
using FXTransfer.Seeders;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);


// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Authentication State Provider
builder.Services.AddScoped<FxAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<FxAuthenticationStateProvider>());

// Services
builder.Services.AddHttpClient<ICurrencyRateService, ExchangeRateApiService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<StandardFeeCalculator>();
builder.Services.AddScoped<PremiumFeeCalculator>();
builder.Services.AddScoped<IFeeCalculator>(sp =>
{
    var context = sp.GetRequiredService<ApplicationDbContext>();
    return new StandardFeeCalculator(context);
});


builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IToastService, ToastService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IReferralService, ReferralService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddHostedService<AlertBackgroundService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddMemoryCache(); // Add memory cache



// Authorization
builder.Services.AddAuthorization();

// Cookie configuration
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/unauthorized";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var feeConfig = await context.FeeConfigurations.FirstOrDefaultAsync();
    
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// FORCE ROOT PATH TO ALWAYS GO TO HOME
app.Use(async (context, next) =>
{
    // If the request is for root or empty, always go to home
    if (context.Request.Path == "/" || string.IsNullOrEmpty(context.Request.Path))
    {
        // Already at home, continue
        await next();
    }
    else
    {
        await next();
    }
});

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await AdminSeeder.SeedAsync(services);
        Console.WriteLine("✅ Database seeded!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Seed error: {ex.Message}");
    }
}

app.Run();