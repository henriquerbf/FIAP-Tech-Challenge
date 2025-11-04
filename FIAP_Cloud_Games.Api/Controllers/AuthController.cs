using FIAP_Cloud_Games.Application.Auth;
using FIAP_Cloud_Games.Domain.Enums;
using FIAP_Cloud_Games.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FIAP_Cloud_Games.Domain.Enums;

namespace FIAP_Cloud_Games.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CloudGamesDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        public AuthController(IConfiguration configuration, CloudGamesDbContext context, IJwtTokenService jwtTokenService)
        {
            _configuration = configuration;
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return Unauthorized(new { message = "E-mail não encontrado." });

            // Idealmente usar hash, mas mantendo seu exemplo
            if (request.Password != user.Password)
                return Unauthorized(new { message = "Senha incorreta." });

            var token = _jwtTokenService.GenerateToken(user.Id, user.Email, user.Role);

            return Ok(new
            {
                token,
                role = user.Role.ToString()
            });
        }

    }

    public record LoginRequest(string Email, string Password);
}
