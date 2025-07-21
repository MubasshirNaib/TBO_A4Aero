using Backend.Core.Dtos;

namespace Backend.Core.Interfaces
{
    public interface IFlightSearchService
    {
        FlightSearchResponse SearchFlights(FlightSearchRequest request);
    }
}