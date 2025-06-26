using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model.Shift
{
    class Timekeeper_log_Insert
    {
    }

    public class Timekeeper_log_User_Insert_parameter
    {
        public int AccountMapID { get; set; }
        public int EmployeeShiftID { get; set; }
        public DateTime? LogTime { get; set; }
        public int ClockType { get; set; }
        public int CurrentBranchId { get; set; }
        public int ConnectionType { get; set; }
        public int TimeKeeperDevice { get; set; }
        public string Bssid { get; set; }
        public string Ssid { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
