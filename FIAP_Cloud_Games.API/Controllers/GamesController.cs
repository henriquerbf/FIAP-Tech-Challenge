using FIAP_Cloud_Games.Domain.Entities;
using FIAP_Cloud_Games.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FIAP_Cloud_Games.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly CloudGamesDbContext _context;
        public GamesController(CloudGamesDbContext context) => _context = context;

        // GET: api/<GamesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> Get()
        {
            // usa o _context normalmente
            return await _context.Games.AsNoTracking().ToListAsync();
        }

        // GET api/<GamesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> Get(Guid id)
        {
            return await _context.Games.FindAsync(id);
        }

        // POST api/<GamesController>
        [HttpPost]
        public async Task<ActionResult<Game>> AddGame(Game Game)
        {
            _context.Games.Add(Game);
            await _context.SaveChangesAsync(); // executa o INSERT

            return CreatedAtAction(nameof(Get), new { id = Game.Id }, Game);
        }

        // PUT api/<GamesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(Guid id, Game Game)
        {
            if (id != Game.Id)
                return BadRequest("O ID da URL não corresponde ao do corpo da requisição.");

            _context.Entry(Game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); // executa o UPDATE
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                    return NotFound(); // se o usuário não existe mais
                else
                    throw; // re-lança se for outro tipo de erro
            }

            return NoContent(); // 204
        }

        // DELETE api/<GamesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var Game = await _context.Games.FindAsync(id);
            if (Game == null)
                return NotFound();

            _context.Games.Remove(Game);
            await _context.SaveChangesAsync(); // executa o DELETE

            return NoContent();
        }

        private bool GameExists(Guid id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
