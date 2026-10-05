using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PortfolioApi.Common;
using PortfolioApi.Data;
using PortfolioApi.DTOs;

namespace PortfolioApi.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Authentication");

        group.MapPost("/login", async (
            LoginRequest req,
            ITokenService tokenService,
            IOptions<JwtSettings> jwtOptions,
            AppDbContext db) =>
        {
            var email = req.Email.ToLowerInvariant();
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                return ApiResults.Fail("Invalid email or password");

            var accessToken = tokenService.GenerateAccessToken(user);
            var refreshToken = tokenService.GenerateRefreshToken();
            var jwtSettings = jwtOptions.Value;

            user.SetRefreshToken(
                refreshToken,
                DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays));
            await db.SaveChangesAsync();

            var userInfo = new UserInfoDto(
                user.Id,
                user.Email,
                user.FullName,
                user.Role);

            var response = new AuthResponse(
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
                userInfo);

            return ApiResults.Ok(response, "Login successful");
        });

        group.MapPost("/refresh", async (
            RefreshRequest req,
            ITokenService tokenService,
            IOptions<JwtSettings> jwtOptions,
            AppDbContext db) =>
        {
            var user = await db.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == req.RefreshToken);

            if (user is null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return ApiResults.Unauthorized("Invalid or expired refresh token");

            var newAccessToken = tokenService.GenerateAccessToken(user);
            var newRefreshToken = tokenService.GenerateRefreshToken();
            var jwtSettings = jwtOptions.Value;

            user.SetRefreshToken(
                newRefreshToken,
                DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays));
            await db.SaveChangesAsync();

            var userInfo = new UserInfoDto(
                user.Id,
                user.Email,
                user.FullName,
                user.Role);

            var response = new AuthResponse(
                newAccessToken,
                newRefreshToken,
                DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
                userInfo);

            return ApiResults.Ok(response, "Token refreshed");
        });
    }
}