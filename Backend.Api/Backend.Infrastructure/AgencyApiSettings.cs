namespace Backend.Infrastructure
{
    public class AgencyApiSettings
    {
        public string BaseUrl { get; set; }
        public string IPAddress { get; set; }
        public string TokenId { get; set; }
        public string EndUserBrowserAgent { get; set; }
        public string PointOfSale { get; set; }
        public string RequestOrigin { get; set; }
        public string UserData { get; set; }

        public int Initial {  get; set; }
    }
}