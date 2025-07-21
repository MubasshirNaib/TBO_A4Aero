using Backend.Core.Dtos;
using Backend.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/v1/Search")]
    public class SearchController : ControllerBase
    {
        private readonly IFlightSearchService _flightSearchService;
        private readonly ILogger<SearchController> _logger;

        public SearchController(IFlightSearchService flightSearchService, ILogger<SearchController> logger)
        {
            _flightSearchService = flightSearchService;
            _logger = logger;
        }

        [HttpPost("Search")]
        public IActionResult Search([FromBody] FlightSearchRequest request)
        {
            _logger.LogInformation("Received flight search request for JourneyType: {JourneyType}", request.JourneyType);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for flight search request: {Errors}", string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            var response = _flightSearchService.SearchFlights(request);
            if (!response.IsSuccess)
            {
                _logger.LogError("Flight search failed: {Errors}", string.Join(", ", response.Errors.Select(e => e.ToString())));
                return BadRequest(response);
            }

            _logger.LogInformation("Flight search successful, TrackingId: {TrackingId}", response.TrackingId);
            return Ok(response);
        }
    }
}