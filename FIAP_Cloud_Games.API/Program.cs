using FIAP_Cloud_Games.Infrastructure.Data;
using FIAP_Cloud_Games.Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Connection string
string teste = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<CloudGamesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// aplica migrations + seeding na inicialização
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CloudGamesDbContext>();
    await DatabaseSeeder.SeedAsync(db);
}

app.MapHealthChecks("/health");

app.MapGet("/ping-db", async (CloudGamesDbContext db) =>
{
    // apenas um teste rápido
    var totalUsers = await db.Users.CountAsync();
    var totalGames = await db.Games.CountAsync();
    var totalLinks = await db.UserGames.CountAsync();
    return Results.Ok(new { totalUsers, totalGames, totalLinks });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
