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

namespace DataAccess.Dao.Report
{
    public interface IEmployeeReportDao : IBaseFactories<DBNull>
    {
        List<Ins_EmployeeAccountMap_ReportByCompanyID_Result> EmployeeAccountMap_ReportByCompanyID(int accountMapID, DateTime dateFrom, DateTime dateTo,out int outResult);
    }

    internal class EmployeeReportDao : DaoFactories<TanTamEntities, DBNull>, IEmployeeReportDao
    {
        public List<Ins_EmployeeAccountMap_ReportByCompanyID_Result> EmployeeAccountMap_ReportByCompanyID(int accountMapID, DateTime dateFrom, DateTime dateTo,out int outResult)
        {
            using (Uow)
            {
                outResult = 0;
                var out_OutResult = new ObjectParameter("TotalAccount", typeof(int));
               

                var data =  Uow.Context.Ins_EmployeeAccountMap_ReportByCompanyID(
                    accountMapID,
                    dateFrom,
                    dateTo,
                    out_OutResult
                ).ToList();

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);

                return data;
            }
        }
    }
}
