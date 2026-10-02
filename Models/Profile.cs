namespace PortfolioApi.Models;

public class Profile
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public string? Email { get; set; }
    public string? Location { get; set; }
}