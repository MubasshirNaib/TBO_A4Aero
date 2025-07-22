//using Backend.Api.Controllers;
//using Backend.Core.Dtos;
//using Backend.Core.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Moq;
//using FluentAssertions;
//using Xunit;

//namespace Backend.UnitTest.Backend.Api.Tests.Controllers
//{
//    public class SearchControllerTests
//    {
//        private readonly Mock<IFlightSearchService> _flightSearchServiceMock;
//        private readonly Mock<ILogger<SearchController>> _loggerMock;
//        private readonly SearchController _controller;

//        public SearchControllerTests()
//        {
//            _flightSearchServiceMock = new Mock<IFlightSearchService>();
//            _loggerMock = new Mock<ILogger<SearchController>>();
//            _controller = new SearchController(_flightSearchServiceMock.Object, _loggerMock.Object);
//        }

//        [Fact]
//        public async Task SearchAsync_ValidRequest_ReturnsOkResult()
//        {
//            // Arrange
//            var request = new FlightAvailabilityRQ
//            {
//                OriginDestinationOptions = new List<OriginDestinationOption>
//                {
//                    new OriginDestinationOption
//                    {
//                        DepartureAirport = "DEL",
//                        ArrivalAirport = "DXB",
//                        FlyDate = "2025-07-23"
//                    }
//                },
//                Passengers = new List<PassengerInfo>
//                {
//                    new PassengerInfo { PassengerType = "Adult", Quantity = 1 }
//                },
//                CabinClass = "Economy",
//                PreferredAirlines = new List<string> { "EK" },
//                FareType = null,
//                PreferredAirline = null,
//                MultipleBrandedFares = false,
//                isMultipleBrandedFares = false,
//                isBaggageInfo = true,
//                isFareRules = true,
//                isRichContent = false,
//                EnableHashmap = false,
//                maxStops = null,
//                resultCount = null,
//                ApiId = 0
//            };
//            var response = new FlightAvailabilityRS
//            {
//                IsSuccess = true,
//                TrackingId = Guid.NewGuid().ToString()
//            };
//            _flightSearchServiceMock.Setup(s => s.SearchFlights(request))
//                .ReturnsAsync(response);

//            // Act
//            var result = await _controller.SearchAsync(request);

//            // Assert
//            result.Should().BeOfType<OkObjectResult>()
//                .Which.Value.Should().BeEquivalentTo(response);
//        }

//        [Fact]
//        public async Task SearchAsync_InvalidModelState_ReturnsBadRequest()
//        {
//            // Arrange
//            var request = new FlightAvailabilityRQ();
//            _controller.ModelState.AddModelError("JourneyType", "JourneyType is required");

//            // Act
//            var result = await _controller.SearchAsync(request);

//            // Assert
//            result.Should().BeOfType<BadRequestObjectResult>()
//                .Which.Value.Should().BeEquivalentTo(_controller.ModelState);
//        }

//        [Fact]
//        public async Task SearchAsync_FailedServiceResponse_ReturnsBadRequest()
//        {
//            // Arrange
//            var request = new FlightAvailabilityRQ
//            {
//                OriginDestinationOptions = new List<OriginDestinationOption>
//                {
//                    new OriginDestinationOption
//                    {
//                        DepartureAirport = "DEL",
//                        ArrivalAirport = "DXB",
//                        FlyDate = "2025-07-23"
//                    }
//                },
//                Passengers = new List<PassengerInfo>
//                {
//                    new PassengerInfo { PassengerType = "Adult", Quantity = 1 }
//                }
//            };
//            var response = new FlightAvailabilityRS
//            {
//                IsSuccess = false,
//                Errors = new List<string> { "No flights available" }
//            };
//            _flightSearchServiceMock.Setup(s => s.SearchFlights(request))
//                .ReturnsAsync(response);

//            // Act
//            var result = await _controller.SearchAsync(request);

//            // Assert
//            result.Should().BeOfType<BadRequestObjectResult>()
//                .Which.Value.Should().BeEquivalentTo(response);
//        }
//    }
//}
