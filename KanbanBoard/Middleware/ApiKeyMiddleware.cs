using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace KanbanBoard.Api.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;

    private const string _apiKey = "XApiKey";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(_apiKey, out
                var providedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is invalid or not provided");
            return;
        }
        var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
        var correctApiKey = appSettings.GetValue<string>(_apiKey);
        if (!correctApiKey.Equals(providedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is invalid or not provided");
            return;
        }
        await _next(context);
    }
}