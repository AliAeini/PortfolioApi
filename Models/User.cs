using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class User : BaseEntity
{
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string? FullName { get; private set; }
    public string Role { get; private set; } = "Admin";

    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiryTime { get; private set; }

    private User() { }

    public static User Create(string email, string passwordHash, string? fullName = null, string role = "Admin")
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        return new User
        {
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            FullName = fullName?.Trim(),
            Role = role
        };
    }

    public void SetRefreshToken(string refreshToken, DateTime expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
        UpdateTimestamp();
    }

    public void ClearRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiryTime = null;
        UpdateTimestamp();
    }
}