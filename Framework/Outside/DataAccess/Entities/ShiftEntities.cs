using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class Ins_Shift_Create_Parameter
    {
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string NameNosign { get; set; }
        public string ShiftKey { get; set; }
        public decimal Coefficient { get; set; }
        public decimal MinimumWorkingHour { get; set; }
        public string Note { get; set; }
        public int EarlyCheckOut { get; set; }
        public int LatelyCheckIn { get; set; }
        public int? MaxLateCheckInOutMinute { get; set; }
        public int? MinSoonCheckInOutMinute { get; set; }
        public int Status { get; set; }
        public string Type { get; set; }
        public int SortIndex { get; set; }
        public int IsOvertimeShift { get; set; }
        public decimal MealCoefficient { get; set; }
        public string Timezone { get; set; }
        
        // Time configuration
        public int? StartHourId { get; set; }
        public int? StartMinuteId { get; set; }
        public int? EndHourId { get; set; }
        public int? EndMinuteId { get; set; }
        public int? StartCheckInMinuteId { get; set; }
        public int? EndCheckInMinuteId { get; set; }
        public int? StartCheckOutMinuteId { get; set; }
        public int? EndCheckOutMinuteId { get; set; }
        public int? StartCheckInHourId { get; set; }
        public int? EndCheckInHourId { get; set; }
        public int? StartCheckOutHourId { get; set; }
        public int? EndCheckOutHourId { get; set; }
    }

    public class Ins_Shift_Branch_Create_Parameter
    {
        public int ShiftID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public bool IsInsertOne { get; set; }
    }

    public class Ins_ShiftAssignment_Create_Parameter
    {
        public int CompanyID { get; set; }
        public int ShiftID { get; set; }
        public string Title { get; set; }
        public int SortIndex { get; set; }
        public int AutoApprove { get; set; }
        public string Type { get; set; }
        public string PayrollConfigType { get; set; }
        public string AssignmentTypeObj { get; set; }
        public string GenerateTimekeepingTypeObj { get; set; }
    }

    public class Ins_ShiftAssignment_Branch_Create_Parameter
    {
        public int ShiftAssignmentID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public bool IsInsertOne { get; set; }
    }

    public class Ins_ShiftAssignment_Position_Create_Parameter
    {
        public int ShiftAssignmentID { get; set; }
        public int CompanyID { get; set; }
        public int PositionID { get; set; }
        public bool IsInsertOne { get; set; }
    }

    public class Ins_ShiftAssignment_Department_Create_Parameter
    {
        public int ShiftAssignmentID { get; set; }
        public int CompanyID { get; set; }
        public int DepartmentID { get; set; }
        public bool IsInsertOne { get; set; }
    }

    public class Ins_ShiftAssignment_CreateAssignment_Parameter
    {
        public int ShiftAssignmentID { get; set; }
        public int ShiftID { get; set; }
        public int DateOfWeek { get; set; }
        public string Label { get; set; }
    }

    // Result classes đã có sẵn trong EntitiesObject project
    // Các Time result classes
    public class Ins_Time_GetList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? Value { get; set; }
        public int IsHour { get; set; }
    }
} 