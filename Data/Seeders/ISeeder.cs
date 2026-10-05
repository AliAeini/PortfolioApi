namespace PortfolioApi.Data.Seeders;

public interface ISeeder
{
    int Order { get; }
    Task SeedAsync(CancellationToken cancellationToken = default);
}