using FIAP_Cloud_Games.Infrastructure.Persistence.Data;
using FIAP_Cloud_Games.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// [ADICIONADO] Serilog lï¿½ do appsettings (Serilog section) e habilita como logger da aplicaï¿½ï¿½o
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.

// Connection string
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

// [ADICIONADO] Logs estruturados de request/response via Serilog
app.UseSerilogRequestLogging();

// [ADICIONADO] Middleware centralizado de erros (devolve ProblemDetails JSON)
app.UseMiddleware<ErrorHandlingMiddleware>();

// app.UseMiddleware<RequestLoggingMiddleware>(); // [OPCIONAL] se quiser log custom de tempo/claims

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
