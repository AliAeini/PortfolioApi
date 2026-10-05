namespace PortfolioApi.DTOs;

public record LoginRequest(
    string Email,
    string Password
);

public record RegisterRequest(
    string Email,
    string Password,
    string? FullName
);

public record UserInfoDto(
    Guid Id,
    string Email,
    string? FullName,
    string Role
);

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAt,
    UserInfoDto User
);

public record RefreshRequest(
    string RefreshToken
);