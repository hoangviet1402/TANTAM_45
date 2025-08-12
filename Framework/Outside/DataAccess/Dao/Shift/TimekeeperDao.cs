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
    public interface ITimekeeperDao : IBaseFactories<DBNull>
    {
        List<Ins_Timekeeper_log_User_GetLog_OneDay_Result> Timekeeper_log_User_GetLog_OneDay(int accountMapID, DateTime dateFrom);
        int? Timekeeper_log_User_Insert(Timekeeper_log_User_Insert_parameter parameter);
        List<Ins_Timekeeper_log_GetListByAccountMapID_Result> Timekeeper_log_GetListByAccountMapID(int companyID,int accountMapID, DateTime dateFrom, DateTime dateTo);
        List<Ins_Timekeeper_log_GetListByAccountMapID_ByPayrollUserID_Result> Timekeeper_log_GetListByAccountMapID_ByPayrollUserID(int companyID,int accountMapID, int PayrollUserI);
        List<Ins_Timekeeper_log_GetListByAccountMapID_Simple_Result> GetListByAccountMapID_Simple(int accountMapID, DateTime dateFrom, DateTime dateTo);
    }

    internal class TimekeeperDao : DaoFactories<TanTamEntities, DBNull>, ITimekeeperDao
    {
        public List<Ins_Timekeeper_log_GetListByAccountMapID_Simple_Result> GetListByAccountMapID_Simple(int accountMapID, DateTime dateFrom, DateTime dateTo)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Timekeeper_log_GetListByAccountMapID_Simple(
                    accountMapID,
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
                    parameter.Accuracy,
                    parameter.Altitude,
                    parameter.AltitudeAccuracy,
                    parameter.Speed,
                    parameter.SpeedAccuracy,
                    parameter.Course,
                    parameter.CourseAccuracy,
                    parameter.Mocked,
                    parameter.Reason,
                    out_OutResult
                );

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }
        public List<Ins_Timekeeper_log_GetListByAccountMapID_Result> Timekeeper_log_GetListByAccountMapID(int companyID, int accountMapID, DateTime dateFrom, DateTime dateTo)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Timekeeper_log_GetListByAccountMapID(
                    companyID,
                    accountMapID,
                    dateFrom,
                    dateTo
                ).ToList();
            }
        }

        public List<Ins_Timekeeper_log_GetListByAccountMapID_ByPayrollUserID_Result> Timekeeper_log_GetListByAccountMapID_ByPayrollUserID(int companyID, int accountMapID, int PayrollUserID)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Timekeeper_log_GetListByAccountMapID_ByPayrollUserID(
                    companyID,
                    accountMapID,
                    PayrollUserID
                ).ToList();
            }
        }
    }
}
