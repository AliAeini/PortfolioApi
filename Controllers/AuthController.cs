using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PortfolioApi.Common;
using PortfolioApi.Data;
using PortfolioApi.DTOs;
using PortfolioApi.Models;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/auth")]
[Tags("Authentication")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;

    public AuthController(
        AppDbContext db,
        ITokenService tokenService,
        IOptions<JwtSettings> jwtOptions)
    {
        _db = db;
        _tokenService = tokenService;
        _jwtSettings = jwtOptions.Value;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct)
    {
        var email = req.Email.ToLowerInvariant();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

        if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return ApiResults.Fail("Invalid email or password");

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays));
        await _db.SaveChangesAsync(ct);

        var profile = await _db.Profiles
            .Where(p => p.CreatedByUserId == user.Id)
            .Select(p => new { p.Id })
            .FirstOrDefaultAsync(ct);

        var userInfo = new UserInfoDto(user.Id, user.Email, user.FullName, user.Role);

        var response = new AuthResponse(
            accessToken,
            refreshToken,
            DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            userInfo,
            profile?.Id ?? Guid.Empty);

        return ApiResults.Ok(response, "Login successful");
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest req, CancellationToken ct)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.RefreshToken == req.RefreshToken, ct);

        if (user is null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            return ApiResults.Unauthorized("Invalid or expired refresh token");

        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.SetRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays));
        await _db.SaveChangesAsync(ct);

        var profile = await _db.Profiles
            .Where(p => p.CreatedByUserId == user.Id)
            .Select(p => new { p.Id })
            .FirstOrDefaultAsync(ct);

        var userInfo = new UserInfoDto(user.Id, user.Email, user.FullName, user.Role);

        var response = new AuthResponse(
            newAccessToken,
            newRefreshToken,
            DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            userInfo,
            profile?.Id ?? Guid.Empty);

        return ApiResults.Ok(response, "Token refreshed");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req, CancellationToken ct)
    {
        var email = req.Email.Trim().ToLowerInvariant();

        if (await _db.Users.AnyAsync(u => u.Email == email, ct))
            return ApiResults.Fail("This email is already registered.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(req.Password);
        var user = PortfolioApi.Models.User.Create(email, passwordHash, req.FullName, "Owner");
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        var profile = Profile.Create(
            req.FullName!,
            bio: "Welcome to my portfolio!",
            email: email);

        profile.SetCreatedBy(user.Id);
        _db.Profiles.Add(profile);
        await _db.SaveChangesAsync(ct);

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays));
        await _db.SaveChangesAsync(ct);

        var userInfo = new UserInfoDto(user.Id, user.Email, user.FullName, user.Role);

        var response = new AuthResponse(
            accessToken,
            refreshToken,
            DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            userInfo,
            profile.Id);

        return ApiResults.Ok(response, "Registration successful");
    }
}