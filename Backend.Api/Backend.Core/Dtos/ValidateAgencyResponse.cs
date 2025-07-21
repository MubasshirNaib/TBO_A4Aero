using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Dtos
{
    public class ValidateAgencyResponse
    {
        public AgencyDetails Agency { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Alerts { get; set; }
        public string TokenId { get; set; }
        public string TrackingId { get; set; }
    }

    public class AgencyDetails
    {
        public decimal TotalAvailableLimit { get; set; }
        public string Currency { get; set; }
        public string LocalCurrency { get; set; }
        public decimal LocalCurrencyROE { get; set; }
    }
}
