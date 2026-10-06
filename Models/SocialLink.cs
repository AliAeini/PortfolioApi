using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class SocialLink : BaseEntity
{
    public Guid ProfileId { get; private set; }
    public Profile Profile { get; private set; } = null!;

    public SocialPlatform Platform { get; private set; }
    public string Url { get; private set; } = null!;
    public string? IconUrl { get; private set; }
    public int DisplayOrder { get; private set; }

    private SocialLink() { }

    public static SocialLink Create(
        Guid profileId,
        SocialPlatform platform,
        string url,
        string? iconUrl = null,
        int displayOrder = 0)
    {
        if (profileId == Guid.Empty)
            throw new ArgumentException("ProfileId is required.", nameof(profileId));

        if (!Enum.IsDefined(typeof(SocialPlatform), platform))
            throw new ArgumentException("Invalid platform.", nameof(platform));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL is required.", nameof(url));

        if (url.Length > 500)
            throw new ArgumentException("URL must not exceed 500 characters.", nameof(url));

        return new SocialLink
        {
            ProfileId = profileId,
            Platform = platform,
            Url = url.Trim(),
            IconUrl = string.IsNullOrWhiteSpace(iconUrl) ? null : iconUrl.Trim(),
            DisplayOrder = displayOrder,
            Status = EntityStatus.Active
        };
    }

    public void Update(
        SocialPlatform platform,
        string url,
        string? iconUrl,
        int displayOrder)
    {
        if (!Enum.IsDefined(typeof(SocialPlatform), platform))
            throw new ArgumentException("Invalid platform.", nameof(platform));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL is required.", nameof(url));

        Platform = platform;
        Url = url.Trim();
        IconUrl = string.IsNullOrWhiteSpace(iconUrl) ? null : iconUrl.Trim();
        DisplayOrder = displayOrder;
    }
}