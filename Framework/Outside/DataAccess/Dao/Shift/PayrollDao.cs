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
        void Payroll_User_Create_MultiDay(Payroll_User_CreateMultiDayParameter parameter, DateTime dateFrom, DateTime dateTo);
        List<Ins_Payroll_User_GetList_Result> Payroll_User_GetList(int assignmentUserID, int accountMapID, int brandId, DateTime dateFrom, DateTime dateTo);
        List<Ins_Payroll_User_GetListByAccountMapID_Result> GetListByAccountMapID(int accountMapID, DateTime dateFrom, DateTime dateTo);
        List<Ins_Shift_User_GetStatus_clock_in_out_Result> Payroll_User_GetStatus_clock_in_out(int accountMapID, DateTime dateFrom, int type);
        List<Ins_Shift_User_GetStatus_clock_in_out_v2_Result> Payroll_User_GetStatus_clock_in_out_v2(int accountMapID, DateTime dateFrom, int type);
        int Payroll_User_UpdateStatus(int payrollID, int accountMapID, int status);
    }

    internal class PayrollDao : DaoFactories<TanTamEntities, DBNull>, IPayrollDao
    {
        public List<Ins_Payroll_User_GetListByAccountMapID_Result> GetListByAccountMapID(int accountMapID, DateTime dateFrom, DateTime dateTo)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Payroll_User_GetListByAccountMapID(
                    accountMapID,
                    dateFrom,
                    dateTo
                ).ToList();
            }
        }
        public int Payroll_User_UpdateStatus(int payrollID,int accountMapID, int status)
        {
            using (Uow)
            {
                int result = 0;
                var outResult = new ObjectParameter("Result", typeof(int));

                Uow.Context.Ins_Payroll_User_UpdateStatus(
                    payrollID,
                    accountMapID,
                    status,
                    outResult
                );

                if (outResult != null && outResult.Value != null)
                    int.TryParse(outResult.Value.ToString(), out result);
                return result;
            }
        }
        public void Payroll_User_Create_MultiDay(Payroll_User_CreateMultiDayParameter parameter,DateTime dateFrom,DateTime dateTo)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Payroll_User_Create_MultiDay(
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
                    parameter.Status,
                    parameter.IsAddPayRollManual
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
        public List<Ins_Shift_User_GetStatus_clock_in_out_Result> Payroll_User_GetStatus_clock_in_out(int accountMapID, DateTime dateFrom, int type)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Shift_User_GetStatus_clock_in_out(
                    accountMapID,
                    dateFrom,
                    type
                ).ToList();
            }
        }

        public List<Ins_Shift_User_GetStatus_clock_in_out_v2_Result> Payroll_User_GetStatus_clock_in_out_v2(int accountMapID, DateTime dateFrom, int type)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Shift_User_GetStatus_clock_in_out_v2(
                    accountMapID,
                    dateFrom,
                    type
                ).ToList();
            }
        }
    }
}
