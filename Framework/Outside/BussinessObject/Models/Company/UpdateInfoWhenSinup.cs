using System.Collections.Generic;
using Newtonsoft.Json;

namespace BussinessObject.Models.Company
{
    public class UpdateInfoWhenSinupRequest
    {
        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public int AccountId { get; set; }

        [JsonProperty("shop", NullValueHandling = NullValueHandling.Ignore)]
        public int CompanyId { get; set; }

        [JsonProperty("shop_name", NullValueHandling = NullValueHandling.Ignore)]
        public string CompanyName { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public float? CompanyLatitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public float? CompanyLongitude { get; set; }

        [JsonProperty("number_employee_expected", NullValueHandling = NullValueHandling.Ignore)]
        public string CompanyNumberEmploye { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string CompanyAddress { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("hear_about_tantam", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> HearAbout { get; set; }

        [JsonProperty("use_purpose", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> UsePurpose { get; set; }

        [JsonProperty("business_field_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BusinesFieldIds { get; set; }
    }
}
// {
//   "shop_name": "Shop thức ăn cho mèo",
//   "business_field_ids": [
//     "63db2f01f9edc77d561d17ac"
//   ],
//   "latitude": 10.80437,
//   "longitude": 106.71927,
//   "email": "vietnguyenhuy.1402@gmail.com",
//   "number_employee_expected": "1-15",
//   "address": "Hồ Chí Minh, Việt Nam",
//   "use_purpose": [
//     "timekeeping"
//   ],
//   "hear_about_tanca": [
//     "Khác"
//   ]
// }