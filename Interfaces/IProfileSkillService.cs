using PortfolioApi.DTOs;

namespace PortfolioApi.Interfaces;

public interface IProfileSkillService
{
    Task<IEnumerable<ProfileSkillDto>> GetByProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<ProfileSkillDto> AddAsync(Guid profileId, AddProfileSkillRequest request, CancellationToken cancellationToken = default);
    Task<ProfileSkillDto> UpdateAsync(Guid profileId, Guid profileSkillId, UpdateProfileSkillRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid profileId, Guid profileSkillId, CancellationToken cancellationToken = default);
}