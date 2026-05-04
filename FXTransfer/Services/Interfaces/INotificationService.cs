using FXTransfer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FXTransfer.Services.Interfaces;

public interface INotificationService
{
    Task<Notification> CreateNotificationAsync(string userId, string title, string message, string type = "info", string? relatedEntityId = null, string? relatedEntityType = null);
    Task<List<Notification>> GetUserNotificationsAsync(string userId, int limit = 20);
    Task<int> GetUnreadCountAsync(string userId);
    Task MarkAsReadAsync(int notificationId);
    Task MarkAllAsReadAsync(string userId);
    Task DeleteNotificationAsync(int notificationId);
    Task SendTransferNotificationAsync(string userId, string fromCurrency, decimal amount, string recipientName, bool isSuccess);
    Task SendRateAlertNotificationAsync(string userId, string fromCurrency, string toCurrency, decimal targetRate, decimal currentRate);
    Task SendLowBalanceNotificationAsync(string userId, string currency, decimal balance, decimal threshold);
    event EventHandler<NotificationEventArgs>? NewNotification;
}

public class NotificationEventArgs : EventArgs
{
    public Notification Notification { get; set; }

    public NotificationEventArgs(Notification notification)
    {
        Notification = notification;
    }
}