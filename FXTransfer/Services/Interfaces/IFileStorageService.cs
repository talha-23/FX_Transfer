using System.Threading.Tasks;

namespace FXTransfer.Services.Interfaces;

/// <summary>
/// SRP: File storage operations interface
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Saves a file to the server
    /// </summary>
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string subDirectory = "uploads");

    /// <summary>
    /// Deletes a file from the server
    /// </summary>
    Task<bool> DeleteFileAsync(string filePath);

    /// <summary>
    /// Gets file as byte array
    /// </summary>
    Task<byte[]?> GetFileAsync(string filePath);

    /// <summary>
    /// Checks if file exists
    /// </summary>
    Task<bool> FileExistsAsync(string filePath);
}