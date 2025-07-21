using Backend.Core.Dtos;
using Backend.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
                    System.Text.Json.JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Received response from external API");
                var result = System.Text.Json.JsonSerializer.Deserialize<ValidateAgencyResponse>(responseContent);

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

        public async Task<FlightSearchResponse> SearchFlights(FlightAvailabilityRQ request)
        {
            try
            {
                // Map FlightAvailabilityRQ to FlightSearchRequest
                var apiRequest = MapToFlightSearchRequest(request);

                var apiUrl = $"{_settings.BaseUrl.TrimEnd('/')}/Search/Search";
                _logger.LogInformation("Sending flight search request to external API: {ApiUrl}", apiUrl);

                var content = new StringContent(
                    JsonConvert.SerializeObject(apiRequest, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Received flight search response from external API: {ResponseContent}", responseContent);

                try
                {
                    var result = JsonConvert.DeserializeObject<FlightSearchResponse>(responseContent);
                    return result ?? new FlightSearchResponse
                    {
                        IsSuccess = false,
                        Errors = new List<ErrorDetail> { new ErrorDetail { Message = "Invalid response received" } }
                    };
                }
                catch (Newtonsoft.Json.JsonException ex)
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
        private FlightSearchRequest MapToFlightSearchRequest(FlightAvailabilityRQ request)
        {
            var apiRequest = new FlightSearchRequest
            {
                IPAddress = "0.0.0.0", // Default or fetch from context (e.g., HttpContext)
                TokenId = "1984019b-c746-4347-8099-d7bb91512be6", // Generate or fetch from auth context
                EndUserBrowserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36", // Default or fetch from context
                PointOfSale = "ID", // Set based on requirements
                RequestOrigin = "Indonesia", // Default or set based on client
                UserData = null, // Not mapped from FlightAvailabilityRQ
                Segment = new List<FlightSegment>(),
                PreferredAirlines = request.PreferredAirlines ?? (request.PreferredAirline != null ? new List<string> { request.PreferredAirline } : new List<string>()),
                AdultCount = 0,
                ChildCount = 0,
                InfantCount = 0,
                FlightCabinClass = MapCabinClass(request.CabinClass ?? (request.CabinClasses?.FirstOrDefault() ?? "Economy")),
                JourneyType = DetermineJourneyType(request.OriginDestinationOptions)
            };

            // Map passengers
            if (request.Passengers != null)
            {
                foreach (var passenger in request.Passengers)
                {
                    switch (passenger.PassengerType?.ToLower())
                    {
                        case "adult":
                            apiRequest.AdultCount = passenger.Quantity;
                            break;
                        case "child":
                            apiRequest.ChildCount = passenger.Quantity;
                            break;
                        case "infant":
                            apiRequest.InfantCount = passenger.Quantity;
                            break;
                    }
                }
            }

            // Map segments
            if (request.OriginDestinationOptions != null)
            {
                foreach (var option in request.OriginDestinationOptions)
                {
                    if (DateTime.TryParse(option.FlyDate, out var flyDate))
                    {
                        apiRequest.Segment.Add(new FlightSegment
                        {
                            Origin = option.DepartureAirport ?? string.Empty,
                            Destination = option.ArrivalAirport ?? string.Empty,
                            PreferredDepartureTime = flyDate,
                            PreferredArrivalTime = flyDate.AddHours(1) // Placeholder; adjust based on API needs
                        });
                    }
                    else
                    {
                        _logger.LogWarning("Invalid FlyDate format for segment: {FlyDate}", option.FlyDate);
                    }
                }
            }

            // Validate request
            if (apiRequest.Segment.Count == 0)
            {
                throw new ArgumentException("At least one flight segment is required.");
            }
            if (apiRequest.AdultCount + apiRequest.ChildCount + apiRequest.InfantCount == 0)
            {
                throw new ArgumentException("At least one passenger is required.");
            }

            return apiRequest;
        }

        private int MapCabinClass(string cabinClass)
        {
            return cabinClass.ToLower() switch
            {
                "economy" => 1,
                "premiumeconomy" => 2,
                "business" => 3,
                "first" => 4,
                _ => 1 // Default to Economy
            };
        }

        private int DetermineJourneyType(List<OriginDestinationOption>? options)
        {
            if (options == null || options.Count == 0)
                return 1; // Default to OneWay
            if (options.Count == 1)
                return 1; // OneWay
            if (options.Count == 2 && options[0].DepartureAirport == options[1].ArrivalAirport &&
                options[0].ArrivalAirport == options[1].DepartureAirport)
                return 2; // Return
            return 3; // Multi-Stop
        }

    }
}