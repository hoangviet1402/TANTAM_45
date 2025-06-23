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
        List<Ins_Payroll_User_GetList_Result> ShiftAssignment_User_Create(int AssignmentUserID, int AccountMapID, DateTime DateFrom, DateTime DateTo);
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
                    parameter.CheckinType,
                    parameter.CheckouType,
                    parameter.RealWorkingHour,
                    parameter.RealWorkingMinute,
                    parameter.RestStartTimeShort,
                    parameter.RestEndTimeShort,
                    parameter.RealCoefficient, 
                    parameter.Status
                );

              
            }
        }

        public List<Ins_Payroll_User_GetList_Result> ShiftAssignment_User_Create(int AssignmentUserID,int AccountMapID,DateTime DateFrom,DateTime DateTo)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Payroll_User_GetList(
                    AssignmentUserID,
                    AccountMapID,
                    DateFrom,
                    DateTo
                ).ToList();
            }
        }
    }
}
