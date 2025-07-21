using Backend.Core.Dtos;

namespace Backend.Core.Interfaces
{
    public interface IFlightSearchService
    {
        // FlightSearchResponse SearchFlights(FlightAvailabilityRQ request);
        Task<FlightSearchResponse> SearchFlights(FlightAvailabilityRQ request);
    }
}