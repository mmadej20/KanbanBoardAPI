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
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using Newtonsoft.Json;

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
        //    .AddNewtonsoftJson(options =>
        //     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        //); 
        //Remove JSONIgnore from properties and uncomment this to get response with full object

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "KanbanBoard", Version = "v1", Description="Simple API to manage boards with tasks and members" });
            c.CustomSchemaIds(s => s.FullName.Replace("+", "."));

        });

        services.AddAutoMapper(typeof(Startup).Assembly);

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        services.AddDbContextFactory<KanbanContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("Local"));
        });

        //services.AddDbContext<KanbanContext>(options =>
        //{
        //    options.UseSqlServer(Configuration.GetConnectionString("Local"));
        //});

        services.AddScoped<IBoardService, BoardService>();
        services.AddScoped<IMemberService, MemberService>();
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