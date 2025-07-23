using Backend.Core.Dtos;
using Backend.Infrastructure.appSettingsData;
using Backend.Infrastructure.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class Validation
    {
        private readonly HttpClient _httpClient;
        private readonly AgencyCredentials _credentials;
        public Validation(HttpClient httpClient, IOptions<AgencyCredentials> credentials)
        {
            _httpClient = httpClient;
            _credentials = credentials.Value;
        }

        public  async Task<string> SearchValidateAgencyAsync(ILogger<AgencyApiClient> _logger, AgencyApiSettings _settings)
        {
            var validationApiUrl = $"{_settings.BaseUrl.TrimEnd('/')}/Authenticate/ValidateAgency";
            _logger.LogInformation("Sending validation request to external API: {ApiUrl}", validationApiUrl);

            var agencyCredentials = new
            {
                _credentials.UserName,
                _credentials.Password,
                _credentials.BookingMode,
                _credentials.IPAddress
            };

            var validationContent = new StringContent(
                JsonSerializer.Serialize(agencyCredentials),
                Encoding.UTF8,
                "application/json");

            var validationResponse = await _httpClient.PostAsync(validationApiUrl, validationContent);
            if (!validationResponse.IsSuccessStatusCode)
            {
                _logger.LogError("Validation API call failed with status code: {StatusCode}", validationResponse.StatusCode);
                throw new HttpRequestException($"Validation failed with status code: {validationResponse.StatusCode}");
            }

            var validationResponseContent = await validationResponse.Content.ReadAsStringAsync();
            _logger.LogInformation("Received validation response from external API");

            var validationResult = JsonSerializer.Deserialize<ValidateAgencyResponse>(validationResponseContent);
            if (validationResult == null || !validationResult.IsSuccess || string.IsNullOrEmpty(validationResult.TokenId))
            {
                _logger.LogError("Validation failed or no Token_Id received. Errors: {Errors}", validationResult?.Errors ?? new List<string> { "Invalid response received" });
                throw new InvalidOperationException("Validation failed or no Token_Id received.");
            }

            return validationResult.TokenId;
        }
    }
}
