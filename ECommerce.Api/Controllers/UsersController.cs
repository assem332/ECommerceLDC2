using Azure.Messaging;
using ECommerce.Application.DTOs.Users;
using ECommerce.Application.Services;
using ECommerce.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthService _authService;

        public UsersController(AuthService authService) => _authService = authService;

        [HttpPost("register")]
        [AllowAnonymous]

        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var user = await _authService.RegisterAsync(dto);
                return Ok(new
                {
                    Message = "Register successful",
                });
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var (user, token) = await _authService.LoginAsync(dto);
                return Ok(new
                {
                    message = "Login successful",
                    user,
                    token
                });
            }
            catch (Exception ex)    
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet ("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var claims = User.Claims;
            return Ok(new
            {
                Id = claims.FirstOrDefault(c => c.Type == "sub")?.Value,
                Email = claims.FirstOrDefault(c=>c.Type=="email")?.Value,
                Role = claims.FirstOrDefault(c=>c.Type=="role")?.Value,
                PhoneNumber = claims.FirstOrDefault(c=>c.Type=="phoneNumber")?.Value,
                Address = claims.FirstOrDefault(c=>c.Type=="address")?.Value

            });
        }

    }
}
