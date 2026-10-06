using PortfolioApi.DTOs;

namespace PortfolioApi.Interfaces;

public interface IExperienceService
{
    Task<IEnumerable<ExperienceDto>> GetByProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<ExperienceDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ExperienceDto> CreateAsync(Guid profileId, CreateExperienceRequest request, CancellationToken cancellationToken = default);
    Task<ExperienceDto> UpdateAsync(Guid profileId, Guid experienceId, UpdateExperienceRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid profileId, Guid experienceId, CancellationToken cancellationToken = default);
}