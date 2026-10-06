using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Interfaces;
using PortfolioApi.Models;
using PortfolioApi.Repositories;

namespace PortfolioApi.Services;

public class SocialLinkService : ISocialLinkService
{
    private readonly ISocialLinkRepository _repository;
    private readonly IProfileRepository _profileRepository;

    public SocialLinkService(
        ISocialLinkRepository repository,
        IProfileRepository profileRepository)
    {
        _repository = repository;
        _profileRepository = profileRepository;
    }

    public async Task<IEnumerable<SocialLinkDto>> GetByProfileAsync(
        Guid profileId, CancellationToken cancellationToken = default)
    {
        var links = await _repository.GetByProfileAsync(profileId, cancellationToken);
        return links.Select(MapToDto);
    }

    public async Task<SocialLinkDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var link = await _repository.GetByIdAsync(id, cancellationToken);
        if (link is null)
            throw new KeyNotFoundException($"SocialLink with id '{id}' not found");

        return MapToDto(link);
    }

    public async Task<SocialLinkDto> CreateAsync(
        Guid profileId,
        CreateSocialLinkRequest request,
        CancellationToken cancellationToken = default)
    {
        var profile = await _profileRepository.GetByIdAsync(profileId, cancellationToken);
        if (profile is null)
            throw new KeyNotFoundException($"Profile with id '{profileId}' not found");

        if (!Enum.IsDefined(typeof(SocialPlatform), request.Platform))
            throw new ArgumentException("Invalid platform.", nameof(request.Platform));

        var platform = (SocialPlatform)request.Platform;

        var link = SocialLink.Create(
            profileId,
            platform,
            request.Url,
            request.IconUrl,
            request.DisplayOrder);

        await _repository.AddAsync(link, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapToDto(link);
    }

    public async Task<SocialLinkDto> UpdateAsync(
        Guid profileId,
        Guid socialLinkId,
        UpdateSocialLinkRequest request,
        CancellationToken cancellationToken = default)
    {
        var link = await _repository.GetByIdAsync(socialLinkId, cancellationToken);
        if (link is null || link.ProfileId != profileId)
            throw new KeyNotFoundException($"SocialLink with id '{socialLinkId}' not found");

        if (!Enum.IsDefined(typeof(SocialPlatform), request.Platform))
            throw new ArgumentException("Invalid platform.", nameof(request.Platform));

        var platform = (SocialPlatform)request.Platform;

        link.Update(
            platform,
            request.Url,
            request.IconUrl,
            request.DisplayOrder);

        _repository.Update(link);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapToDto(link);
    }

    public async Task DeleteAsync(
        Guid profileId, Guid socialLinkId, CancellationToken cancellationToken = default)
    {
        var link = await _repository.GetByIdAsync(socialLinkId, cancellationToken);
        if (link is null || link.ProfileId != profileId)
            throw new KeyNotFoundException($"SocialLink with id '{socialLinkId}' not found");

        _repository.Remove(link);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private static SocialLinkDto MapToDto(SocialLink s)
        => new(
            s.Id,
            s.ProfileId,
            (int)s.Platform,
            s.Platform.ToString(),
            s.Url,
            s.IconUrl,
            s.DisplayOrder,
            s.CreatedAt,
            s.UpdatedAt);
}