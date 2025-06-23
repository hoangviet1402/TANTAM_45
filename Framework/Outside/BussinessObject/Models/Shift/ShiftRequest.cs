using Newtonsoft.Json;
using System.Collections.Generic;

namespace BussinessObject.Models.Shift
{
    
    #region List Open Shift Request (KEEP - still used)
    public class ListOpenShiftRequest
    {
        [JsonProperty("startdate")]
        public string StartDate { get; set; }

        [JsonProperty("enddate")]
        public string EndDate { get; set; }
    }
    #endregion
} 