using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class SocialLink : BaseEntity
{
    public string Platform { get; private set; } = null!;
    public string Url { get; private set; } = null!;
    public string? IconUrl { get; private set; }
    public int DisplayOrder { get; private set; }

    private SocialLink() { }

    public static SocialLink Create(string platform, string url, string? iconUrl = null, int displayOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(platform))
            throw new ArgumentException("Platform is required.", nameof(platform));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL is required.", nameof(url));

        return new SocialLink
        {
            Platform = platform.Trim(),
            Url = url.Trim(),
            IconUrl = iconUrl?.Trim(),
            DisplayOrder = displayOrder
        };
    }

    public void Update(string platform, string url, string? iconUrl, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(platform))
            throw new ArgumentException("Platform is required.", nameof(platform));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL is required.", nameof(url));

        Platform = platform.Trim();
        Url = url.Trim();
        IconUrl = iconUrl?.Trim();
        DisplayOrder = displayOrder;
        UpdateTimestamp();
    }
}