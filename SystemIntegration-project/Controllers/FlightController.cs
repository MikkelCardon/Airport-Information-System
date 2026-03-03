using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemIntegration_project.Database;
using SystemIntegration_project.Models;

namespace SystemIntegration_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly FlightContext _flightContext;
        
        public FlightController(FlightContext flightContext)
        {
            _flightContext = flightContext;
        }
        
        [HttpPost]
        public ActionResult<FlightInfo> Create([FromBody] FlightInfo flightInfo)
        {
            _flightContext.flights.Add(flightInfo);
            _flightContext.SaveChanges();

            return flightInfo;
        }

        [HttpPut("{flightNumber}")]
        public async Task<ActionResult<FlightInfo>> Update(string flightNumber, [FromBody] FlightInfo flightInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (flightNumber != flightInfo.FlightNumber)
            {
                return BadRequest("FlightNumber in route and body do not match. :(");
            }

            FlightInfo flight = await _flightContext.flights.FindAsync(flightNumber);
            if (flight == null)
            {
                return NotFound($"Flight {flightNumber} not found.");
            }

            flight.Destination = flightInfo.Destination;
            flight.DepartureTime = flightInfo.DepartureTime;
            flight.Gate = flightInfo.Gate;
            flight.Status = flightInfo.Status;

            await _flightContext.SaveChangesAsync();
                
            return flightInfo;
        }
        
    }
}
