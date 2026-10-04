using Microsoft.Extensions.Options;
using PortfolioApi.Settings;

namespace PortfolioApi.Services;

public class FileStorageService : IFileStorageService
{
    private readonly FileUploadSettings _settings;
    private readonly ILogger<FileStorageService> _logger;

    public FileStorageService(IOptions<FileUploadSettings> settings, ILogger<FileStorageService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<List<string>> SaveFilesAsync(
        IFormFileCollection files,
        FileUploadContext context,
        CancellationToken cancellationToken = default)
    {
        if (files.Count > _settings.MaxFileCount)
            throw new InvalidOperationException(
                $"Maximum {_settings.MaxFileCount} file(s) allowed per request.");

        var savedPaths = new List<string>();
        var uploadDir = GetUploadDirectory(context);
        Directory.CreateDirectory(uploadDir);

        foreach (var file in files)
        {
            var path = await SaveSingleFileAsync(file, uploadDir, cancellationToken);
            savedPaths.Add(path);
        }

        return savedPaths;
    }

    public async Task<string> SaveFileAsync(
        IFormFile file,
        FileUploadContext context,
        CancellationToken cancellationToken = default)
    {
        var uploadDir = GetUploadDirectory(context);
        Directory.CreateDirectory(uploadDir);

        return await SaveSingleFileAsync(file, uploadDir, cancellationToken);
    }

    private async Task<string> SaveSingleFileAsync(
        IFormFile file,
        string uploadDir,
        CancellationToken cancellationToken)
    {
        if (file.Length > _settings.MaxFileSizeBytes)
            throw new InvalidOperationException(
                $"File '{file.FileName}' exceeds the maximum allowed size of {_settings.MaxFileSizeBytes / 1024 / 1024} MB.");

        if (file.Length == 0)
            throw new InvalidOperationException(
                $"File '{file.FileName}' is empty.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_settings.AllowedExtensions.Contains(extension))
            throw new InvalidOperationException(
                $"File extension '{extension}' is not allowed. Allowed: {string.Join(", ", _settings.AllowedExtensions)}.");

        if (!_settings.AllowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            throw new InvalidOperationException(
                $"Content type '{file.ContentType}' is not allowed.");

        if (!await IsFileSignatureValidAsync(file, file.ContentType))
            throw new InvalidOperationException(
                $"File '{file.FileName}' content does not match its declared type.");

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadDir, uniqueFileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        _logger.LogInformation("File saved: {Path}", filePath);

        return GetRelativeUrlPath(filePath);
    }

    private string GetUploadDirectory(FileUploadContext context)
    {
        var baseDir = Path.Combine(Directory.GetCurrentDirectory(), _settings.BaseUploadPath);

        var userFolder = context.UserId.HasValue
            ? context.UserId.Value.ToString()
            : _settings.AnonymousFolderName;

        var pathParts = new List<string>
        {
            baseDir,
            _settings.UsersFolderName,
            userFolder,
            context.Category
        };

        if (!string.IsNullOrWhiteSpace(context.SubFolder))
            pathParts.Add(context.SubFolder);

        return Path.Combine(pathParts.ToArray());
    }

    private string GetRelativeUrlPath(string absolutePath)
    {
        var normalized = absolutePath.Replace('\\', '/');
        var index = normalized.IndexOf("wwwroot/", StringComparison.OrdinalIgnoreCase);

        if (index >= 0)
            return "/" + normalized[(index + "wwwroot/".Length)..];

        return absolutePath;
    }

    private static async Task<bool> IsFileSignatureValidAsync(IFormFile file, string declaredMimeType)
    {
        var signatures = new Dictionary<string, byte[]>
        {
            ["image/jpeg"] = [0xFF, 0xD8, 0xFF],
            ["image/png"] = [0x89, 0x50, 0x4E, 0x47],
            ["image/webp"] = [0x52, 0x49, 0x46, 0x46],
            ["image/gif"] = [0x47, 0x49, 0x46]
        };

        if (!signatures.TryGetValue(declaredMimeType, out var expectedSignature))
            return true;

        using var stream = file.OpenReadStream();
        var header = new byte[expectedSignature.Length];
        var bytesRead = await stream.ReadAsync(header);

        if (bytesRead < expectedSignature.Length)
            return false;

        stream.Position = 0;
        return header.SequenceEqual(expectedSignature);
    }

    public void DeleteFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return;

        var relativePath = filePath.TrimStart('/');
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            _logger.LogInformation("File deleted: {Path}", fullPath);
        }
    }

    public void DeleteUserFolder(Guid userId)
    {
        var userFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            _settings.BaseUploadPath,
            _settings.UsersFolderName,
            userId.ToString());

        if (Directory.Exists(userFolder))
        {
            Directory.Delete(userFolder, recursive: true);
            _logger.LogInformation("User folder deleted: {Path}", userFolder);
        }
    }
}