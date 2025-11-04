using FIAP_Cloud_Games.Domain.Entities;
using FIAP_Cloud_Games.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FIAP_Cloud_Games.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly CloudGamesDbContext _context;
        public GameController(CloudGamesDbContext context) => _context = context;

        // GET: api/<GamesController>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Game>>> Get()
        {
            return await _context.Games.AsNoTracking().ToListAsync();
        }

        // GET api/<GamesController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Game>> Get(Guid id)
        {
            return await _context.Games.FindAsync(id);
        }

        // POST api/<GamesController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Game>> AddGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync(); // executa o INSERT

            return CreatedAtAction(nameof(Get), new { id = game.Id }, game);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Game>> UpdateGame(Game game)
        {
            var existing = await _context.Games.FindAsync(game.Id);
            if (existing == null)
                return null;

            // Use domain guard methods to update state
            existing.UpdateTitle(game.Title);
            existing.UpdateDescription(game.Description);
            existing.UpdateGenre(game.Genre);
            existing.SetReleaseDate(game.ReleaseDate);
            existing.UpdatePrice(game.Price);
            existing.SetDiscount(game.Discount);

            await _context.SaveChangesAsync();
            return existing;
        }

        // DELETE api/<GamesController>/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return NotFound();

            _context.Games.Remove(game);
            await _context.SaveChangesAsync(); // executa o DELETE

            return NoContent();
        }

        // CreatePromotion api/<GamesController>/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePromotion(Guid id, decimal discount)
        {
            discount = discount / 100; // convert to decimal
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return NotFound();

            game.SetDiscount(discount);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = game.Id }, game);

            return NoContent();
        }
    }
}
