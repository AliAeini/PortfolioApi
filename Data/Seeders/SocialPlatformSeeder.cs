namespace PortfolioApi.Data.Seeders;

public class SocialPlatformSeeder : ISeeder
{
    private readonly ILogger<SocialPlatformSeeder> _logger;

    public SocialPlatformSeeder(ILogger<SocialPlatformSeeder> logger)
    {
        _logger = logger;
    }

    public int Order => 30;

    public Task SeedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Social platforms are handled via enum. Skipping seed.");
        return Task.CompletedTask;
    }
}