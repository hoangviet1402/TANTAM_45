using System;
using System.Collections.Generic;

namespace DataAccess.Model.OpenShift
{
    /// <summary>
    /// OpenShift status model for API response
    /// </summary>
    public class OpenShiftStatusModel
    {
        public int not_available { get; set; }
        public List<string> status_color { get; set; }

        public OpenShiftStatusModel()
        {
            status_color = new List<string> { "#838BA3", "#EBEBEB" };
        }
    }

    /// <summary>
    /// OpenShift main detail result model
    /// </summary>
    public class OpenShiftDetailResult
    {
        public string id { get; set; }
        public string shift_name { get; set; }
        public int total_employees { get; set; }
        public string shift_id { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string working_day { get; set; }
        public string timezone { get; set; }
        public bool is_draft { get; set; }
        public OpenShiftStatusModel status { get; set; }
        public List<OpenShiftBranchResult> branches { get; set; }
        public List<OpenShiftPositionResult> positions { get; set; }
        public List<OpenShiftUserResult> users { get; set; }
        public List<object> departments { get; set; }

        public OpenShiftDetailResult()
        {
            branches = new List<OpenShiftBranchResult>();
            positions = new List<OpenShiftPositionResult>();
            users = new List<OpenShiftUserResult>();
            departments = new List<object>();
        }
    }

    /// <summary>
    /// OpenShift branch result model
    /// </summary>
    public class OpenShiftBranchResult
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    /// <summary>
    /// OpenShift position result model
    /// </summary>
    public class OpenShiftPositionResult
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    /// <summary>
    /// OpenShift user result model
    /// </summary>
    public class OpenShiftUserResult
    {
        public string id { get; set; }
        public string name { get; set; }
        public string employee_code { get; set; }
        public string position { get; set; }
        public string avatar { get; set; }
        public int status { get; set; }
        public string registered_at { get; set; }
    }

    /// <summary>
    /// Complete detail result containing all OpenShift data - FLATTENED for direct API response
    /// </summary>
    public class OpenShiftCompleteDetailResult
    {
        public string id { get; set; }
        public string shift_name { get; set; }
        public int total_employees { get; set; }
        public string shift_id { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string working_day { get; set; }
        public string timezone { get; set; }
        public bool is_draft { get; set; }
        public OpenShiftStatusModel status { get; set; }
        public List<OpenShiftBranchResult> branches { get; set; }
        public List<OpenShiftPositionResult> positions { get; set; }
        public List<OpenShiftUserResult> users { get; set; }
        public List<object> departments { get; set; }

        public OpenShiftCompleteDetailResult()
        {
            branches = new List<OpenShiftBranchResult>();
            positions = new List<OpenShiftPositionResult>();
            users = new List<OpenShiftUserResult>();
            departments = new List<object>();
        }
    }
} 