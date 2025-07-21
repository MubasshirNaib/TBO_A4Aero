using Backend.Core.Dtos;
using Backend.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Backend.Infrastructure.Clients
{
    public class AgencyApiClient : IAgencyApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AgencyApiSettings _settings;
        private readonly ILogger<AgencyApiClient> _logger;

        public AgencyApiClient(HttpClient httpClient, IOptions<AgencyApiSettings> settings, ILogger<AgencyApiClient> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<ValidateAgencyResponse> ValidateAgencyAsync(ValidateAgencyRequest request)
        {
            try
            {
                var apiUrl = $"{_settings.BaseUrl.TrimEnd('/')}/Authenticate/ValidateAgency";
                _logger.LogInformation("Sending request to external API: {ApiUrl}", apiUrl);

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Received response from external API");
                var result = JsonSerializer.Deserialize<ValidateAgencyResponse>(responseContent);

                return result ?? new ValidateAgencyResponse
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Invalid response received" }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling external API: {Message}", ex.Message);
                return new ValidateAgencyResponse
                {
                    IsSuccess = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public FlightSearchResponse SearchFlights(FlightSearchRequest request)
        {
            try
            {
                var apiUrl = $"{_settings.BaseUrl.TrimEnd('/')}/Search/Search";
                _logger.LogInformation("Sending flight search request to external API: {ApiUrl}", apiUrl);

                var content = new StringContent(
                    JsonSerializer.Serialize(request, new JsonSerializerOptions { IgnoreNullValues = true }),
                    Encoding.UTF8,
                    "application/json");

                var response = _httpClient.PostAsync(apiUrl, content).Result; // Synchronous call
                response.EnsureSuccessStatusCode();

                var responseContent = response.Content.ReadAsStringAsync().Result;
                _logger.LogInformation("Received flight search response from external API: {ResponseContent}", responseContent);

                try
                {
                    var result = JsonSerializer.Deserialize<FlightSearchResponse>(responseContent);
                    return result ?? new FlightSearchResponse
                    {
                        IsSuccess = false,
                        Errors = new List<ErrorDetail> { new ErrorDetail { Message = "Invalid response received" } }
                    };
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "JSON deserialization error: {Message}", ex.Message);
                    return new FlightSearchResponse
                    {
                        IsSuccess = false,
                        Errors = new List<ErrorDetail> { new ErrorDetail { Message = $"Deserialization failed: {ex.Message}" } }
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling flight search API: {Message}", ex.Message);
                return new FlightSearchResponse
                {
                    IsSuccess = false,
                    Errors = new List<ErrorDetail> { new ErrorDetail { Message = ex.Message } }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in flight search: {Message}", ex.Message);
                return new FlightSearchResponse
                {
                    IsSuccess = false,
                    Errors = new List<ErrorDetail> { new ErrorDetail { Message = ex.Message } }
                };
            }
        }
    }
}