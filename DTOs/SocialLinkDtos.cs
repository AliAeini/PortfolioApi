namespace PortfolioApi.DTOs;

public record SocialLinkDto(
    Guid Id,
    Guid ProfileId,
    int Platform,
    string PlatformName,
    string Url,
    string? IconUrl,
    int DisplayOrder,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateSocialLinkRequest(
    int Platform,
    string Url,
    string? IconUrl,
    int DisplayOrder = 0
);

public record UpdateSocialLinkRequest(
    int Platform,
    string Url,
    string? IconUrl,
    int DisplayOrder
);