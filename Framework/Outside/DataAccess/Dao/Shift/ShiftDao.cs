using DataAccess.EF;
using DataAccess.Interface;
using DataAccess.Model.Shift;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccess.Dao.TanTamDao
{
    public interface IShiftDao : IBaseFactories<DBNull>
    {
        List<Ins_Time_GetList_Result> GetTimes(string lang);
        int Shift_Create_Info(Ins_Shift_Create_Parameter parameter);
        List<Ins_Shift_Branch_Create_Result> Shift_Branch_Create(Ins_Shift_Branch_Create_Parameter parameter);
        List<Ins_Shift_CreateTimeInOutConfig_Result> Shift_Create_TimeInOutConfig(Ins_Shift_Create_Parameter parameter);
        int CreateShiftAssignmentUserWorkingDaySingle(int employeeId, int shiftId, DateTime workingDay, bool allowReactivation = true);
        List<Ins_ShiftAssignment_User_WorkingDay_GetEmployee_Single_Result> GetEmployeeSingle(int companyId, int? employeeId);
        List<Ins_ShiftAssignment_User_WorkingDay_GetShifts_Result> GetShifts(int companyId);
        List<Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result> GetShiftAssignmentUserWorkingDaySummary(int companyId, DateTime? startDate, DateTime? endDate, int employeeId, int? month, int? year);
        Ins_ShiftAssignment_User_WorkingDay_RejectShift_Result RejectShift(int id, int userId);
        Ins_ShiftAssignment_User_WorkingDay_RegisterShift_Result RegisterShift(int shiftId, DateTime workingDay, int userId);
        List<Ins_Shift_GetListByUser_Result> GetShiftListByUser(int userId, DateTime? workingDay, bool isAll = true);
        List<Ins_Assignment_GetDateOfWeekByShiftId_V2_Result> GetAssignmentDateOfWeekByShiftId(int shiftId);
        List<Ins_ShiftTimeInOutConfig_GetByShiftId_Result> GetShiftTimeConfig(int shiftId);
        List<Ins_HourData_GetAll_Result> GetAllHours();
        List<Ins_MinuteData_GetAll_Result> GetAllMinutes();
        Ins_Report_GetWorkingTotal_V2_Result GetWorkingTotal(int companyId, DateTime? workingDay, int? regionId, int? branchId);
        List<Ins_Report_GetWorkingDetail_V2_Result> GetWorkingDetail(int companyId, DateTime? workingDay, int? regionId, int? branchId);
        Ins_ShiftAssignment_User_WorkingDay_UpdateCheckInOut_Result UpdateCheckInOut(int id, int userId, string checkinTime, string checkoutTime, bool isCheckin, bool isCheckout);
        Ins_ShiftAssignment_User_WorkingDay_UncheckInOut_Result UncheckInOut(int id, int userId, bool isUncheckin, bool isUncheckout, string reason);
        List<Ins_ShiftAssignment_GetListWithShift_Result> GetShiftAssignmentListWithShiftByEmployee(int companyId, int page = 1, int pageSize = 15, string status = "active", int? startHourValue = null, int? endHourValue = null, string keyword = null);
        List<Ins_ShiftAssignment_GetDetailWithShift_Result> GetShiftAssignmentDetailWithShiftByEmployee(int shiftAssignmentId, int companyId);
        List<Ins_ShiftAssignment_GetAssignmentsByShiftAssignmentId_Result> GetAssignmentsByShiftAssignmentId(int shiftAssignmentId, int companyId);
        List<Ins_ShiftAssignment_GetBranchesByShiftAssignmentId_Result> GetBranchesByShiftAssignmentId(int shiftAssignmentId, int companyId);
        List<Ins_ShiftAssignment_GetDepartmentsByShiftAssignmentId_Result> GetDepartmentsByShiftAssignmentId(int shiftAssignmentId, int companyId);
        List<Ins_ShiftAssignment_GetPositionsByShiftAssignmentId_Result> GetPositionsByShiftAssignmentId(int shiftAssignmentId, int companyId);
        List<Ins_ShiftAssignment_UpdateMain_Result> UpdateShiftAssignmentMain(int shiftAssignmentId, int companyId, int updatedBy, string title = null, int? autoApprove = null, int? approverId = null, string payrollConfigType = null, string assignmentType = null, string generateTimekeepingType = null, int? assignmentSortIndex = null, decimal? assignmentMealCoefficient = null);
        List<Ins_Shift_UpdateMain_Result> UpdateShiftMain(int shiftId, int companyId, int updatedBy, string shiftName = null, string shiftKey = null, decimal? coefficient = null, decimal? minimumWorkingHour = null, string note = null, int? earlyCheckOut = null, int? latelyCheckIn = null, int? maxLateCheckInOutMinute = null, int? minSoonCheckInOutMinute = null, int? status = null, string type = null, int? sortIndex = null, int? isOvertimeShift = null, decimal? mealCoefficient = null, string timezone = null);
        bool ClearShiftBranches(int shiftId);
        int DeleteShiftAssignment(int shiftAssignmentId, int companyId);
        List<Ins_Shift_GetSimple_Result> Shift_GetSimple(int companyId, int brandid);

        List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result> Shift_TimePenaltyRule_SelectByShiftId(int shiftId);
        void Shift_TimePenaltyRule_Createdefault(int shiftId);
    }

    internal class ShiftDao : DaoFactories<TanTamEntities, DBNull>, IShiftDao
    {
        public List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result> Shift_TimePenaltyRule_SelectByShiftId(int shiftId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Shift_TimePenaltyRule_SelectByShiftId(shiftId);
                return data.ToList();
            }
        }

        public void Shift_TimePenaltyRule_Createdefault(int shiftId)
        {
            using (Uow)
            {
                Uow.Context.Ins_Shift_TimePenaltyRule_Createdefault(shiftId);
            }
        }

        public List<Ins_Shift_GetSimple_Result> Shift_GetSimple(int companyId, int brandid)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Shift_GetSimple(companyId , brandid,-1,-1);
                return data.ToList();
            }
        }
        public  List<Ins_Time_GetList_Result> GetTimes(string lang)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Time_GetList(lang);
                return data.ToList();
            }
        }

        public  int Shift_Create_Info(Ins_Shift_Create_Parameter parameter)
        {
            using (Uow)
            {
                var outResult = 0;

                var out_OutResult = new ObjectParameter("ShiftId", typeof(int));

                var data = Uow.Context.Ins_Shift_Create(
                    parameter.CompanyID,
                    parameter.Name,
                    parameter.NameNosign,
                    parameter.ShiftKey,
                    parameter.Coefficient,
                    parameter.MinimumWorkingHour,
                    parameter.Note,
                    parameter.EarlyCheckOut,
                    parameter.LatelyCheckIn,
                    parameter.MaxLateCheckInOutMinute,
                    parameter.MinSoonCheckInOutMinute,
                    parameter.Status,
                    parameter.Type,
                    parameter.SortIndex,
                    parameter.IsOvertimeShift,
                    parameter.MealCoefficient,
                    parameter.Timezone,
                    out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

        public  List<Ins_Shift_CreateTimeInOutConfig_Result> Shift_Create_TimeInOutConfig(Ins_Shift_Create_Parameter parameter)
        {
            using (Uow)
            {
                
                var data = Uow.Context.Ins_Shift_CreateTimeInOutConfig(
                    parameter.ShiftId,
                    parameter.StartHourId,
                    parameter.StartMinuteId,
                    parameter.EndHourId,
                    parameter.EndMinuteId,
                    parameter.StartCheckInMinuteId,
                    parameter.EndCheckInMinuteId,
                    parameter.StartCheckOutMinuteId,
                    parameter.EndCheckOutMinuteId,
                    parameter.StartCheckInHourId,
                    parameter.EndCheckInHourId,
                    parameter.StartCheckOutHourId,
                    parameter.EndCheckOutHourId,
                    parameter.MaxLateCheckInOutMinute,
                    parameter.MinSoonCheckInOutMinute,
                    "vi");
                return data.ToList();
            }           
        }

        public  List<Ins_Shift_Branch_Create_Result> Shift_Branch_Create(Ins_Shift_Branch_Create_Parameter parameter)
        {
            using (Uow)
            {
                var out_OutResult = new ObjectParameter("AssignmentID", typeof(int));
                var data = Uow.Context.Ins_Shift_Branch_Create(
                    parameter.ShiftID,
                    parameter.CompanyID,
                    parameter.BranchID,
                    parameter.IsInsertOne);

                return data.ToList();
            }          
        }

        public int CreateShiftAssignmentUserWorkingDaySingle(int employeeId, int shiftId, DateTime workingDay, bool allowReactivation = true)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_ShiftAssignment_User_WorkingDay_CreateSingle(employeeId, shiftId, workingDay, allowReactivation);
                return result.FirstOrDefault() ?? 0;
            }
        }

        public List<Ins_ShiftAssignment_User_WorkingDay_GetEmployee_Single_Result> GetEmployeeSingle(int companyId, int? employeeId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_User_WorkingDay_GetEmployee_Single(companyId, employeeId).ToList();
            }
        }

        public List<Ins_ShiftAssignment_User_WorkingDay_GetShifts_Result> GetShifts(int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_User_WorkingDay_GetShifts(companyId).ToList();
            }
        }

        public List<Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result> GetShiftAssignmentUserWorkingDaySummary(int companyId, DateTime? startDate, DateTime? endDate, int employeeId, int? month, int? year)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single(companyId, startDate, endDate, employeeId, month, year).ToList();
            }
        }

        public Ins_ShiftAssignment_User_WorkingDay_RejectShift_Result RejectShift(int id, int userId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_User_WorkingDay_RejectShift(id, userId).FirstOrDefault();
            }
        }

        public Ins_ShiftAssignment_User_WorkingDay_RegisterShift_Result RegisterShift(int shiftId, DateTime workingDay, int userId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_User_WorkingDay_RegisterShift(shiftId, workingDay, userId).FirstOrDefault();
            }
        }

        public List<Ins_Shift_GetListByUser_Result> GetShiftListByUser(int userId, DateTime? workingDay, bool isAll = true)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Shift_GetListByUser(userId, workingDay, isAll).ToList();
            }
        }

        public List<Ins_Assignment_GetDateOfWeekByShiftId_V2_Result> GetAssignmentDateOfWeekByShiftId(int shiftId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Assignment_GetDateOfWeekByShiftId_V2(shiftId).ToList();
            }
        }

        public List<Ins_ShiftTimeInOutConfig_GetByShiftId_Result> GetShiftTimeConfig(int shiftId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftTimeInOutConfig_GetByShiftId(shiftId).ToList();
            }
        }

        public List<Ins_HourData_GetAll_Result> GetAllHours()
        {
            using (Uow)
            {
                return Uow.Context.Ins_HourData_GetAll().ToList();
            }
        }

        public List<Ins_MinuteData_GetAll_Result> GetAllMinutes()
        {
            using (Uow)
            {
                return Uow.Context.Ins_MinuteData_GetAll().ToList();
            }
        }

        public Ins_Report_GetWorkingTotal_V2_Result GetWorkingTotal(int companyId, DateTime? workingDay, int? regionId, int? branchId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Report_GetWorkingTotal_V2(companyId, workingDay, regionId, branchId).FirstOrDefault();
            }
        }

        public List<Ins_Report_GetWorkingDetail_V2_Result> GetWorkingDetail(int companyId, DateTime? workingDay, int? regionId, int? branchId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Report_GetWorkingDetail_V2(companyId, workingDay, regionId, branchId).ToList();
            }
        }

        public Ins_ShiftAssignment_User_WorkingDay_UpdateCheckInOut_Result UpdateCheckInOut(int id, int userId, string checkinTime, string checkoutTime, bool isCheckin, bool isCheckout)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_User_WorkingDay_UpdateCheckInOut(id, userId, checkinTime, checkoutTime, isCheckin, isCheckout).FirstOrDefault();
            }
        }

        public Ins_ShiftAssignment_User_WorkingDay_UncheckInOut_Result UncheckInOut(int id, int userId, bool isUncheckin, bool isUncheckout, string reason)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_User_WorkingDay_UncheckInOut(id, userId, isUncheckin, isUncheckout, reason).FirstOrDefault();
            }
        }

        public List<Ins_ShiftAssignment_GetListWithShift_Result> GetShiftAssignmentListWithShiftByEmployee(int companyId, int page = 1, int pageSize = 15, string status = "active", int? startHourValue = null, int? endHourValue = null, string keyword = null)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_GetListWithShift(companyId, page, pageSize, status, startHourValue, endHourValue, keyword).ToList();
            }
        }

        public List<Ins_ShiftAssignment_GetDetailWithShift_Result> GetShiftAssignmentDetailWithShiftByEmployee(int shiftAssignmentId, int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_GetDetailWithShift(shiftAssignmentId, companyId).ToList();
            }
        }

        public List<Ins_ShiftAssignment_GetAssignmentsByShiftAssignmentId_Result> GetAssignmentsByShiftAssignmentId(int shiftAssignmentId, int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_GetAssignmentsByShiftAssignmentId(shiftAssignmentId, companyId).ToList();
            }
        }

        public List<Ins_ShiftAssignment_GetBranchesByShiftAssignmentId_Result> GetBranchesByShiftAssignmentId(int shiftAssignmentId, int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_GetBranchesByShiftAssignmentId(shiftAssignmentId, companyId).ToList();
            }
        }

        public List<Ins_ShiftAssignment_GetDepartmentsByShiftAssignmentId_Result> GetDepartmentsByShiftAssignmentId(int shiftAssignmentId, int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_GetDepartmentsByShiftAssignmentId(shiftAssignmentId, companyId).ToList();
            }
        }

        public List<Ins_ShiftAssignment_GetPositionsByShiftAssignmentId_Result> GetPositionsByShiftAssignmentId(int shiftAssignmentId, int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_GetPositionsByShiftAssignmentId(shiftAssignmentId, companyId).ToList();
            }
        }

        public List<Ins_ShiftAssignment_UpdateMain_Result> UpdateShiftAssignmentMain(
            int shiftAssignmentId,
            int companyId,
            int updatedBy,
            string title = null,
            int? autoApprove = null,
            int? approverId = null,
            string payrollConfigType = null,
            string assignmentType = null,
            string generateTimekeepingType = null,
            int? assignmentSortIndex = null,
            decimal? assignmentMealCoefficient = null)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_ShiftAssignment_UpdateMain(
                    shiftAssignmentId,
                    companyId,
                    updatedBy,
                    title,
                    autoApprove,
                    approverId,
                    payrollConfigType,
                    assignmentType,
                    generateTimekeepingType,
                    assignmentSortIndex,
                    assignmentMealCoefficient
                );

                return result?.ToList() ?? new List<Ins_ShiftAssignment_UpdateMain_Result>();
            }
        }

        public List<Ins_Shift_UpdateMain_Result> UpdateShiftMain(
            int shiftId,
            int companyId,
            int updatedBy,
            string shiftName = null,
            string shiftKey = null,
            decimal? coefficient = null,
            decimal? minimumWorkingHour = null,
            string note = null,
            int? earlyCheckOut = null,
            int? latelyCheckIn = null,
            int? maxLateCheckInOutMinute = null,
            int? minSoonCheckInOutMinute = null,
            int? status = null,
            string type = null,
            int? sortIndex = null,
            int? isOvertimeShift = null,
            decimal? mealCoefficient = null,
            string timezone = null)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Shift_UpdateMain(
                    shiftId,
                    companyId,
                    updatedBy,
                    shiftName,
                    shiftKey,
                    coefficient,
                    minimumWorkingHour,
                    note,
                    earlyCheckOut,
                    latelyCheckIn,
                    maxLateCheckInOutMinute,
                    minSoonCheckInOutMinute,
                    status,
                    type,
                    sortIndex,
                    isOvertimeShift,
                    mealCoefficient,
                    timezone
                );

                return result?.ToList() ?? new List<Ins_Shift_UpdateMain_Result>();
            }
        }



        public bool ClearShiftBranches(int shiftId)
        {
            using (Uow)
            {
                try
                {
                    var result = Uow.Context.Ins_Shift_ClearBranches(shiftId);
                    return result.Any();
                }
                catch
                {
                    return false;
                }
            }
        }

        public int DeleteShiftAssignment(int shiftAssignmentId, int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_DeleteShiftAndAssignment(shiftAssignmentId, companyId).FirstOrDefault().GetValueOrDefault();
            }
        }
    }
}
