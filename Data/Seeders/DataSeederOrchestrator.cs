namespace PortfolioApi.Data.Seeders;

public class DataSeederOrchestrator
{
    private readonly IEnumerable<ISeeder> _seeders;
    private readonly ILogger<DataSeederOrchestrator> _logger;

    public DataSeederOrchestrator(
        IEnumerable<ISeeder> seeders,
        ILogger<DataSeederOrchestrator> logger)
    {
        _seeders = seeders;
        _logger = logger;
    }

    public async Task SeedAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting database seeding...");

        var orderedSeeders = _seeders.OrderBy(s => s.Order).ToList();

        foreach (var seeder in orderedSeeders)
        {
            try
            {
                _logger.LogInformation("Running seeder: {SeederName}", seeder.GetType().Name);
                await seeder.SeedAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error running seeder: {SeederName}", seeder.GetType().Name);
                throw;
            }
        }

        _logger.LogInformation("Database seeding completed successfully.");
    }
}