using Backend.Core.Dtos;
using Backend.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.Api.Controllers
{
    [ApiController]
   // [Route("api/{apiVersion}/Authenticate/[controller]")]
    [Route("api/{apiVersion}/Authenticate/")]
    public class AgencyController : ControllerBase
    {
        private readonly IAgencyValidationService _agencyValidationService;
        private readonly ILogger<AgencyController> _logger;

        public AgencyController(IAgencyValidationService agencyValidationService, ILogger<AgencyController> logger)
        {
            _agencyValidationService = agencyValidationService;
            _logger = logger;
        }

        [HttpPost("ValidateAgency")]
        public async Task<IActionResult> ValidateAgency([FromBody] ValidateAgencyRequest request)
        {
            _logger.LogInformation("Received ValidateAgency request for UserName: {UserName}", request.UserName);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for ValidateAgency request");
                return BadRequest(ModelState);
            }

            var response = await _agencyValidationService.ValidateAgencyAsync(request);
            if (!response.IsSuccess)
            {
                _logger.LogError("Agency validation failed: {Errors}", string.Join(", ", response.Errors));
                return BadRequest(response);
            }

            _logger.LogInformation("Agency validation successful, TokenId: {TokenId}", response.TokenId);
            return Ok(response);
        }
    }
}