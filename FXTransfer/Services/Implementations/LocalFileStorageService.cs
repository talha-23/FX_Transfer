using System;
using System.IO;
using System.Threading.Tasks;
using FXTransfer.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FXTransfer.Services.Implementations;

/// <summary>
/// SRP: Handles local file storage operations
/// </summary>
public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<LocalFileStorageService> _logger;

    public LocalFileStorageService(IWebHostEnvironment environment, ILogger<LocalFileStorageService> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string subDirectory = "uploads")
    {
        try
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, subDirectory);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(stream);
            }

            _logger.LogInformation($"File saved: {filePath}");

            return $"/{subDirectory}/{uniqueFileName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to save file: {fileName}");
            throw;
        }
    }

    public Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation($"File deleted: {filePath}");
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to delete file: {filePath}");
            return Task.FromResult(false);
        }
    }

    public async Task<byte[]?> GetFileAsync(string filePath)
    {
        try
        {
            var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                return await File.ReadAllBytesAsync(fullPath);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to get file: {filePath}");
            return null;
        }
    }

    public Task<bool> FileExistsAsync(string filePath)
    {
        var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
        return Task.FromResult(File.Exists(fullPath));
    }
}