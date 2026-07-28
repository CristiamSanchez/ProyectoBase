using System.Net;
using System.Text.Json;
using Proyecto.Api.Models;

namespace Proyecto.Api.Middleware;


public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;


    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
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
        catch(Exception ex)
        {
            _logger.LogError(
                ex,
                "Error no controlado"
            );


            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;


            context.Response.ContentType =
                "application/json";


            var response =
                ApiResponse<string>.Error(
                    "Ocurrió un error interno en el servidor"
                );


            var json =
                JsonSerializer.Serialize(response);


            await context.Response.WriteAsync(json);
        }
    }
}