using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Enum;
using DataAccess;
using DataAccess.Model.Shift;
using EntitiesObject.Entities.TanTamEntities;
using MyUtility.Extensions;

namespace BussinessObject.Bo.Shift
{
    public class PayrollBo : BaseBo<DBNull>
    {
        public PayrollBo()
            : base(DaoFactory.Payroll)
        {
            
        }

        public void ShiftAssignment_User_Create(Payroll_User_CreateMultiDayParameter parameter, DateTime dateFrom, DateTime dateTo)
        {
            DaoFactory.Payroll.ShiftAssignment_User_Create(parameter, dateFrom , dateTo);
        }


        public List<Ins_Payroll_User_GetList_Result> Payroll_User_GetList(int assignmentUserID, int accountMapID, DateTime dateFrom, DateTime dateTo)
        {
            var data  = DaoFactory.Payroll.ShiftAssignment_User_Create(assignmentUserID, accountMapID , dateFrom , dateTo);
            if(assignmentUserID == 0)
            {
                return data.Where(x => x.ShiftType == shift_type_enum.standard_working.Text()).ToList();
            }
            else
            {
                return data;
            }
        }
    }
}
