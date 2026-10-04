using PortfolioApi.Services;

namespace PortfolioApi.Services;

public interface IFileStorageService
{
    Task<List<string>> SaveFilesAsync(
        IFormFileCollection files,
        FileUploadContext context,
        CancellationToken cancellationToken = default);

    Task<string> SaveFileAsync(
        IFormFile file,
        FileUploadContext context,
        CancellationToken cancellationToken = default);

    void DeleteFile(string filePath);
    void DeleteUserFolder(Guid userId);
}