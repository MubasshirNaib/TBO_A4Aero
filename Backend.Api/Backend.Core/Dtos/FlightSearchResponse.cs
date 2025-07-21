namespace Backend.Core.Dtos
{
    public class FlightSearchResponse
    {
        public List<List<FlightResult>> Results { get; set; }
        public bool IsDomestic { get; set; }
        public string LocalCurrency { get; set; }
        public decimal LocalCurrencyROE { get; set; }
        public decimal Tbo_CentralMarkupValue { get; set; }
        public int Tbo_CentralMarkupValueType { get; set; }
        public object CentralMarkup { get; set; }
        public bool IsSuccess { get; set; }
        public List<ErrorDetail> Errors { get; set; }
        public List<string> Alerts { get; set; }
        public string TokenId { get; set; }
        public string TrackingId { get; set; }
    }

    public class ErrorDetail
    {
        public string Origin { get; set; }
        public string UserMessage { get; set; }
        public int? Code { get; set; }
        public string TimeStamp { get; set; }
        public string Resolution { get; set; }
        public string Message { get; set; }
        public override string ToString() => $"{Code}: {UserMessage ?? Message}";
    }

    public class FlightResult
    {
        public string ResultId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public bool IsLcc { get; set; }
        public bool NonRefundable { get; set; }
        public string AirlineRemark { get; set; }
        public Fare Fare { get; set; }
        public List<FareBreakdown> FareBreakdown { get; set; }
        public string LastTicketDate { get; set; }
        public string TicketAdvisory { get; set; }
        public List<List<Segment>> Segments { get; set; }
        public List<FareRule> FareRules { get; set; }
        public string ValidatingAirline { get; set; }
        public int TripIndicator { get; set; }
        public string ResponseTime { get; set; }
        public int JourneyType { get; set; }
        public bool IsVoidAllowed { get; set; }
        public bool IsRefundAllowed { get; set; }
        public bool IsReissueAllowed { get; set; }
        public object CorporateCodeDetail { get; set; }
        public bool IsShowUpsellOption { get; set; }
        public bool IsResultForBundleFare { get; set; }
        public object UpsellOptionsList { get; set; }
        public bool IsCheckInBaggageFare { get; set; }
        public object RequiredFieldValidators { get; set; }
        public object MiniFareRules { get; set; }
        public bool IsVATApplicable { get; set; }
        public string FareClassification { get; set; }
        public string ProviderRemarks { get; set; }
    }

    public class Fare
    {
        public decimal TotalFare { get; set; }
        public string FareType { get; set; }
        public decimal AgentMarkup { get; set; }
        public decimal OtherCharges { get; set; }
        public string AgentPreferredCurrency { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal BaseFare { get; set; }
        public decimal Tax { get; set; }
        public decimal CreditCardCharge { get; set; }
        public decimal PenaltyAmount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal VATPercentage { get; set; }
    }

    public class FareBreakdown
    {
        public string Currency { get; set; }
        public int PassengerType { get; set; }
        public int PassengerCount { get; set; }
        public decimal TotalFare { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal AgentMarkup { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal BaseFare { get; set; }
        public decimal Tax { get; set; }
        public decimal CreditCardCharge { get; set; }
        public decimal PenaltyAmount { get; set; }
        public List<TaxBreakUp> TaxBreakUpList { get; set; }
        public decimal VATAmount { get; set; }
        public decimal VATPercentage { get; set; }
    }

    public class TaxBreakUp
    {
        public string Type { get; set; }
        public decimal Amount { get; set; }
    }

    public class Segment
    {
        public int SegmentId { get; set; }
        public int NoOfSeatAvailable { get; set; }
        public object AllianceInfo { get; set; }
        public object FlightInfoIndex { get; set; }
        public string OperatingCarrier { get; set; }
        public int SegmentIndicator { get; set; }
        public string Airline { get; set; }
        public Airport Origin { get; set; }
        public Airport Destination { get; set; }
        public string FlightNumber { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string BookingClass { get; set; }
        public string AvailabiLity { get; set; }
        public int FlightStatus { get; set; }
        public string Status { get; set; }
        public string MealType { get; set; }
        public bool ETicketEligible { get; set; }
        public string AirlinePNR { get; set; }
        public string Craft { get; set; }
        public bool StopOver { get; set; }
        public int Stops { get; set; }
        public int Mile { get; set; }
        public string Duration { get; set; }
        public string GroundTime { get; set; }
        public string AccumulatedDuration { get; set; }
        public string StopPoint { get; set; }
        public string StopPointArrivalTime { get; set; }
        public string StopPointDepartureTime { get; set; }
        public string IncludedBaggage { get; set; }
        public string CabinBaggage { get; set; }
        public string CabinClass { get; set; }
        public string AdditionalBaggage { get; set; }
        public AirlineDetails AirlineDetails { get; set; }
        public string AirlineName { get; set; }
        public string DepartureDateTime { get; set; }
        public string DepartureDate { get; set; }
        public string ArrivalDateTime { get; set; }
        public string ArrivalDate { get; set; }
        public string LayoverText { get; set; }
        public List<string> InFlightServices { get; set; } // Changed to List<string> to handle array
        public string FareFamilyClass { get; set; }
    }

    public class Airport
    {
        public string AirportCode { get; set; }
        public string AirportName { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Terminal { get; set; }
    }

    public class AirlineDetails
    {
        public string AirlineCode { get; set; }
        public string FlightNumber { get; set; }
        public string Craft { get; set; }
        public string AirlineName { get; set; }
        public string OperatingCarrier { get; set; }
        public object AllianceInfo { get; set; }
    }

    public class FareRule
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Airline { get; set; }
        public string FareRestriction { get; set; }
        public string FareBasisCode { get; set; }
        public string FareRuleDetail { get; set; }
        public string DepartureDate { get; set; }
        public string FlightNumber { get; set; }
        public string FareFamilyCode { get; set; }
        public string FareRuleIndex { get; set; }
        public string FreeTextFareRuleDetail { get; set; }
    }
}