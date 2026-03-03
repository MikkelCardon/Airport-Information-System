using SystemIntegration_project.Database;
using Microsoft.EntityFrameworkCore;
using SystemIntegration_project.Models;
using SystemIntegration_project.Services.Middleware;

namespace SystemIntegration_project;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddAuthorization();

        builder.Services.AddDbContext<FlightContext>(op => op.UseInMemoryDatabase("FlightDb"));
        builder.Services.AddSingleton<TopicSpecifications>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.UseMiddleware<AfterEndpointPrintMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}