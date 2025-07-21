using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Dtos
{
    public class FlightAvailabilityRQ
    {
        [JsonProperty("OriginDestinationOptions", NullValueHandling = NullValueHandling.Ignore)]
        public List<OriginDestinationOption>? OriginDestinationOptions { get; set; }

        [JsonProperty("Passengers", NullValueHandling = NullValueHandling.Ignore)]
        public List<PassengerInfo>? Passengers { get; set; }

        [JsonProperty("CabinClasses", NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? CabinClasses { get; set; }

        [JsonProperty("CabinClasse", NullValueHandling = NullValueHandling.Ignore)]
        public string? CabinClass { get; set; }

        [JsonProperty("FareType", NullValueHandling = NullValueHandling.Ignore)]
        public string? FareType { get; set; }

        [JsonProperty("PreferredAirlines", NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? PreferredAirlines { get; set; }

        [JsonProperty("PreferredAirline", NullValueHandling = NullValueHandling.Ignore)]
        public string? PreferredAirline { get; set; }

        [JsonProperty("MultipleBrandedFares", NullValueHandling = NullValueHandling.Ignore)]
        public bool? MultipleBrandedFares { get; set; }

        [JsonProperty("isMultipleBrandedFares", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsMultipleBrandedFares { get; set; } = null;

        [JsonProperty("isBaggageInfo", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsBaggageInfo { get; set; } = null;

        [JsonProperty("isFareRules", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsFareRules { get; set; } = null;

        [JsonProperty("isRichContent", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsRichContent { get; set; } = null;

        [JsonProperty("EnableHashmap", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EnableHashmap { get; set; } = null;

        [JsonProperty("maxStops", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxStops { get; set; }

        [JsonProperty("resultCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? ResultCount { get; set; }

        public int ApiId { get; set; }
    }

    public class OriginDestinationOption
    {
        [JsonProperty("DepartureAirport", NullValueHandling = NullValueHandling.Ignore)]
        public string? DepartureAirport { get; set; }

        [JsonProperty("ArrivalAirport", NullValueHandling = NullValueHandling.Ignore)]
        public string? ArrivalAirport { get; set; }

        [JsonProperty("FlyDate", NullValueHandling = NullValueHandling.Ignore)]
        public string? FlyDate { get; set; }
    }

    public class PassengerInfo
    {
        [JsonProperty("PassengerType", NullValueHandling = NullValueHandling.Ignore)]
        public string PassengerType { get; set; } = "";

        [JsonProperty("Quantity", NullValueHandling = NullValueHandling.Ignore)]
        public int Quantity { get; set; } = 0;
    }
}
