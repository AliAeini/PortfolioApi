namespace PortfolioApi.Services;

public interface IUserHandler
{
    Task<Guid?> HandleProfileOwnerAsync(
        Guid profileId,
        string email,
        string password,
        string fullName,
        CancellationToken cancellationToken = default);
}