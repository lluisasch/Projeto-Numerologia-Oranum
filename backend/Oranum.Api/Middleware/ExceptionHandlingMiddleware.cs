using System.Text.Json;
using Oranum.Domain.Exceptions;

namespace Oranum.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (DomainValidationException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = exception.Message,
                type = "validation_error"
            }, JsonOptions));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Erro nao tratado durante o processamento da requisicao.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = "Nao foi possivel concluir a leitura agora. Tente novamente em instantes.",
                type = "server_error"
            }, JsonOptions));
        }
    }
}
