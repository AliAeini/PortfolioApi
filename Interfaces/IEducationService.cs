using PortfolioApi.DTOs;

namespace PortfolioApi.Interfaces;

public interface IEducationService
{
    Task<IEnumerable<EducationDto>> GetByProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<EducationDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<EducationDto> CreateAsync(Guid profileId, CreateEducationRequest request, CancellationToken cancellationToken = default);
    Task<EducationDto> UpdateAsync(Guid profileId, Guid educationId, UpdateEducationRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid profileId, Guid educationId, CancellationToken cancellationToken = default);
}