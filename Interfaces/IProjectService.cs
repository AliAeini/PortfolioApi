using PortfolioApi.DTOs;

namespace PortfolioApi.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetByProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<ProjectDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);   // 👈 این، نه GetByIdWithDetailsAsync
    Task<ProjectDto> CreateAsync(Guid profileId, CreateProjectRequest request, CancellationToken cancellationToken = default);
    Task<ProjectDto> UpdateAsync(Guid profileId, Guid projectId, UpdateProjectRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid profileId, Guid projectId, CancellationToken cancellationToken = default);
}