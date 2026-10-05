namespace PortfolioApi.DTOs;

public record ProfileDto(
    Guid Id,
    string FullName,
    string Bio,
    string? ShortBio,
    Guid? JobCategoryId,
    string? JobCategoryName,
    string? JobTitle,
    int? YearsOfExperience,
    bool AvailableForHire,
    string? AvatarUrl,
    string? CoverImageUrl,
    string? Email,
    string? PhoneNumber,
    string? Location,
    string? Website,
    DateTime? DateOfBirth,
    string? Nationality,
    string? Languages,
    string? Hobbies,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record ProfileSummaryDto(
    Guid Id,
    string FullName,
    string? JobTitle,
    string? JobCategoryName,
    string? AvatarUrl,
    string? Location,
    bool AvailableForHire,
    int SkillCount
);

public record CreateProfileRequest(
    string FullName,
    string Bio,
    string? Email,
    string? Location,
    string? OwnerPassword
);

public record UpdateProfileRequest(
    string FullName,
    string Bio,
    string? ShortBio,
    Guid? JobCategoryId,
    string? JobTitle,
    int? YearsOfExperience,
    bool AvailableForHire,
    string? AvatarUrl,          
    string? CoverImageUrl,     
    string? Email,
    string? PhoneNumber,
    string? Location,
    string? Website,
    DateTime? DateOfBirth,
    string? Nationality,
    string? Languages,
    string? Hobbies
);