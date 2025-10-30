using FIAP_Cloud_Games.Infrastructure.Persistence.Data;
using FIAP_Cloud_Games.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Connection string
string teste = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<CloudGamesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddHealthChecks();

var app = builder.Build();

// aplica migrations + seeding na inicialização
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CloudGamesDbContext>();
    await db.Database.MigrateAsync();        // garante as migrations
    await DatabaseSeeder.SeedAsync(db);      // chama seu seeder existente
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Mapeia o endpoint de health check
    app.MapHealthChecks("/health");

    app.MapGet("/db/ping", async (CloudGamesDbContext db) =>
    {
        // apenas um teste rápido
        var totalUsers = await db.Users.CountAsync();
        var totalGames = await db.Games.CountAsync();
        var totalLinks = await db.UserGames.CountAsync();
        return Results.Ok(new { totalUsers, totalGames, totalLinks });
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
