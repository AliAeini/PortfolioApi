using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;

namespace PortfolioApi.Repositories;

public class ProfileRepository : Repository<Profile>, IProfileRepository
{
    public ProfileRepository(AppDbContext db) : base(db) { }

    public async Task<bool> EmailExistsAsync(
        string email,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var normalized = email.Trim().ToLowerInvariant();

        var query = _dbSet.Where(p => p.Email != null && p.Email.ToLower() == normalized);

        if (excludeId.HasValue)
            query = query.Where(p => p.Id != excludeId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<Profile?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var normalized = email.Trim().ToLowerInvariant();
        return await _dbSet.FirstOrDefaultAsync(p => p.Email != null && p.Email.ToLower() == normalized, cancellationToken);
    }
}