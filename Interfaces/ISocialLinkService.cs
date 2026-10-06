using PortfolioApi.DTOs;

namespace PortfolioApi.Interfaces;

public interface ISocialLinkService
{
    Task<IEnumerable<SocialLinkDto>> GetByProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<SocialLinkDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SocialLinkDto> CreateAsync(Guid profileId, CreateSocialLinkRequest request, CancellationToken cancellationToken = default);
    Task<SocialLinkDto> UpdateAsync(Guid profileId, Guid socialLinkId, UpdateSocialLinkRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid profileId, Guid socialLinkId, CancellationToken cancellationToken = default);
}