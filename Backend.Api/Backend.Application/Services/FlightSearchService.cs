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

        public FlightSearchResponse SearchFlights(FlightSearchRequest request)
        {
            return _agencyApiClient.SearchFlights(request);
        }
    }
}