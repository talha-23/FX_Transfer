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

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<NotificationService> _logger;

    public event EventHandler<NotificationEventArgs>? NewNotification;

    public NotificationService(ApplicationDbContext context, ILogger<NotificationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Notification> CreateNotificationAsync(string userId, string title, string message, string type = "info", string? relatedEntityId = null, string? relatedEntityType = null)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            RelatedEntityId = relatedEntityId,
            RelatedEntityType = relatedEntityType
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        // Trigger event for real-time notification
        NewNotification?.Invoke(this, new NotificationEventArgs(notification));

        _logger.LogInformation($"Notification created for user {userId}: {title}");
        return notification;
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(string userId, int limit = 20)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .CountAsync();
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null && !notification.IsRead)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAllAsReadAsync(string userId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNotificationAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SendTransferNotificationAsync(string userId, string fromCurrency, decimal amount, string recipientName, bool isSuccess)
    {
        if (isSuccess)
        {
            await CreateNotificationAsync(
                userId,
                "✅ Transfer Successful",
                $"Successfully sent {amount:F2} {fromCurrency} to {recipientName}",
                "success"
            );
        }
        else
        {
            await CreateNotificationAsync(
                userId,
                "❌ Transfer Failed",
                $"Transfer of {amount:F2} {fromCurrency} to {recipientName} failed",
                "error"
            );
        }
    }

    public async Task SendRateAlertNotificationAsync(string userId, string fromCurrency, string toCurrency, decimal targetRate, decimal currentRate)
    {
        await CreateNotificationAsync(
            userId,
            "🔔 Rate Alert Triggered",
            $"Exchange rate for {fromCurrency}/{toCurrency} hit {currentRate:F4} (Target: {targetRate:F4})",
            "info"
        );
    }

    public async Task SendLowBalanceNotificationAsync(string userId, string currency, decimal balance, decimal threshold)
    {
        await CreateNotificationAsync(
            userId,
            "⚠️ Low Balance Alert",
            $"Your {currency} wallet balance is {balance:F2}. Consider adding funds.",
            "warning"
        );
    }
}