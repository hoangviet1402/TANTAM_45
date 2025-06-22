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
        void ShiftAssignment_User_Create(Payroll_User_CreateMultiDayParameter parameter);
    }

    internal class PayrollDao : DaoFactories<TanTamEntities, DBNull>, IPayrollDao
    {
        public void ShiftAssignment_User_Create(Payroll_User_CreateMultiDayParameter parameter)
        {
            using (Uow)
            {
                
                var data = Uow.Context.Ins_Payroll_User_MultiDay(                   );

              
            }
        }
    }
}
