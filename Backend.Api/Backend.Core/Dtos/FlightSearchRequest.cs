namespace Backend.Core.Dtos
{
    public class FlightSearchRequest
    {
        public string IPAddress { get; set; }
        public string TokenId { get; set; }
        public string EndUserBrowserAgent { get; set; }
        public string PointOfSale { get; set; }
        public string RequestOrigin { get; set; }
        public object? UserData { get; set; } // Nullable
        public int JourneyType { get; set; } // 1: OneWay, 2: Return, 3: Multi-Stop
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public int FlightCabinClass { get; set; }
        public List<FlightSegment> Segment { get; set; }
        public List<string> PreferredAirlines { get; set; }
    }

    public class FlightSegment
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime PreferredDepartureTime { get; set; }
        public DateTime PreferredArrivalTime { get; set; }
    }
}