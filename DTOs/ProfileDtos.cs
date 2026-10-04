namespace PortfolioApi.DTOs;

public record ProfileDto(
    Guid Id,
    string FullName,
    string Bio,
    string? AvatarUrl,
    string? Email,
    string? Location,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateProfileRequest(
    string FullName,
    string Bio,
    string? AvatarUrl,
    string? Email,
    string? Location,
    string? OwnerPassword
);

public record UpdateProfileRequest(
    string FullName,
    string Bio,
    string? AvatarUrl,
    string? Email,
    string? Location
);

public record UpdateAvatarRequest(string AvatarUrl);