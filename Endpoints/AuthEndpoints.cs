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

            user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays));
            await db.SaveChangesAsync();

            var response = new AuthResponse(
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes));

            return ApiResults.Ok(response, "Login successful");
        });
    }
}