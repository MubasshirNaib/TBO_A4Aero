using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.appSettingsData
{
    public class ApiRequestDefaults
    {
        public string IPAddress { get; set; }
        public string TokenId { get; set; }
        public string EndUserBrowserAgent { get; set; }
        public string PointOfSale { get; set; }
        public string RequestOrigin { get; set; }
        public string UserData { get; set; }
    }

}
