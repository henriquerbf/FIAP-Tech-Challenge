using FIAP_Cloud_Games.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace FIAP_Cloud_Games.API.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain error: {Message}", ex.Message);
            await WriteProblem(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized: {Message}", ex.Message);
            await WriteProblem(context, HttpStatusCode.Unauthorized, "Acesso não autorizado.");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "NotFound: {Message}", ex.Message);
            await WriteProblem(context, HttpStatusCode.NotFound, "Recurso não encontrado.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error");
            await WriteProblem(context, HttpStatusCode.InternalServerError, "Erro interno inesperado.");
        }
    }

    private static async Task WriteProblem(HttpContext ctx, HttpStatusCode code, string detail)
    {
        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.StatusCode = (int)code;

        var problem = new
        {
            type = "about:blank",
            title = code.ToString(),
            status = (int)code,
            detail,
            traceId = ctx.TraceIdentifier
        };

        var json = JsonSerializer.Serialize(problem);
        await ctx.Response.WriteAsync(json);
    }
}
