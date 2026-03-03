using SystemIntegration_project.Database;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using SystemIntegration_project.Models;
using SystemIntegration_project.Services;
using SystemIntegration_project.Services.Middleware;

namespace SystemIntegration_project;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddAuthorization();

        builder.Services.AddSingleton<FlightInfoCreater>();
        builder.Services.AddDbContext<FlightContext>(op => op.UseInMemoryDatabase("FlightDb"));
        builder.Services.AddSingleton<TopicSpecifications>(_ =>
        {
            var specs = new TopicSpecifications();
            specs.TopicSpecificationsList.Add(
                new TopicSpecifications.TopicSpecification(
                    topicName: "Delayed_Flights",
                    exchangeName: "flightInfo",
                    filter: f => f.Status == "Delayed"
                )
            );
            specs.TopicSpecificationsList.Add(
                new TopicSpecifications.TopicSpecification(
                    topicName: "New_Flights",
                    exchangeName: "flightInfo",
                    filter: f => f.DepartureTime > DateTime.Now
                )
            );
            return specs;
        });
        
        //RabbitMq
        var factory = new ConnectionFactory() { HostName = "localhost" };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync("flightInfo", ExchangeType.Topic);
        
        builder.Services.AddSingleton<IChannel>(channel);

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.UseMiddleware<AfterEndpointPrintMiddleware>();
        app.UseMiddleware<AfterEndpointPublishMiddleware>();

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