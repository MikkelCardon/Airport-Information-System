using System.ComponentModel.DataAnnotations;

namespace Models;

public class FlightInfo
{
    [Key]
    public string FlightNumber { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureTime { get; set; }
    public string Gate { get; set; }

    private enum FlightStatus
    {
        OnTime,
        Delayed,
        Boarding,
        Departed,
        Cancelled
    }

    public FlightInfo()
    {
    }

    public FlightInfo(string flightNumber, string destination, DateTime departureTime, string gate, string status)
    {
        FlightNumber = flightNumber;
        Destination = destination;
        DepartureTime = departureTime;
        Gate = gate;
        Status = status; //Bruger setter for at validere.
    }

    private FlightStatus _status;

    public string Status
    {
        get { return _status.ToString(); }
        set
        {
            if (Enum.TryParse(value, out FlightStatus parsedStatus))
            {
                _status = parsedStatus;
            }
            else
            {
                throw new ArgumentException("Invalid status value.");
            }
        }
    }
}