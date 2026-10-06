using PortfolioApi.Models;
using PortfolioApi.Repositories;

namespace PortfolioApi.Interfaces;

public interface IProfileSkillRepository : IRepository<ProfileSkill>
{
    Task<IEnumerable<ProfileSkill>> GetByProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<ProfileSkill?> GetByProfileAndSkillAsync(Guid profileId, Guid skillId, CancellationToken cancellationToken = default);
    Task<ProfileSkill?> GetByIdWithSkillAsync(Guid id, CancellationToken cancellationToken = default);   // 👈 جدید
    Task<bool> ExistsAsync(Guid profileId, Guid skillId, CancellationToken cancellationToken = default);
}