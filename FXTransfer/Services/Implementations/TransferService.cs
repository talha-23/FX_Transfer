using FXTransfer.Data;
using FXTransfer.Models.DTOs;
using FXTransfer.Models.Entities;
using FXTransfer.Models.Enums;
using FXTransfer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FXTransfer.Services.Implementations;

/// <summary>
/// SRP: Handles transfer execution and management
/// DIP: Depends on abstractions (ICurrencyRateService, IFeeCalculator, IWalletService)
/// Event-driven: Publishes transfer events
/// </summary>
public class TransferService : ITransferService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrencyRateService _rateService;
    private readonly IFeeCalculator _feeCalculator;
    private readonly IWalletService _walletService;
    private readonly ILogger<TransferService> _logger;

    // Event declarations
    public event EventHandler<TransferEventArgs>? TransferCompleted;
    public event EventHandler<TransferEventArgs>? TransferFailed;
    public event EventHandler<LowBalanceEventArgs>? LowBalanceEvent;

    public TransferService(
        ApplicationDbContext context,
        ICurrencyRateService rateService,
        IFeeCalculator feeCalculator,
        IWalletService walletService,
        ILogger<TransferService> logger)
    {
        _context = context;
        _rateService = rateService;
        _feeCalculator = feeCalculator;
        _walletService = walletService;
        _logger = logger;
    }

    /// <summary>
    /// Executes a transfer with validation and event firing
    /// </summary>
    public async Task<Transfer> ExecuteTransferAsync(TransferRequest request)
    {
        try
        {
            // Validate request
            if (request.Amount <= 0)
                throw new ArgumentException("Amount must be positive");

            if (string.IsNullOrEmpty(request.FromCurrency) || string.IsNullOrEmpty(request.ToCurrency))
                throw new ArgumentException("Currency codes are required");

            // Get exchange rate
            var exchangeRate = await _rateService.GetExchangeRateAsync(request.FromCurrency, request.ToCurrency);

            // Calculate converted amount
            var convertedAmount = request.Amount * exchangeRate;

            // Calculate fee
            var fee = await _feeCalculator.CalculateFeeAsync(request.Amount, request.FromCurrency, request.ToCurrency, request.User);

            var totalAmount = request.Amount + fee;

            // Check balance
            var balance = await _walletService.GetBalanceAsync(request.UserId, request.FromCurrency);

            if (balance < totalAmount)
            {
                // Fire low balance event
                OnLowBalanceEvent(new LowBalanceEventArgs(request.UserId, request.FromCurrency, balance, totalAmount));
                throw new InsufficientBalanceException($"Insufficient balance. Required: {totalAmount}, Available: {balance}");
            }

            // Create transfer record
            var transfer = new Transfer
            {
                UserId = request.UserId,
                FromCurrency = request.FromCurrency.ToUpper(),
                ToCurrency = request.ToCurrency.ToUpper(),
                Amount = request.Amount,
                ConvertedAmount = convertedAmount,
                ExchangeRate = exchangeRate,
                Fee = fee,
                TotalAmount = totalAmount,
                Status = TransferStatus.Pending.ToString(),
                RecipientEmail = request.RecipientEmail,
                RecipientName = request.RecipientName,
                CreatedAt = DateTime.UtcNow,
                RequiresApproval = request.Amount > 5000,
                IpAddress = request.IpAddress,
                Geolocation = request.Geolocation
            };

            // Deduct from wallet
            await _walletService.DeductBalanceAsync(request.UserId, request.FromCurrency, totalAmount);

            // Save transfer
            _context.Transfers.Add(transfer);
            await _context.SaveChangesAsync();

            // Mark as completed
            transfer.Status = TransferStatus.Completed.ToString();
            transfer.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Fire completion event
            OnTransferCompleted(transfer);

            _logger.LogInformation($"Transfer {transfer.Id} completed for user {request.UserId}");

            return transfer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Transfer failed for user {request.UserId}");
            OnTransferFailed(new TransferEventArgs(new Transfer { UserId = request.UserId }));
            throw;
        }
    }

    /// <summary>
    /// Gets user transfer history with pagination
    /// </summary>
    public async Task<List<Transfer>> GetUserTransfersAsync(string userId, int page = 1, int pageSize = 10)
    {
        try
        {
            return await _context.Transfers
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to get transfers for user {userId}");
            return new List<Transfer>();
        }
    }

    /// <summary>
    /// Gets transfer by ID
    /// </summary>
    public async Task<Transfer?> GetTransferByIdAsync(int transferId)
    {
        try
        {
            return await _context.Transfers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == transferId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to get transfer {transferId}");
            return null;
        }
    }

    /// <summary>
    /// Gets transfer by reference
    /// </summary>
    public async Task<Transfer?> GetTransferByReferenceAsync(string reference)
    {
        try
        {
            return await _context.Transfers
                .FirstOrDefaultAsync(t => t.ReceiptPath != null && t.ReceiptPath.Contains(reference));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to get transfer by reference {reference}");
            return null;
        }
    }

    // Event trigger methods
    protected virtual void OnTransferCompleted(Transfer transfer)
    {
        TransferCompleted?.Invoke(this, new TransferEventArgs(transfer));
    }

    protected virtual void OnTransferFailed(TransferEventArgs args)
    {
        TransferFailed?.Invoke(this, args);
    }

    protected virtual void OnLowBalanceEvent(LowBalanceEventArgs args)
    {
        LowBalanceEvent?.Invoke(this, args);
    }
}