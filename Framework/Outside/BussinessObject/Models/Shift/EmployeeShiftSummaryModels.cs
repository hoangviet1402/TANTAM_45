using System;
using System.Collections.Generic;

namespace BussinessObject.Models.Shift
{
    public class EmployeeShiftSummaryResponse
    {
        public SummaryMeta meta { get; set; }
        public List<EmployeeShiftItem> items { get; set; }

        public EmployeeShiftSummaryResponse()
        {
            meta = new SummaryMeta();
            items = new List<EmployeeShiftItem>();
        }
    }

    public class SummaryMeta
    {
        public int total { get; set; }
        public int count { get; set; }
        public int per_page { get; set; }
        public int current_page { get; set; }
        public int total_pages { get; set; }

        public SummaryMeta()
        {
            per_page = 10;
            current_page = 1;
        }
    }

    public class EmployeeShiftItem
    {
        public string user_id { get; set; }
        public string employee_id { get; set; }
        public string phone { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string company_id { get; set; }
        public string identification { get; set; }
        public List<object> payroll { get; set; }
        public Dictionary<string, List<ShiftDetailItem>> shifts { get; set; }
        public Dictionary<string, string> conflict_shifts { get; set; }
        public decimal total_working_hour { get; set; }
        public decimal real_working_hour { get; set; }

        public EmployeeShiftItem()
        {
            payroll = new List<object>();
            shifts = new Dictionary<string, List<ShiftDetailItem>>();
            conflict_shifts = new Dictionary<string, string>();
        }
    }

    public class EmployeeBranchObject
    {
        public string id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
    }

    public class ShiftDetailItem
    {
        public string id { get; set; }
        public string name { get; set; }
        public string shift_key { get; set; }
        public string shift_id { get; set; }
        public string shift_type { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public decimal working_hour { get; set; }
        public string working_day { get; set; }
        public int week_of_year { get; set; }
        public List<EmployeeBranchObject> branch_obj { get; set; }
        public string company_id { get; set; }
        public string checkin_time { get; set; }
        public string checkout_time { get; set; }
        public int is_confirm { get; set; }
        public int is_overtime_shift { get; set; }
        public decimal meal_coefficient { get; set; }
        public string timezone { get; set; }
        public int is_open_shift { get; set; }
        public string dynamic_user_id { get; set; }
        public string checkin_type { get; set; }
        public string checkout_type { get; set; }
        public string checkin_log_id { get; set; }
        public string checkout_log_id { get; set; }
        public string checkin_branch_id { get; set; }
        public string checkout_branch_id { get; set; }
        public CheckinOption checkin_option { get; set; }
        public CheckoutOption checkout_option { get; set; }
        public string shift_name { get; set; }
        public DisplayOption display_option { get; set; }
        public decimal real_working_hour { get; set; }
        public int real_working_minute { get; set; }
        public string rest_start_time_short { get; set; }
        public string rest_end_time_short { get; set; }
        public decimal coefficient { get; set; }
        public decimal real_coefficient { get; set; }
        public ShiftStatus status { get; set; }
        public bool approved { get; set; }

        public ShiftDetailItem()
        {
            shift_type = "hard";
            is_confirm = 1;
            is_overtime_shift = 0;
            meal_coefficient = 0;
            timezone = "Asia/Saigon";
            is_open_shift = 0;
            checkin_type = "";
            checkout_type = "";
            rest_start_time_short = "00:00";
            rest_end_time_short = "00:00";
            coefficient = 1;
            real_coefficient = 0;
            approved = false;
            branch_obj = new List<EmployeeBranchObject>();
            display_option = new DisplayOption();
            status = new ShiftStatus();
        }
    }

    public class CheckinOption
    {
        public string type { get; set; }
        public string name { get; set; }
        public string type_name { get; set; }
    }

    public class CheckoutOption
    {
        public string type { get; set; }
        public string name { get; set; }
        public string type_name { get; set; }
    }

    public class DisplayOption
    {
        public string shift_name { get; set; }
        public string option_type { get; set; }
        public string option_name { get; set; }

        public DisplayOption()
        {
            option_type = "";
            option_name = "";
        }
    }

    public class ShiftStatus
    {
        public string color { get; set; }
        public List<string> status_color { get; set; }
        public string name { get; set; }
        public int not_available { get; set; }
        public List<string> detail { get; set; }

        public ShiftStatus()
        {
            color = "#666666";
            status_color = new List<string> { "#838BA3", "#EBEBEB" };
            name = "Chưa vào/ra ca";
            not_available = 0;
            detail = new List<string> { "Thời gian: 0 giờ" };
        }
    }
} 