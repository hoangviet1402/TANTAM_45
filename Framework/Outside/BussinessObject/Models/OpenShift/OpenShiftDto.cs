using System;
using System.Collections.Generic;

namespace BussinessObject.Models.OpenShift
{
    public class CreateOpenShiftRequest
    {
        public List<string> branch_ids { get; set; }
        public List<string> position_ids { get; set; }
        public int total_employees { get; set; }
        public string shift_id { get; set; }
        public string working_day { get; set; }
        /// <summary>
        /// Trạng thái nháp: 1 = nháp (draft), 0 = đã xuất bản (published)
        /// Mặc định là 1 (draft) khi tạo mới
        /// </summary>
        public int is_draft { get; set; } = 1; // Mặc định là draft (1 = true)
    }

    /// <summary>
    /// Publish OpenShift request model
    /// </summary>
    public class PublishOpenShiftRequest
    {
        public List<string> ids { get; set; }
    }

    /// <summary>
    /// List OpenShift request model
    /// </summary>
    public class ListOpenShiftRequest
    {
        public string start_date { get; set; }
        public string end_date { get; set; }
    }

    /// <summary>
    /// OpenShift list item for API response (matches sample format)
    /// </summary>
    public class OpenShiftListItemDto
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
        public OpenShiftStatusDto status { get; set; }
        public int registered_employees { get; set; }
    }

    /// <summary>
    /// OpenShift status DTO
    /// </summary>
    public class OpenShiftStatusDto
    {
        public int not_available { get; set; }
        public List<string> status_color { get; set; }
    }

    /// <summary>
    /// List OpenShift response (array of arrays, each representing a day)
    /// </summary>
    public class ListOpenShiftResponse
    {
        public List<List<OpenShiftListItemDto>> data { get; set; }

        public ListOpenShiftResponse()
        {
            data = new List<List<OpenShiftListItemDto>>();
        }
    }

    /// <summary>
    /// Request for getting shift list by working day
    /// </summary>
    public class ShiftListByWorkingDayRequest
    {
        public int page { get; set; } = 1;
        public int status { get; set; } = 1;
        public string working_day { get; set; }
        public int is_all { get; set; } = 1;
    }

    /// <summary>
    /// Shift item for shift list by working day response
    /// </summary>
    public class ShiftListByWorkingDayItemDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string timezone { get; set; }
    }
} 