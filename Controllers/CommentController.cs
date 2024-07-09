using api.DTOs.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
#nullable disable
namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> userManager)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllComment()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _commentRepository.GetAllAsync();
            var commentDto = comment.Select(x => x.ToCommentDto());
            return Ok(comment);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }
        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, CreateUpdateCommentRequestDto commnentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _stockRepository.StockExists(stockId))
            {
                return BadRequest("Stock does not exists !");
            }
            var user = User.GetUserName();  
            var appUser = await _userManager.FindByNameAsync(user);
            
            var commentModel = commnentDto.ToCommentCreate(stockId);
            commentModel.AppUserId= appUser.Id;
            await _commentRepository.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, CreateUpdateCommentRequestDto comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var commentModel = await _commentRepository.UpdateAsync(id, comment.ToCommentCreate());
            if (commentModel == null)
            {
                return NotFound("Comment does not exists !");
            }
            return Ok(commentModel.ToCommentDto());

        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var commentModel = await _commentRepository.DeleteAsync(id);
            if (commentModel == null)
            {
                return NotFound("Comment does not exists !");
            }
            return NoContent();
        }

    }


}