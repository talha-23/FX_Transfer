namespace FXTransfer.Models.Enums;

/// <summary>
/// Status of a transfer transaction
/// </summary>
public enum TransferStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Flagged = 4,
    Cancelled = 5
}