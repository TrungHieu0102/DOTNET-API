
using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfolio> CreatePortfolioAsync(Portfolio portfolio);
        Task<Portfolio> DeletePortfolioAsync(AppUser appUser, string symbol);
    }
}