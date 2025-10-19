using KanbanBoard.Application.Services;
using KanbanBoard.DataAccess;
using KanbanBoard.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;

namespace KanbanBoard.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "KanbanBoard", Version = "v1", Description = "Simple API to manage boards with tasks and members" });
            c.CustomSchemaIds(s => s.FullName.Replace("+", "."));
        });

        services.AddAutoMapper((config) =>
        {
            config.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
        });

        services.AddMediatR((config) =>
        {
            config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });

        services.AddDbContextFactory<KanbanContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("Local"));
        });

        services.AddScoped<IBoardService, BoardService>();
        services.AddScoped<IMemberService, MemberService>();

        var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

        services.AddSerilog(logger);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KanbanBoard v1"));
        }

        app.UseRouting();

        app.UseCors((options) =>
        {
            options.AllowAnyOrigin();
            options.AllowAnyMethod();
        });

        //app.UseMiddleware<ApiKeyMiddleware>();
        app.UseAuthorization();
        app.UseSerilogRequestLogging();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}