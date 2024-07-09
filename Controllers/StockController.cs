using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        public readonly IStockRepository _stockRepository;
        public StockController(ApplicationDBContext dbContext, IStockRepository stockRepository)
        {
            _dbContext = dbContext;
            _stockRepository = stockRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stock = await _stockRepository.GetAllAsync(query);
            var stockDto = stock.Select(x => x.ToStockDto()).ToList();
            return Ok(stockDto);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateUpdateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stockModel = stockDto.ToStockFromCreateDTO();
            await _stockRepository.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel);

        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] CreateUpdateStockRequestDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stockModel = await _stockRepository.UpdateAsync(id, updateDto);
            if (stockModel == null)
            {
                return NotFound();
            }


            await _dbContext.SaveChangesAsync();
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stockModel = await _stockRepository.DeleteAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}

