
using Backend.Core.Dtos;
using Backend.Core.Interfaces;

namespace Backend.Application.Services
{
    public class AgencyValidationService : IAgencyValidationService
    {
        private readonly IAgencyApiClient _agencyApiClient;

        public AgencyValidationService(IAgencyApiClient agencyApiClient)
        {
            _agencyApiClient = agencyApiClient;
        }

        public async Task<ValidateAgencyResponse> ValidateAgencyAsync(ValidateAgencyRequest request)
        {
            return await _agencyApiClient.ValidateAgencyAsync(request);
        }
    }
}