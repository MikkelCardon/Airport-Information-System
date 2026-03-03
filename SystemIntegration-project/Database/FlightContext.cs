using Microsoft.EntityFrameworkCore;
using Models;
using SystemIntegration_project.Models;

namespace SystemIntegration_project.Database;

public class FlightContext : DbContext
{
    public FlightContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<FlightInfo> flights { get; set; }

    public void PrintFlights()
    {
        if (flights == null || !flights.Any())
        {
            Console.WriteLine("No flights available.");
            return;
        }

        // Table header
        Console.WriteLine("+-------------+----------------+---------------------+--------+------------+");
        Console.WriteLine("| Flight      | Destination    | Departure Time      | Gate   | Status     |");
        Console.WriteLine("+-------------+----------------+---------------------+--------+------------+");

        foreach (var flight in flights.OrderBy(f => f.DepartureTime))
        {
            // Format each row
            Console.WriteLine(
                $"| {flight.FlightNumber,-11} | {flight.Destination,-14} | {flight.DepartureTime,-19:yyyy-MM-dd HH:mm} | {flight.Gate,-6} | {flight.Status,-10} |"
            );
        }

        // Table footer
        Console.WriteLine("+-------------+----------------+---------------------+--------+------------+");
    }
}