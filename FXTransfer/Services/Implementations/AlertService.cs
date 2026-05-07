using FXTransfer.Data;
using FXTransfer.Models.Entities;
using FXTransfer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FXTransfer.Services.Implementations;

public class AlertService : IAlertService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrencyRateService _rateService;
    private readonly ILogger<AlertService> _logger;

    public event EventHandler<AlertTriggeredEventArgs>? AlertTriggered;

    public AlertService(
        ApplicationDbContext context,
        ICurrencyRateService rateService,
        ILogger<AlertService> logger)
    {
        _context = context;
        _rateService = rateService;
        _logger = logger;
    }

    public async Task<RateAlert> CreateAlertAsync(string userId, string fromCurrency, string toCurrency, decimal targetRate, string alertType = "Above")
    {
        var alert = new RateAlert
        {
            UserId = userId,
            FromCurrency = fromCurrency.ToUpper(),
            ToCurrency = toCurrency.ToUpper(),
            TargetRate = targetRate,
            AlertType = alertType,
            CreatedAt = DateTime.UtcNow,
            IsTriggered = false
        };

        _context.RateAlerts.Add(alert);
        await _context.SaveChangesAsync();

        return alert;
    }
    public async Task<List<RateAlert>> GetAllActiveAlertsAsync()
    {
        return await _context.RateAlerts
            .Where(a => !a.IsTriggered)
            .ToListAsync();
    }

    public async Task<List<RateAlert>> GetUserAlertsAsync(string userId)
    {
        return await _context.RateAlerts
            .Where(a => a.UserId == userId && !a.IsTriggered)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task DeleteAlertAsync(int alertId)
    {
        var alert = await _context.RateAlerts.FindAsync(alertId);
        if (alert != null)
        {
            _context.RateAlerts.Remove(alert);
            await _context.SaveChangesAsync();
        }
    }

    public async Task CheckAndTriggerAlertsAsync()
    {
        try
        {
            var activeAlerts = await GetAllActiveAlertsAsync();

            foreach (var alert in activeAlerts)
            {
                var rates = await _rateService.GetRatesAsync(alert.FromCurrency);

                if (rates.ContainsKey(alert.ToCurrency))
                {
                    var currentRate = rates[alert.ToCurrency];
                    bool shouldTrigger = false;

                    if (alert.AlertType == "Above" && currentRate >= alert.TargetRate)
                        shouldTrigger = true;
                    else if (alert.AlertType == "Below" && currentRate <= alert.TargetRate)
                        shouldTrigger = true;

                    if (shouldTrigger)
                    {
                        alert.IsTriggered = true;
                        alert.TriggeredAt = DateTime.UtcNow;

                        // Create notification for user
                        await CreateNotificationAsync(
                            alert.UserId,
                            "🔔 Rate Alert Triggered",
                            $"Exchange rate for {alert.FromCurrency}/{alert.ToCurrency} hit {currentRate:F4} (Target: {alert.TargetRate:F4})",
                            "success"
                        );

                        _logger.LogInformation($"Alert {alert.Id} triggered!");
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking alerts");
        }
    }

    // Add this helper method to create notifications
    private async Task CreateNotificationAsync(string userId, string title, string message, string type)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }
}