using System.Reflection.Metadata.Ecma335;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public StockRepository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _dbContext.Stocks.AddAsync(stockModel);
            await _dbContext.SaveChangesAsync();
            return stockModel;

        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            _dbContext.Stocks.Remove(stockModel);
            await _dbContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stock = _dbContext.Stocks.Include(x => x.Comments).ThenInclude(a=>a.appUser).AsQueryable();
            if (!string.IsNullOrEmpty(query.CompanyName))
            {
                stock = stock.Where(x => x.CompanyName.Contains(query.CompanyName));

            }
            if (!string.IsNullOrEmpty(query.Symbol))
            {
                stock = stock.Where(x => x.Symbol.Contains(query.Symbol));
            }
            if (!string.IsNullOrEmpty(query.SorBy))
            {
                if (query.SorBy.Equals("SymBol", StringComparison.OrdinalIgnoreCase))
                {
                    stock = query.IsDecsending ? stock.OrderByDescending(x => x.Symbol) : stock.OrderBy(x => x.Symbol);
                }
            }
            var skipNumber = (query.PageNumber-1) * query.PageSize;
            return await stock.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }   



        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _dbContext.Stocks.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _dbContext.Stocks.FirstOrDefaultAsync(x=>x.Symbol == symbol);
        }

        public async Task<bool> StockExists(int id)
        {
            return await _dbContext.Stocks.AnyAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, CreateUpdateStockRequestDto stockDto)
        {
            var stockModel = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            stockModel.Symbol = stockDto.Symbol;
            stockModel.Purchase = stockDto.Purchase;
            stockModel.MarketCap = stockDto.MarketCap;
            stockModel.LastDiv = stockDto.LastDiv;
            stockModel.Industry = stockDto.Industry;
            stockModel.CompanyName = stockDto.CompanyName;
            await _dbContext.SaveChangesAsync();
            return stockModel;

        }
    }
}
