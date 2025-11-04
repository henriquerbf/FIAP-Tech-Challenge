using FIAP_Cloud_Games.Domain.Entities;
using FIAP_Cloud_Games.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP_Cloud_Games.Infrastructure.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(CloudGamesDbContext db, CancellationToken ct = default)
        {
            // cria DB caso não exista (útil em dev)
            await db.Database.MigrateAsync(ct);

            if (!await db.Games.AnyAsync(ct))
            {
                var games = new List<Game>
                {
                    new Game("Hollow Knight", "Metroidvania indie", "Action", 100.00m, new DateTime(2017, 02, 24), 0.10m),
                    new Game("Celeste", "Plataforma desafiador", "Platform", 80.00m, new DateTime(2018, 01, 25), 0.15m),
                    new Game("Stardew Valley", "Fazendinha", "Simulation", 60.00m, new DateTime(2016, 02, 26), 0.00m),
                    new Game("Hades", "Roguelike mitologia grega", "Action RPG", 120.00m, new DateTime(2020, 09, 17), 0.20m),
                    new Game("The Witcher 3: Wild Hunt", "Aventura e fantasia sombria", "RPG", 150.00m, new DateTime(2015, 05, 19), 0.25m),
                    new Game("Minecraft", "Criação e sobrevivência em blocos", "Sandbox", 90.00m, new DateTime(2011, 11, 18), 0.05m),
                    new Game("Dead Cells", "Ação 2D com elementos rogue-lite", "Action", 85.00m, new DateTime(2018, 08, 07), 0.10m),
                    new Game("Horizon Zero Dawn", "Ação e mundo aberto futurista", "Action RPG", 180.00m, new DateTime(2017, 02, 28), 0.30m),
                    new Game("Slay the Spire", "Cartas e roguelike", "Strategy", 70.00m, new DateTime(2019, 01, 23), 0.00m),
                    new Game("Terraria", "Exploração e construção em 2D", "Adventure", 60.00m, new DateTime(2011, 05, 16), 0.05m)
                };

                db.Games.AddRange(games);
            }


            if (!await db.Users.AnyAsync(ct))
            {

                List<User> users = new List<User>
                {
                    new User(name: "Fillipy",   email: "lip-crs@hotmail.com", password: "Str0ng@Pwd"),
                    new User(name: "Henrique",  email: "whatever@hotmail.com", password: "pass@123"),
                    new User(name: "Mona Lisa", email: "whatever1@hotmail.com", password: "pass@123"),
                    new User(name: "Mano Liso", email: "whatever2@hotmail.com", password: "pass@123")
                };

                foreach (var user in users)
                    user.AssignRole("admin");

                users.Add(new User(name: "Leopardo", email: "tantofaz@hotmail.com", password: "3dg4bcl5@@"));
                db.Users.AddRange(users);

                // relaciona com 2 jogos usando a tabela de junção
                // (temos que salvar antes para garantir Ids, já que eles são gerados no constructor)
                await db.SaveChangesAsync(ct);

                var hollowKnight = await db.Games.FirstAsync(g => g.Title == "Hollow Knight", ct);
                var celeste = await db.Games.FirstAsync(g => g.Title == "Celeste", ct);
                var stardewValley = await db.Games.FirstAsync(g => g.Title == "Stardew Valley", ct);

                db.UserGames.AddRange(
                    new UserGame { UserId = users[0].Id, GameId = hollowKnight.Id, AddedAt = DateTime.UtcNow },
                    new UserGame { UserId = users[0].Id, GameId = celeste.Id, AddedAt = DateTime.UtcNow },
                    new UserGame { UserId = users[1].Id, GameId = hollowKnight.Id, AddedAt = DateTime.UtcNow },
                    new UserGame { UserId = users[1].Id, GameId = stardewValley.Id, AddedAt = DateTime.UtcNow }
                );
            }

            await db.SaveChangesAsync(ct);
        }
    }
}
