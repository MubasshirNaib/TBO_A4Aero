using Backend.Core.Dtos;
using Backend.Core.Interfaces;


namespace Backend.Application.Services
{
    public class FlightSearchService : IFlightSearchService
    {
        private readonly IAgencyApiClient _agencyApiClient;

        public FlightSearchService(IAgencyApiClient agencyApiClient)
        {
            _agencyApiClient = agencyApiClient;
        }

        public async Task<FlightSearchResponse> SearchFlights(FlightAvailabilityRQ request)
        {
            return await _agencyApiClient.SearchFlights(request);
        }
    }
}