using DataAccess;
using KanbanBoard.Services;
using KanbanBoard.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace KanbanBoard;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "KanbanBoard", Version = "v1" });
        });

        services.AddAutoMapper(typeof(Startup).Assembly);

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        services.AddDbContext<KanbanContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("Local"));
        });


        services.AddScoped<IKanbanService, KanbanService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}