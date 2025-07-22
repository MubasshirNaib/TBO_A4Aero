using Backend.Core.Dtos;
using Backend.Infrastructure.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class RequestMapping
    {
       

        public static FlightSearchRequest MapToFlightSearchRequest(FlightAvailabilityRQ request, ILogger<AgencyApiClient> _logger, AgencyApiSettings _settings)
        {
            var adultCount = request.Passengers?.Find(p => p.PassengerType.ToLower() == "adult")?.Quantity;
            var childCount = request.Passengers?.Find(p => p.PassengerType.ToLower() == "child")?.Quantity;
            var infantCount = request.Passengers?.Find(p => p.PassengerType.ToLower() == "infant")?.Quantity;

            var apiRequest = new FlightSearchRequest
            {

                IPAddress = _settings.IPAddress,
                TokenId = _settings.TokenId,
                EndUserBrowserAgent = _settings.EndUserBrowserAgent,
                PointOfSale = _settings.PointOfSale,
                RequestOrigin = _settings.RequestOrigin,
                UserData = _settings.UserData,
                AdultCount = adultCount ?? 1,
                ChildCount = childCount ?? 0,
                InfantCount = infantCount ?? 0,
                Segment = new List<FlightSegment>(),
                PreferredAirlines = request.PreferredAirlines ?? (request.PreferredAirline != null ? new List<string> { request.PreferredAirline } : new List<string>()),
                FlightCabinClass = MapCabinClass(request.CabinClass ?? (request.CabinClasses?.FirstOrDefault() ?? "Economy")),
                JourneyType = DetermineJourneyType(request.OriginDestinationOptions)
            };

            //// Map passengers
            //if (request.Passengers != null)
            //{
            //    apiRequest.AdultCount = 1;
            //    apiRequest.ChildCount = 0;
            //    apiRequest.InfantCount = 0;


            //    foreach (var passenger in request.Passengers)
            //    {
            //        switch (passenger.PassengerType?.ToLower())
            //        {
            //            case "adult":
            //                apiRequest.AdultCount = passenger.Quantity; 
            //                break;
            //            case "child":
            //                apiRequest.ChildCount = passenger.Quantity; 
            //                break;
            //            case "infant":
            //                apiRequest.InfantCount = passenger.Quantity;
            //                break;
            //        }
            //    }
            //}

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

        private static int MapCabinClass(string cabinClass)
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

        private static int DetermineJourneyType(List<OriginDestinationOption>? options)
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
