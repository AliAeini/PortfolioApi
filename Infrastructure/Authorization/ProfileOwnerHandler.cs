using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;

namespace PortfolioApi.Infrastructure.Authorization;

public class ProfileOwnerHandler : AuthorizationHandler<ProfileOwnerRequirement>
{
    private readonly AppDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProfileOwnerHandler(
        AppDbContext db,
        IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ProfileOwnerRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return;

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
            return;

        var profileIdValue = httpContext.Request.RouteValues["profileId"]?.ToString()
                          ?? httpContext.Request.RouteValues["id"]?.ToString();

        if (string.IsNullOrEmpty(profileIdValue) || !Guid.TryParse(profileIdValue, out var profileId))
            return;

        var isOwner = await _db.Profiles
            .AsNoTracking()
            .AnyAsync(p => p.Id == profileId && p.CreatedByUserId == userId);

        if (isOwner)
            context.Succeed(requirement);
    }
}