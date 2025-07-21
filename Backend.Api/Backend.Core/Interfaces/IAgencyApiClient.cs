using Backend.Core.Dtos;

namespace Backend.Core.Interfaces
{
    public interface IAgencyApiClient
    {
        Task<ValidateAgencyResponse> ValidateAgencyAsync(ValidateAgencyRequest request);
        // FlightSearchResponse SearchFlights(FlightAvailabilityRQ request);
        Task<FlightSearchResponse> SearchFlights(FlightAvailabilityRQ request);
    }
}