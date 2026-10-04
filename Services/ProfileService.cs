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

    public async Task<IEnumerable<ProfileDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var profiles = await _repository.GetAllAsync(cancellationToken);
        return profiles.Select(MapToDto);
    }

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
            request.AvatarUrl,
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
            request.Email,
            request.Location
            );

        // _repository.Update(profile);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapToDto(profile);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var profile = await _repository.GetByIdAsync(id, cancellationToken);
        if (profile is null)
            throw new KeyNotFoundException($"Profile with id '{id}' not found");

        _repository.Remove(profile);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private static ProfileDto MapToDto(Profile profile)
        => new(
            profile.Id,
            profile.FullName,
            profile.Bio,
            profile.AvatarUrl,
            profile.Email,
            profile.Location,
            profile.CreatedAt,
            profile.UpdatedAt
        );

    public async Task<ProfileDto> UpdateAvatarAsync(
        Guid id,
        string avatarUrl,
        CancellationToken cancellationToken = default)
    {
        var profile = await _repository.GetByIdAsync(id, cancellationToken);
        if (profile is null)
            throw new KeyNotFoundException($"Profile with id '{id}' not found");
        if (!string.IsNullOrWhiteSpace(profile.AvatarUrl) && profile.AvatarUrl != avatarUrl)
        {
            _fileStorage.DeleteFile(profile.AvatarUrl);
        }
        if (string.IsNullOrWhiteSpace(avatarUrl))
            throw new ArgumentException("Avatar URL is required.", nameof(avatarUrl));

        profile.SetAvatarUrl(avatarUrl);
        _repository.Update(profile);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapToDto(profile);
    }
}