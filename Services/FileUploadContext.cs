namespace PortfolioApi.Services;

public record FileUploadContext(
    Guid? UserId,
    string Category,
    string? SubFolder = null
);