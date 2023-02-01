using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace KanbanBoard.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;

    private const string APIKEY = "XApiKey";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(APIKEY, out
                var providedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is invalid or not provided");
            return;
        }
        var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
        var correctApiKey = appSettings.GetValue<string>(APIKEY);
        if (!correctApiKey.Equals(providedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is invalid or not provided");
            return;
        }
        await _next(context);
    }
}