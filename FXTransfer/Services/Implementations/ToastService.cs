using System;
using FXTransfer.Services.Interfaces;

namespace FXTransfer.Services.Implementations;

/// <summary>
/// SRP: Manages toast notifications
/// Event-driven: Publishes toast events
/// </summary>
public class ToastService : IToastService
{
    public event EventHandler<ToastMessageEventArgs>? OnShow;

    public void ShowSuccess(string message)
    {
        OnShow?.Invoke(this, new ToastMessageEventArgs(message, "success"));
    }

    public void ShowError(string message)
    {
        OnShow?.Invoke(this, new ToastMessageEventArgs(message, "error"));
    }

    public void ShowInfo(string message)
    {
        OnShow?.Invoke(this, new ToastMessageEventArgs(message, "info"));
    }

    public void ShowWarning(string message)
    {
        OnShow?.Invoke(this, new ToastMessageEventArgs(message, "warning"));
    }
}