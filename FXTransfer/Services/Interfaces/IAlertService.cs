using FXTransfer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FXTransfer.Services.Interfaces;

/// <summary>
/// SRP: Rate alert management interface
/// ISP: Focused on alert operations
/// </summary>
public interface IAlertService
{
    /// <summary>
    /// Creates a new rate alert
    /// </summary>
    Task<RateAlert> CreateAlertAsync(string userId, string fromCurrency, string toCurrency, decimal targetRate);

    /// <summary>
    /// Gets all alerts for a user
    /// </summary>
    Task<List<RateAlert>> GetUserAlertsAsync(string userId);

    /// <summary>
    /// Deletes an alert
    /// </summary>
    Task DeleteAlertAsync(int alertId);

    /// <summary>
    /// Checks and triggers alerts that hit target rates
    /// </summary>
    Task CheckAndTriggerAlertsAsync();

    /// <summary>
    /// Event triggered when an alert is fired
    /// </summary>
    event EventHandler<AlertTriggeredEventArgs>? AlertTriggered;
}

public class AlertTriggeredEventArgs : EventArgs
{
    public RateAlert Alert { get; set; }
    public decimal CurrentRate { get; set; }

    public AlertTriggeredEventArgs(RateAlert alert, decimal currentRate)
    {
        Alert = alert;
        CurrentRate = currentRate;
    }
}