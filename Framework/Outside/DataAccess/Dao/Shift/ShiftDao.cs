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
        List<Ins_ShiftAssignment_User_WorkingDay_GetEmployees_Result> GetEmployees(int companyId, string employeeIds);
        List<Ins_ShiftAssignment_User_WorkingDay_GetShifts_Result> GetShifts(int companyId);
        List<Ins_ShiftAssignment_User_WorkingDay_GetSummary_Result> GetShiftAssignmentUserWorkingDaySummary(int companyId, DateTime? startDate, DateTime? endDate, string employeeIds, int? month, int? year);
        Ins_ShiftAssignment_User_WorkingDay_RejectShift_Result RejectShift(int id, int userId);
        Ins_ShiftAssignment_User_WorkingDay_RegisterShift_Result RegisterShift(int shiftId, DateTime workingDay, int userId);
        List<Ins_Shift_GetListByUser_Result> GetShiftListByUser(int userId, DateTime? workingDay);
        List<Ins_Assignment_GetDateOfWeekByShiftIds_Result> GetAssignmentDateOfWeekByShiftIds(string shiftIds);
        List<Ins_ShiftTimeInOutConfig_GetByShiftId_Result> GetShiftTimeConfig(int shiftId);
        List<Ins_HourData_GetAll_Result> GetAllHours();
        List<Ins_MinuteData_GetAll_Result> GetAllMinutes();
        Ins_ShiftAssignment_User_WorkingDay_UpdateCheckInOut_Result UpdateCheckInOut(int id, int userId, string checkinTime, string checkoutTime, bool isCheckin, bool isCheckout);
        Ins_ShiftAssignment_User_WorkingDay_UncheckInOut_Result UncheckInOut(int id, int userId, bool isUncheckin, bool isUncheckout, string reason);
    }

    internal class ShiftDao : DaoFactories<TanTamEntities, DBNull>, IShiftDao
    {
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

        public List<Ins_ShiftAssignment_User_WorkingDay_GetEmployees_Result> GetEmployees(int companyId, string employeeIds)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_User_WorkingDay_GetEmployees(companyId, employeeIds).ToList();
            }
        }

        public List<Ins_ShiftAssignment_User_WorkingDay_GetShifts_Result> GetShifts(int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_User_WorkingDay_GetShifts(companyId).ToList();
            }
        }

        public List<Ins_ShiftAssignment_User_WorkingDay_GetSummary_Result> GetShiftAssignmentUserWorkingDaySummary(int companyId, DateTime? startDate, DateTime? endDate, string employeeIds, int? month, int? year)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_User_WorkingDay_GetSummary(companyId, startDate, endDate, employeeIds, month, year).ToList();
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

        public List<Ins_Shift_GetListByUser_Result> GetShiftListByUser(int userId, DateTime? workingDay)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Shift_GetListByUser(userId, workingDay).ToList();
            }
        }

        public List<Ins_Assignment_GetDateOfWeekByShiftIds_Result> GetAssignmentDateOfWeekByShiftIds(string shiftIds)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Assignment_GetDateOfWeekByShiftIds(shiftIds).ToList();
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
    }
}
