using PortfolioApi.DTOs;

namespace PortfolioApi.Services;

public interface IProfileService
{
    Task<IEnumerable<ProfileSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProfileDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProfileDto> CreateAsync(CreateProfileRequest request, CancellationToken cancellationToken = default);
    Task<ProfileDto> UpdateAsync(Guid id, UpdateProfileRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    // Task<ProfileDto> UpdateAvatarAsync(Guid id, string avatarUrl, CancellationToken cancellationToken = default);
}