using System;

namespace FXTransfer.Services;

/// <summary>
/// SRP: Custom exception for insufficient balance scenarios
/// </summary>
public class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException() : base() { }
    public InsufficientBalanceException(string message) : base(message) { }
    public InsufficientBalanceException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>
/// SRP: Custom exception for API failure scenarios
/// </summary>
public class ApiFailureException : Exception
{
    public ApiFailureException() : base() { }
    public ApiFailureException(string message) : base(message) { }
    public ApiFailureException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>
/// SRP: Custom exception for invalid transfer requests
/// </summary>
public class InvalidTransferException : Exception
{
    public InvalidTransferException() : base() { }
    public InvalidTransferException(string message) : base(message) { }
    public InvalidTransferException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>
/// SRP: Custom exception for rate limit exceeded
/// </summary>
public class RateLimitExceededException : Exception
{
    public RateLimitExceededException() : base() { }
    public RateLimitExceededException(string message) : base(message) { }
    public RateLimitExceededException(string message, Exception inner) : base(message, inner) { }
}