namespace PortfolioApi.Services;

public interface IProfileUniquenessChecker
{
    Task<bool> IsEmailUniqueAsync(string email, Guid? excludeProfileId = null);
}