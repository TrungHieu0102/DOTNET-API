using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllComment()
        {
            var comment = await _commentRepository.GetAllAsync();
            var commentDto = comment.Select(x => x.ToCommentDto());
            return Ok(comment);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }
        [HttpPost("{stockId}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, CreateUpdateCommentRequestDto commnentDto)
        {
            if (!await _stockRepository.StockExists(stockId))
            {
                return BadRequest("Stock does not exists !");
            }
            var commentModel = commnentDto.ToCommentCreate(stockId);
            await _commentRepository.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, CreateUpdateCommentRequestDto comment)
        {
            var commentModel = await _commentRepository.UpdateAsync(id, comment.ToCommentCreate());
            if (commentModel == null)
            {
                return NotFound("Comment does not exists !");
            }
            return Ok(commentModel.ToCommentDto());

        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id){
            var commentModel = await _commentRepository.DeleteAsync(id);
            if(commentModel == null){
                return NotFound("Comment does not exists !");
            }
            return NoContent();
        }

    }


}