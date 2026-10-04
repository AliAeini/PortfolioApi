using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class Profile : BaseEntity
{
    public string FullName { get; private set; } = null!;
    public string Bio { get; private set; } = null!;
    public string? AvatarUrl { get; private set; }
    public string? Email { get; private set; }
    public string? Location { get; private set; }

    private Profile() { }

    public static Profile Create(
        string fullName,
        string bio,
        string? email = null,
        string? avatarUrl = null,
        string? location = null)
    {
        ValidateFields(fullName, bio, email);

        return new Profile
        {
            FullName = fullName.Trim(),
            Bio = bio.Trim(),
            Email = email?.Trim().ToLowerInvariant(),
            AvatarUrl = avatarUrl?.Trim(),
            Location = location?.Trim()
        };
    }

    public void Update(
        string fullName,
        string bio,
        string? email = null,
        string? location = null,
        string? avatarUrl = null)
    {
        ValidateFields(fullName, bio, email);

        FullName = fullName.Trim();
        Bio = bio.Trim();
        Email = email?.Trim().ToLowerInvariant();
        Location = location?.Trim();
        AvatarUrl = avatarUrl?.Trim();
        UpdateTimestamp();
    }

    private static void ValidateFields(string fullName, string bio, string? email)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.", nameof(fullName));

        if (fullName.Length > 200)
            throw new ArgumentException("Full name must not exceed 200 characters.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(bio))
            throw new ArgumentException("Bio is required.", nameof(bio));

        if (bio.Length < 10)
            throw new ArgumentException("Bio must be at least 10 characters.", nameof(bio));

        if (!string.IsNullOrWhiteSpace(email) && !IsValidEmail(email))
            throw new ArgumentException("Email must be a valid email address.", nameof(email));
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch { return false; }
    }

    public void SetAvatarUrl(string avatarUrl)
    {
        AvatarUrl = avatarUrl?.Trim();
        UpdateTimestamp();
    }
}