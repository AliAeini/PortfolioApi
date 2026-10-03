using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;

namespace PortfolioApi.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;
    private readonly ILogger<UserService> _logger;

    public UserService(AppDbContext db, ILogger<UserService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _db.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email)) return null;
        var normalized = email.Trim().ToLowerInvariant();
        return await _db.Users.FirstOrDefaultAsync(u => u.Email == normalized, cancellationToken);
    }

    public async Task<User?> GetByCreatedByAsync(Guid createdByUserId, CancellationToken cancellationToken = default)
        => await _db.Users.FirstOrDefaultAsync(u => u.CreatedByUserId == createdByUserId, cancellationToken);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        var normalized = email.Trim().ToLowerInvariant();
        return await _db.Users.AnyAsync(u => u.Email == normalized, cancellationToken);
    }

    public async Task<User> CreateAsync(
        string email,
        string password,
        string fullName,
        string role,
        Guid? createdByUserId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            throw new ArgumentException("Password must be at least 8 characters.", nameof(password));

        var normalizedEmail = email.Trim().ToLowerInvariant();

        if (await EmailExistsAsync(normalizedEmail, cancellationToken))
            throw new InvalidOperationException("This email is already registered.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        var user = User.Create(normalizedEmail, passwordHash, fullName.Trim(), role);

        if (createdByUserId.HasValue)
            user.SetCreatedBy(createdByUserId.Value);

        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User created: {Email} with role {Role}", normalizedEmail, role);

        return user;
    }

    public async Task UpdateRefreshTokenAsync(
        Guid userId,
        string refreshToken,
        DateTime expiry,
        CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(userId, cancellationToken)
            ?? throw new KeyNotFoundException($"User with id '{userId}' not found");

        user.SetRefreshToken(refreshToken, expiry);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task ClearRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(userId, cancellationToken)
            ?? throw new KeyNotFoundException($"User with id '{userId}' not found");

        user.ClearRefreshToken();
        await _db.SaveChangesAsync(cancellationToken);
    }
}