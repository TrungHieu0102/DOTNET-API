
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
#nullable disable
namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public PortfolioRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Portfolio> CreatePortfolioAsync(Portfolio portfolio)
        {
            await _dbContext.AddAsync(portfolio);
            await _dbContext.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolioAsync(AppUser appUser, string symbol)
        {
            var portfolioModel = await _dbContext.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == appUser.Id);
            if (portfolioModel == null)
            {
                return null;
            }
            _dbContext.Portfolios.Remove(portfolioModel);
            await _dbContext.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _dbContext.Portfolios.Where(u => u.AppUserId == user.Id)
            .Select(stock => new Stock
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap
            }).ToListAsync();
        }
    }
}
