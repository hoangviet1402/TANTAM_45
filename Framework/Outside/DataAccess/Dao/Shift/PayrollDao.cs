using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using DataAccess.EF;
using DataAccess.Interface;
using DataAccess.Model.Shift;
using EntitiesObject.Entities.TanTamEntities;

namespace DataAccess.Dao.Shift
{
    public interface IPayrollDao : IBaseFactories<DBNull>
    {
        void ShiftAssignment_User_Create(Payroll_User_CreateMultiDayParameter parameter, DateTime dateFrom, DateTime dateTo);
        List<Ins_Payroll_User_GetList_Result> Payroll_User_GetList(int assignmentUserID, int accountMapID, int brandId, DateTime dateFrom, DateTime dateTo);
        List<Ins_Timekeeper_log_User_GetLog_OneDay_Result> Timekeeper_log_User_GetLog_OneDay(int accountMapID, DateTime dateFrom);
        List<Ins_Shift_User_GetStatus_clock_in_out_Result> Shift_User_GetStatus_clock_in_out(int accountMapID, DateTime dateFrom);
        int? Timekeeper_log_User_Insert(Timekeeper_log_User_Insert_parameter parameter);
    }

    internal class PayrollDao : DaoFactories<TanTamEntities, DBNull>, IPayrollDao
    {
        public void ShiftAssignment_User_Create(Payroll_User_CreateMultiDayParameter parameter,DateTime dateFrom,DateTime dateTo)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Payroll_User_MultiDay(
                    parameter.AssignmentUserID, 
                    parameter.AccountMapID, 
                    dateFrom,
                    dateTo, 
                    parameter.StartTime, 
                    parameter.EndTime, 
                    parameter.WeekOfYear,
                    parameter.RealWorkingHour,
                    parameter.RealWorkingMinute,
                    parameter.RestStartTimeShort,
                    parameter.RestEndTimeShort,
                    parameter.RealCoefficient, 
                    parameter.Status
                );

              
            }
        }

        public List<Ins_Payroll_User_GetList_Result> Payroll_User_GetList(int assignmentUserID,int accountMapID, int brandId, DateTime dateFrom,DateTime dateTo)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Payroll_User_GetList(
                    assignmentUserID,
                    accountMapID,
                    brandId,
                    dateFrom,
                    dateTo
                ).ToList();
            }
        }

        public List<Ins_Timekeeper_log_User_GetLog_OneDay_Result> Timekeeper_log_User_GetLog_OneDay(int accountMapID, DateTime dateFrom)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Timekeeper_log_User_GetLog_OneDay(
                    accountMapID,                    
                    dateFrom                   
                ).ToList();
            }
        }

        public List<Ins_Shift_User_GetStatus_clock_in_out_Result> Shift_User_GetStatus_clock_in_out(int accountMapID, DateTime dateFrom)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Shift_User_GetStatus_clock_in_out(
                    accountMapID,
                    dateFrom
                ).ToList();
            }
        }

        public int? Timekeeper_log_User_Insert(Timekeeper_log_User_Insert_parameter parameter)
        {
            using (Uow)
            {
                var outResult = 0;

                var out_OutResult = new ObjectParameter("Timekeeper_logID", typeof(int));

                var data = Uow.Context.Ins_Timekeeper_log_User_Insert(
                    parameter.AccountMapID,
                    parameter.EmployeeShiftID,
                    parameter.LogTime,
                    parameter.ClockType,
                    parameter.CurrentBranchId,
                    parameter.ConnectionType,
                    parameter.TimeKeeperDevice,
                    parameter.Bssid,
                    parameter.Ssid,
                    parameter.Latitude,
                    parameter.Longitude,
                    out_OutResult
                );

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }
    }
}
