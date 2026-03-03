using SystemIntegration_project.Database;

namespace SystemIntegration_project.Services.Middleware;

public class AfterEndpointMiddleware
{
    private readonly RequestDelegate _next;

    public AfterEndpointMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context); //Kalder endpointet før vi laver vores logger

        var dbInstance = context.RequestServices.GetRequiredService<FlightContext>();
        dbInstance.PrintFlights();
    }
}