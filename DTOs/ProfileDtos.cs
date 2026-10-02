namespace PortfolioApi.DTOs;

public record ProfileDto(
    Guid Id,
    string FullName,
    string Bio,
    string? AvatarUrl,
    string? Email,
    string? Location
);

public record CreateProfileRequest(
    string FullName,
    string Bio,
    string? AvatarUrl,
    string? Email,
    string? Location
);

public record UpdateProfileRequest(
    string FullName,
    string Bio,
    string? AvatarUrl,
    string? Email,
    string? Location
);