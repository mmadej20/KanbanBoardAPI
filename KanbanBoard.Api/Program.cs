using KanbanBoard.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace KanbanBoard.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = services.GetRequiredService<KanbanContext>();

                var retry = 0;
                var maxRetry = 10;
                while (retry < maxRetry)
                {
                    try
                    {
                        logger.LogInformation("Applying database migrations if database does not exist");
                        context.Database.Migrate();
                        break;
                    }
                    catch (Exception)
                    {
                        retry++;
                        logger.LogWarning("Database not ready yet - {Retry}/{MaxRetry}", retry, maxRetry);
                        Thread.Sleep(2500);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during database initialization");
                throw;
            }
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}