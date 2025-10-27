using FIAP_Cloud_Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIAP_Cloud_Games.Infrastructure.Data.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(CloudGamesDbContext db, CancellationToken ct = default)
        {
            // cria DB caso não exista (útil em dev)
            await db.Database.MigrateAsync(ct);

            if (!await db.Games.AnyAsync(ct))
            {
                var g1 = new Game("Hollow Knight", "Metroidvania indie", "Action", 100.00m, new DateTime(2017, 02, 24), 0.10m);
                var g2 = new Game("Celeste", "Plataforma desafiador", "Platform", 80.00m, new DateTime(2018, 01, 25), 0.15m);
                var g3 = new Game("Stardew Valley", "Fazendinha", "Simulation", 60.00m, new DateTime(2016, 02, 26), 0.00m);

                db.Games.AddRange(g1, g2, g3);
            }

            if (!await db.Users.AnyAsync(ct))
            {
                var u = new User(name: "Fillipy", role: "Admin", email: "lip-crs@hotmail.com", password: "Str0ng@Pwd");
                db.Users.Add(u);

                // relaciona com 2 jogos usando a tabela de junção
                // (temos que salvar antes para garantir Ids, já que eles são gerados no constructor)
                await db.SaveChangesAsync(ct);

                var hollow = await db.Games.FirstAsync(g => g.Title == "Hollow Knight", ct);
                var celeste = await db.Games.FirstAsync(g => g.Title == "Celeste", ct);

                db.UserGames.AddRange(
                    new UserGame { UserId = u.Id, GameId = hollow.Id, AddedAt = DateTime.UtcNow },
                    new UserGame { UserId = u.Id, GameId = celeste.Id, AddedAt = DateTime.UtcNow }
                );
            }

            await db.SaveChangesAsync(ct);
        }
    }
}
