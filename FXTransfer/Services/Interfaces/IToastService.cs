using System;

namespace FXTransfer.Services.Interfaces;

/// <summary>
/// SRP: Toast notification service interface
/// ISP: Focused on notification operations
/// </summary>
public interface IToastService
{
    event EventHandler<ToastMessageEventArgs>? OnShow;

    void ShowSuccess(string message);
    void ShowError(string message);
    void ShowInfo(string message);
    void ShowWarning(string message);
}

public class ToastMessageEventArgs : EventArgs
{
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // success, error, info, warning

    public ToastMessageEventArgs(string message, string type)
    {
        Message = message;
        Type = type;
    }
}