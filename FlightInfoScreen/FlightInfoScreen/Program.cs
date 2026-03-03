using System.Diagnostics;
using System.Text;
using Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace FlightInfoScreen;

class Program
{
    static async Task Main(string[] args)
    {
        string id = Guid.NewGuid().ToString();
        Console.WriteLine($"Flight info screen {id}...");
        
        var factory = new ConnectionFactory() { HostName = "localhost" };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        
        var queue = await channel.QueueDeclareAsync(queue: $"InfoScreen-{id}");
        await channel.ExchangeDeclareAsync(exchange: "flightInfo", type: ExchangeType.Topic);

        Console.WriteLine("Topic options:");
        Console.WriteLine("1. Delayed flights");
        Console.WriteLine("2. New Flights");
        Console.WriteLine("User input:");

        string topic = Console.ReadLine() switch
        {
            "1" => "Delayed_Flights",
            "2" => "New_Flights",
            _ => ""
        };
        
        await channel.QueueBindAsync(queue: queue.QueueName, exchange: "flightInfo", routingKey: topic);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            List<FlightInfo> flights = JsonSerializer.Deserialize<List<FlightInfo>>(message);
            PrintFlights(flights);
            return Task.CompletedTask;
        };
        await channel.BasicConsumeAsync(queue.QueueName, autoAck: true, consumer: consumer);

        Console.ReadLine();
    }
    
    public static void PrintFlights(List<FlightInfo> flights)
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