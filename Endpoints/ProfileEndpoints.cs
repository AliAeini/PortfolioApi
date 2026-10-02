using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.Data;
using PortfolioApi.DTOs;
using PortfolioApi.Models;

namespace PortfolioApi.Endpoints;

public static class ProfileEndpoints
{
    public static void MapProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/profiles")
                       .WithTags("Profiles");

        group.MapGet("/", async (AppDbContext db) =>
        {
            var profiles = await db.Profiles
                .Select(p => new ProfileDto(p.Id, p.FullName, p.Bio,
                                            p.AvatarUrl, p.Email, p.Location))
                .ToListAsync();

            return ApiResults.Ok(profiles, $"{profiles.Count} profile(s) found");
        });

        group.MapGet("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var profile = await db.Profiles.FindAsync(id);
            if (profile is null)
                return ApiResults.NotFound($"Profile with id '{id}' not found");

            var dto = new ProfileDto(profile.Id, profile.FullName, profile.Bio,
                                     profile.AvatarUrl, profile.Email, profile.Location);
            return ApiResults.Ok(dto);
        });

        group.MapPost("/", async (CreateProfileRequest req, AppDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(req.FullName))
                return ApiResults.Fail("FullName is required",
                    new List<string> { "FullName cannot be empty" });

            if (string.IsNullOrWhiteSpace(req.Bio))
                return ApiResults.Fail("Bio is required",
                    new List<string> { "Bio cannot be empty" });

            var profile = new Profile
            {
                FullName = req.FullName,
                Bio = req.Bio,
                AvatarUrl = req.AvatarUrl,
                Email = req.Email,
                Location = req.Location
            };

            db.Profiles.Add(profile);
            await db.SaveChangesAsync();

            var dto = new ProfileDto(profile.Id, profile.FullName, profile.Bio,
                                     profile.AvatarUrl, profile.Email, profile.Location);
            return ApiResults.Created($"/api/profiles/{profile.Id}", dto,
                                      "Profile created successfully");
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateProfileRequest req, AppDbContext db) =>
        {
            var profile = await db.Profiles.FindAsync(id);
            if (profile is null)
                return ApiResults.NotFound($"Profile with id '{id}' not found");

            if (string.IsNullOrWhiteSpace(req.FullName))
                return ApiResults.Fail("FullName is required",
                    new List<string> { "FullName cannot be empty" });

            profile.FullName = req.FullName;
            profile.Bio = req.Bio;
            profile.AvatarUrl = req.AvatarUrl;
            profile.Email = req.Email;
            profile.Location = req.Location;

            await db.SaveChangesAsync();

            var dto = new ProfileDto(profile.Id, profile.FullName, profile.Bio,
                                     profile.AvatarUrl, profile.Email, profile.Location);
            return ApiResults.Ok(dto, "Profile updated successfully");
        });

        group.MapDelete("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var profile = await db.Profiles.FindAsync(id);
            if (profile is null)
                return ApiResults.NotFound($"Profile with id '{id}' not found");

            db.Profiles.Remove(profile);
            await db.SaveChangesAsync();

            return ApiResults.Ok("Profile deleted successfully");
        });
    }
}