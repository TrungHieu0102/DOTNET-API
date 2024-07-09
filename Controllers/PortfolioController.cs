
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
#nullable disable
namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var user = User.GetUserName();
            var appUSer = await _userManager.FindByNameAsync(user);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUSer);
            return Ok(userPortfolio);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepository.GetBySymbolAsync(symbol);
            if (stock == null)
            {
                return BadRequest("Không tìm thấy Stock !");
            }
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            if (userPortfolio.Any(p => p.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Stock đã tồn tại");
            }
            var portfolio = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id,
            };
            await _portfolioRepository.CreatePortfolioAsync(portfolio);
            if (portfolio == null)
            {
                return StatusCode(500, "Không thể tạo do stock Null !");
            }
            return Created();
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(user);
            var filterStock = userPortfolio.Where(p => p.Symbol.ToLower() == symbol.ToLower());
            if (filterStock.Count() == 1)
            {
                await _portfolioRepository.DeletePortfolioAsync(user,symbol);
            }
            else{
                return StatusCode(500, "Stock không tồn tại");
            }
            return Ok();

        }


    }
}