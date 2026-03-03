using SystemIntegration_project.Database;
using SystemIntegration_project.Models;
using RabbitMQ.Client;

namespace SystemIntegration_project.Services.Middleware;

public class AfterEndpointPublishMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TopicSpecifications _topicSpecifications;

    public AfterEndpointPublishMiddleware(RequestDelegate next, TopicSpecifications topicSpecifications)
    {
        _next = next;
        _topicSpecifications = topicSpecifications;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context); //Kalder endpointet før vi laver vores logger

        var dbInstance = context.RequestServices.GetRequiredService<FlightContext>();
        
        foreach (var topicSpecification in _topicSpecifications.TopicSpecificationsList)
        {
            
        }
    }
}