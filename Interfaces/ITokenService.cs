using PortfolioApi.Models;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}