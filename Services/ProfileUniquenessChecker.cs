using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;

namespace PortfolioApi.Services;

public class ProfileUniquenessChecker : IProfileUniquenessChecker
{
    private readonly AppDbContext _db;

    public ProfileUniquenessChecker(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeProfileId = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            return true;

        var query = _db.Profiles.Where(p => p.Email == email);

        if (excludeProfileId.HasValue)
            query = query.Where(p => p.Id != excludeProfileId.Value);

        return !await query.AnyAsync();
    }
}