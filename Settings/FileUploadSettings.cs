namespace PortfolioApi.Settings;

public class FileUploadSettings
{
    public const string SectionName = "FileUploadSettings";

    public string BaseUploadPath { get; set; } = "wwwroot/uploads";
    public string UsersFolderName { get; set; } = "users";
    public string AnonymousFolderName { get; set; } = "anonymous";
    public long MaxFileSizeBytes { get; set; } = 5 * 1024 * 1024;
    public int MaxFileCount { get; set; } = 5;
    public string[] AllowedExtensions { get; set; } = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
    public string[] AllowedMimeTypes { get; set; } = ["image/jpeg", "image/png", "image/webp", "image/gif"];
}