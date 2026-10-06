using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class Profile : BaseEntity
{
    public string FullName { get; private set; } = null!;
    public string Bio { get; private set; } = null!;
    public string? ShortBio { get; private set; }

    public Guid? JobCategoryId { get; private set; }
    public JobCategory? JobCategory { get; private set; }
    public string? JobTitle { get; private set; }
    public int? YearsOfExperience { get; private set; }
    public bool AvailableForHire { get; private set; }

    public string? AvatarUrl { get; private set; }
    public string? CoverImageUrl { get; private set; }

    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? Location { get; private set; }
    public string? Website { get; private set; }

    public DateTime? DateOfBirth { get; private set; }
    public string? Nationality { get; private set; }
    public string? Languages { get; private set; }
    public string? Hobbies { get; private set; }

    public ICollection<ProfileSkill> ProfileSkills { get; private set; } = new List<ProfileSkill>();
    public ICollection<SocialLink> SocialLinks { get; private set; } = new List<SocialLink>();
    public ICollection<Education> Educations { get; private set; } = new List<Education>();
    public ICollection<Experience> Experiences { get; private set; } = new List<Experience>();

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
            AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl) ? null : avatarUrl.Trim(),
            Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim()
        };
    }

    public void Update(
        string fullName,
        string bio,
        string? shortBio,
        Guid? jobCategoryId,
        string? jobTitle,
        int? yearsOfExperience,
        bool availableForHire,
        string? email,
        string? phoneNumber,
        string? avatarUrl,
        string? coverImageUrl,
        string? location,
        string? website,
        DateTime? dateOfBirth,
        string? nationality,
        string? languages,
        string? hobbies)
    {
        ValidateFields(fullName, bio, email);

        FullName = fullName.Trim();
        AvatarUrl = avatarUrl;
        CoverImageUrl = coverImageUrl;
        Bio = bio.Trim();
        ShortBio = string.IsNullOrWhiteSpace(shortBio) ? null : shortBio.Trim();
        JobCategoryId = jobCategoryId;
        JobTitle = string.IsNullOrWhiteSpace(jobTitle) ? null : jobTitle.Trim();
        YearsOfExperience = yearsOfExperience;
        AvailableForHire = availableForHire;
        Email = email?.Trim().ToLowerInvariant();
        PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();
        Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim();
        Website = string.IsNullOrWhiteSpace(website) ? null : website.Trim();
        DateOfBirth = dateOfBirth;
        Nationality = string.IsNullOrWhiteSpace(nationality) ? null : nationality.Trim();
        Languages = string.IsNullOrWhiteSpace(languages) ? null : languages.Trim();
        Hobbies = string.IsNullOrWhiteSpace(hobbies) ? null : hobbies.Trim();

        UpdateTimestamp();
    }

    public void SetAvatarUrl(string? avatarUrl)
    {
        AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl) ? null : avatarUrl.Trim();
        UpdateTimestamp();
    }

    public void SetCoverImageUrl(string? coverImageUrl)
    {
        CoverImageUrl = string.IsNullOrWhiteSpace(coverImageUrl) ? null : coverImageUrl.Trim();
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

        if (bio.Length > 2000)
            throw new ArgumentException("Bio must not exceed 2000 characters.", nameof(bio));

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
}