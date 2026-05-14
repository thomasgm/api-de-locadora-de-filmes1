using System.Net;
using System.Text.Json;

namespace LocadoraFilmes.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            status = 500,
            //message = "Ocorreu um erro interno. Tente novamente mais tarde.",
            message = ex.Message,        // ← mostra o erro real
            detail = ex.InnerException?.Message  // ← e o erro interno
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}