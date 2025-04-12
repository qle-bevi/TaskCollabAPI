using System.Net;
using System.Text.Json;
using TaskCollab.Application.Common.Exceptions;

namespace TaskCollab.API.Middleware;

public class ExceptionHandlingMiddleware
{
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new
                {
                    title = "Erreur de validation",
                    status = (int)statusCode,
                    errors = validationException.Errors
                });
                break;
                
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new
                {
                    title = "Ressource non trouvée",
                    status = (int)statusCode,
                    error = notFoundException.Message
                });
                break;
                
            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                result = JsonSerializer.Serialize(new
                {
                    title = "Non autorisé",
                    status = (int)statusCode,
                    error = exception.Message
                });
                break;
                
            default:
                _logger.LogError(exception, "Une erreur non gérée est survenue");
                result = JsonSerializer.Serialize(new
                {
                    title = "Une erreur est survenue",
                    status = (int)statusCode,
                    error = "Une erreur interne est survenue. Veuillez contacter l'administrateur."
                });
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(result);
    }
}