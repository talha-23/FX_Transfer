using FXTransfer.Models.Entities;
using FXTransfer.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FXTransfer.Services.Interfaces;

/// <summary>
/// SRP: Transfer operations interface
/// ISP: Focused on transfer-related operations
/// </summary>
public interface ITransferService
{
    // Events
    event EventHandler<TransferEventArgs>? TransferCompleted;
    event EventHandler<TransferEventArgs>? TransferFailed;
    event EventHandler<LowBalanceEventArgs>? LowBalanceEvent;

    /// <summary>
    /// Executes a currency transfer
    /// </summary>
    Task<Transfer> ExecuteTransferAsync(TransferRequest request);

    /// <summary>
    /// Gets transfer history for a user
    /// </summary>
    Task<List<Transfer>> GetUserTransfersAsync(string userId, int page = 1, int pageSize = 10);

    /// <summary>
    /// Gets a specific transfer by ID
    /// </summary>
    Task<Transfer?> GetTransferByIdAsync(int transferId);

    /// <summary>
    /// Gets transfer by reference
    /// </summary>
    Task<Transfer?> GetTransferByReferenceAsync(string reference);
}

/// <summary>
/// Event arguments for transfer events
/// </summary>
public class TransferEventArgs : EventArgs
{
    public Transfer Transfer { get; }
    public DateTime Timestamp { get; }

    public TransferEventArgs(Transfer transfer)
    {
        Transfer = transfer;
        Timestamp = DateTime.UtcNow;
    }
}

/// <summary>
/// Event arguments for low balance events
/// </summary>
public class LowBalanceEventArgs : EventArgs
{
    public string UserId { get; }
    public string Currency { get; }
    public decimal CurrentBalance { get; }
    public decimal RequiredAmount { get; }

    public LowBalanceEventArgs(string userId, string currency, decimal currentBalance, decimal requiredAmount)
    {
        UserId = userId;
        Currency = currency;
        CurrentBalance = currentBalance;
        RequiredAmount = requiredAmount;
    }
}