using PortfolioApi.DTOs;
using PortfolioApi.Models;
using PortfolioApi.Repositories;


namespace PortfolioApi.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _repository;
    private readonly IUserHandler _userHandler;
    private readonly IFileStorageService _fileStorage;

    public ProfileService(
        IProfileRepository repository,
        IUserHandler userHandler,
        IFileStorageService fileStorage)
    {
        _repository = repository;
        _userHandler = userHandler;
        _fileStorage = fileStorage;
    }

    public async Task<IEnumerable<ProfileSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var profiles = await _repository.GetAllAsync(cancellationToken);
        return profiles.Select(MapToSummaryDto);
    }

    private static ProfileSummaryDto MapToSummaryDto(Profile profile)
        => new(
            profile.Id,
            profile.FullName,
            profile.JobTitle,
            profile.JobCategory?.Name,
            profile.AvatarUrl,
            profile.Location,
            profile.AvailableForHire,
            profile.ProfileSkills?.Count ?? 0);

    public async Task<ProfileDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var profile = await _repository.GetByIdAsync(id, cancellationToken);
        if (profile is null)
            throw new KeyNotFoundException($"Profile with id '{id}' not found");

        return MapToDto(profile);
    }

    public async Task<ProfileDto> CreateAsync(
        CreateProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var exists = await _repository.EmailExistsAsync(request.Email, null, cancellationToken);
            if (exists)
                throw new InvalidOperationException("This email is already registered.");
        }

        var profile = Profile.Create(
            request.FullName,
            request.Bio,
            request.Email,
            avatarUrl: null,
            request.Location);

        await _repository.AddAsync(profile, cancellationToken);

        await _userHandler.HandleProfileOwnerAsync(
            profileId: profile.Id,
            email: request.Email ?? string.Empty,
            password: request.OwnerPassword ?? string.Empty,
            fullName: request.FullName,
            cancellationToken: cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        return MapToDto(profile);
    }

    public async Task<ProfileDto> UpdateAsync(
        Guid id,
        UpdateProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var profile = await _repository.GetByIdAsync(id, cancellationToken);
        if (profile is null)
            throw new KeyNotFoundException($"Profile with id '{id}' not found");

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var exists = await _repository.EmailExistsAsync(request.Email, id, cancellationToken);
            if (exists)
                throw new InvalidOperationException("This email is already registered.");
        }

        profile.Update(
            request.FullName,
            request.Bio,
            request.ShortBio,
            request.JobCategoryId,
            request.JobTitle,
            request.YearsOfExperience,
            request.AvailableForHire,
            request.Email,
            request.PhoneNumber,
            request.Location,
            request.Website,
            request.DateOfBirth,
            request.Nationality,
            request.Languages,
            request.Hobbies);

        await _repository.SaveChangesAsync(cancellationToken);
        return MapToDto(profile);
    }

    public async Task<ProfileDto> UpdateAvatarAsync(
        Guid id,
        string avatarUrl,
        CancellationToken cancellationToken = default)
    {
        var profile = await _repository.GetByIdAsync(id, cancellationToken);
        if (profile is null)
            throw new KeyNotFoundException($"Profile with id '{id}' not found");

        if (string.IsNullOrWhiteSpace(avatarUrl))
            throw new ArgumentException("Avatar URL is required.", nameof(avatarUrl));

        if (!string.IsNullOrWhiteSpace(profile.AvatarUrl) && profile.AvatarUrl != avatarUrl)
        {
            _fileStorage.DeleteFile(profile.AvatarUrl);
        }

        profile.SetAvatarUrl(avatarUrl);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapToDto(profile);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var profile = await _repository.GetByIdAsync(id, cancellationToken);
        if (profile is null)
            throw new KeyNotFoundException($"Profile with id '{id}' not found");

        if (!string.IsNullOrWhiteSpace(profile.AvatarUrl))
        {
            _fileStorage.DeleteFile(profile.AvatarUrl);
        }

        _repository.Remove(profile);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private static ProfileDto MapToDto(Profile profile)
        => new(
            profile.Id,
            profile.FullName,
            profile.Bio,
            profile.ShortBio,
            profile.JobCategoryId,
            profile.JobCategory?.Name,
            profile.JobTitle,
            profile.YearsOfExperience,
            profile.AvailableForHire,
            profile.AvatarUrl,
            profile.CoverImageUrl,
            profile.Email,
            profile.PhoneNumber,
            profile.Location,
            profile.Website,
            profile.DateOfBirth,
            profile.Nationality,
            profile.Languages,
            profile.Hobbies,
            profile.CreatedAt,
            profile.UpdatedAt
        );
}