namespace PortfolioApi.Services;

public class UserHandler : IUserHandler
{
    private readonly IUserService _userService;
    private readonly ILogger<UserHandler> _logger;

    public UserHandler(IUserService userService, ILogger<UserHandler> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<Guid?> HandleProfileOwnerAsync(
        Guid profileId,
        string email,
        string password,
        string fullName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            _logger.LogWarning("Profile {ProfileId} created without email; no owner created.", profileId);
            return null;
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            _logger.LogWarning("Profile {ProfileId} created without valid password; no owner created.", profileId);
            return null;
        }

        var existingOwner = await _userService.GetByCreatedByAsync(profileId, cancellationToken);
        if (existingOwner is not null)
        {
            _logger.LogInformation(
                "Profile {ProfileId} already has owner {UserId}",
                profileId, existingOwner.Id);
            return existingOwner.Id;
        }

        if (await _userService.EmailExistsAsync(email, cancellationToken))
        {
            _logger.LogWarning(
                "Email {Email} is already taken; cannot create owner for profile {ProfileId}.",
                email, profileId);
            return null;
        }

        var owner = await _userService.CreateAsync(
            email: email,
            password: password,
            fullName: fullName,
            role: "Owner",
            createdByUserId: profileId,
            cancellationToken: cancellationToken);

        _logger.LogInformation(
            "Owner created for profile {ProfileId}: {Email}",
            profileId, owner.Email);

        return owner.Id;
    }
}