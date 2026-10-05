using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class SocialLink : BaseEntity
{
    public Guid ProfileId { get; private set; } 
    public Profile Profile { get; private set; } = null!; 
    public string Platform { get; private set; } = null!;
    public string Url { get; private set; } = null!;
    public string? IconUrl { get; private set; }
    public int DisplayOrder { get; private set; }

    private SocialLink() { }

    public static SocialLink Create(
        Guid profileId,
        string platform,
        string url,
        string? iconUrl = null,
        int displayOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(platform))
            throw new ArgumentException("Platform is required.", nameof(platform));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL is required.", nameof(url));

        return new SocialLink
        {
            ProfileId = profileId,
            Platform = platform.Trim(),
            Url = url.Trim(),
            IconUrl = string.IsNullOrWhiteSpace(iconUrl) ? null : iconUrl.Trim(),
            DisplayOrder = displayOrder,
            Status = EntityStatus.Active
        };
    }

    public void Update(string platform, string url, string? iconUrl, int displayOrder)
    {
        Platform = platform.Trim();
        Url = url.Trim();
        IconUrl = string.IsNullOrWhiteSpace(iconUrl) ? null : iconUrl.Trim();
        DisplayOrder = displayOrder;
        UpdateTimestamp();
    }
}