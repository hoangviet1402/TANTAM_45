using System.Collections.Generic;
using BussinessObject.Enum;
using MyUtility.Extensions;

namespace BussinessObject.Permission
{
    public static class WebPermissionKeys
    {
        // Employee
        public const string EmployeeViewList = "web_employee_view_list";
        public const string EmployeeCreate = "web_employee_create";
        public const string EmployeeDelete = "web_employee_delete";
        public const string EmployeeUpdate = "web_employee_update";
        public const string EmployeeShowEmailAndPhone = "web_employee_show_email_and_phone";
        public const string EmployeeViewAllowanceInfo = "web_employee_view_allowance_info";
        public const string EmployeeUpdateTimeTrackingConfig = "web_employee_update_timetracking_config";
        public const string EmployeeExportFileEmployee = "web_employee_export_file_employee";
        public const string EmployeeExportDayLeftEmployee = "web_employee_export_day_left_employee";
        public const string EmployeeExportPromotionHistory = "web_employee_export_promotion_history";
        public const string EmployeeViewPromotionHistory = "web_employee_view_promotion_history";
        // Shift
        public const string ShiftViewList = "web_shift_view_list";
        public const string ShiftCreate = "web_shift_create";
        public const string ShiftUpdate = "web_shift_update";
        public const string ShiftDelete = "web_shift_delete";
        public const string ShiftExportList = "web_shift_export_list";
        public const string ShiftViewSummaryEmployee = "web_shift_view_summary_employee";
        public const string ShiftAssignCreate = "web_shift_assign_create";
        public const string ShiftAssignDelete = "web_shift_assign_delete";
        public const string ShiftAssignExportList = "web_shift_assign_export_list";
        // Task
        public const string TaskViewProjectList = "web_task_view_project_list";
        public const string TaskExportAssignWork = "web_task_export_assign_work";
        public const string TaskExportRequestApprovalProcess = "web_task_export_request_approval_process";
        public const string TaskViewWorkList = "web_task_view_work_list";
    }

    public static class MobilePermissionKeys
    {
        public const string MobileWorkTimekeeping = "mobile_work_timekeeping";
    }
}