using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
#nullable disable
namespace api.Controllers
{
    [Route("/api/account")]
    [ApiController]
    public class AccountControler : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager;
        public AccountControler(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.EmailAddress

                };
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(new NewUserDto
                        {
                            UserName = appUser.UserName,
                            EmailAddress = appUser.Email,
                            Token = _tokenService.CreateToken(appUser)
                        });
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LonginDto longinDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == longinDto.Username.ToLower());
            if (user == null)
                return Unauthorized("Invalid !");
            var result = await _signinManager.CheckPasswordSignInAsync(user, longinDto.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Username does not exist or Password incorrect !");
            return Ok(new NewUserDto
            {
                UserName = user.UserName,
                EmailAddress = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        }

    }
}