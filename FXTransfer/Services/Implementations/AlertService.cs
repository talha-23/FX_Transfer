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

    public async Task<RateAlert> CreateAlertAsync(string userId, string fromCurrency, string toCurrency, decimal targetRate)
    {
        var alert = new RateAlert
        {
            UserId = userId,
            FromCurrency = fromCurrency.ToUpper(),
            ToCurrency = toCurrency.ToUpper(),
            TargetRate = targetRate,
            CreatedAt = DateTime.UtcNow,
            IsTriggered = false
        };

        _context.RateAlerts.Add(alert);
        await _context.SaveChangesAsync();
        return alert;
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
        // Implementation for background service
        await Task.CompletedTask;
    }
}