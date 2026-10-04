using PortfolioApi.Models;

namespace PortfolioApi.Services;

public interface IUserService
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByCreatedByAsync(Guid createdByUserId, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<User> CreateAsync(string email, string password, string fullName, string role, Guid? createdByUserId = null, CancellationToken cancellationToken = default);
    Task UpdateRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiry, CancellationToken cancellationToken = default);
    Task ClearRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default);
}