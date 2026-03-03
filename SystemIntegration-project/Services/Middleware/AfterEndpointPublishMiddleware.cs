using System.Text;
using System.Text.Json;
using Models;
using SystemIntegration_project.Database;
using SystemIntegration_project.Models;
using RabbitMQ.Client;

namespace SystemIntegration_project.Services.Middleware;

public class AfterEndpointPublishMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TopicSpecifications _topicSpecifications;
    private readonly IChannel _channel;

    public AfterEndpointPublishMiddleware(RequestDelegate next, TopicSpecifications topicSpecifications, IChannel channel)
    {
        _next = next;
        _topicSpecifications = topicSpecifications;
        _channel = channel;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context); //Kalder endpointet før vi laver vores logger

        var dbInstance = context.RequestServices.GetRequiredService<FlightContext>();
        List<FlightInfo> flightList = dbInstance.flights.ToList();
        
        foreach (var topicSpecification in _topicSpecifications.TopicSpecificationsList)
        {
            List<FlightInfo> data = flightList.Where(f => topicSpecification.Filter(f)).ToList();

            var json = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(json);
            
            await _channel.BasicPublishAsync(
                exchange: topicSpecification.ExchangeName,
                routingKey: topicSpecification.TopicName,
                body: body
            );
            Console.WriteLine($"Published {data.Count} items to topic: {topicSpecification.TopicName}");
        }
    }
}