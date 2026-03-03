using Models;

namespace SystemIntegration_project.Services;

public class FlightInfoCreater
{
    public FlightInfo GenerateRandomFlight()
    {
        Random rng = new Random();

        string[] destinations = { "London", "Paris", "New York", "Tokyo", "Dubai", "Berlin", "Madrid", "Rome" };
        string[] airlines = { "SK", "DY", "AA", "LH", "BA", "EK", "AF" };
        string[] statuses = { "OnTime", "Delayed", "Boarding", "Departed", "Cancelled" };
        string[] gateLetters = { "A", "B", "C", "D" };

        string flightNumber = airlines[rng.Next(airlines.Length)] + rng.Next(100, 999);
        string destination  = destinations[rng.Next(destinations.Length)];
        string gate         = gateLetters[rng.Next(gateLetters.Length)] + rng.Next(1, 20);
        string status       = statuses[rng.Next(statuses.Length)];
        DateTime departure  = DateTime.Now.AddMinutes(rng.Next(-60, 300));

        return new FlightInfo(flightNumber, destination, departure, gate, status);
    }
}