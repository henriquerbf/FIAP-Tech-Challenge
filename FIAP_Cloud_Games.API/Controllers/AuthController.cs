using FIAP_Cloud_Games.Application.Auth;
using FIAP_Cloud_Games.Domain.Enums;
using FIAP_Cloud_Games.Infrastructure.Persistence;
using FIAP_Cloud_Games.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIAP_Cloud_Games.API.Controllers;

[ApiController]
[Route("api/auth")]

public class AuthController : ControllerBase
{
    private readonly CloudGamesDbContext _db;
    private readonly IJwtTokenService _jwt;

    public AuthController(CloudGamesDbContext db, IJwtTokenService jwt)
    {
        _db = db; _jwt = jwt;
    }

    public record LoginRequest(string Email, string Password);

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email, ct);
        if (user == null) return Unauthorized(new { error = "Credenciais inválidas" });

        // TODO: verificar hash de senha corretamente (BCrypt/Argon2/PBKDF2)
        if (user.Password != req.Password) return Unauthorized(new { error = "Credenciais inválidas" });

        var token = _jwt.GenerateToken(user.Id, user.Email, user.Role);
        return Ok(new { token, role = user.Role.ToString(), userId = user.Id });
    }
}
