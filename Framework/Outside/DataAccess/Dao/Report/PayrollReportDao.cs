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
    public interface IPayrollReportDao : IBaseFactories<DBNull>
    {
        List<Ins_Payroll_ReportByCompanyID_Result> Payroll_ReportByCompanyID(int accountMapID, DateTime dateFrom, DateTime dateTo,out int outResult);
        List<Ins_Payroll_ReportSoonLateByCompanyID_Result> Payroll_ReportSoonLateByCompanyID(int accountMapID, DateTime dateFrom, DateTime dateTo, int reportType, out int outResult);
        List<Ins_Payroll_ReportNotInOutByCompanyID_Result> Payroll_ReportNotInOutByCompanyID(int accountMapID, DateTime dateFrom, DateTime dateTo, int reportType, out int outResult);
        List<Ins_Payroll_User_GetListByCompanyID_Result> Payroll_User_GetListByCompanyID(int companyID, DateTime dateFrom, DateTime dateTo);
    }

    internal class PayrollReportDao : DaoFactories<TanTamEntities, DBNull>, IPayrollReportDao
    {
        public List<Ins_Payroll_ReportByCompanyID_Result> Payroll_ReportByCompanyID(int accountMapID, DateTime dateFrom, DateTime dateTo,out int outResult)
        {
            using (Uow)
            {
                outResult = 0;
                var out_OutResult = new ObjectParameter("TotalAccount", typeof(int));
                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);

                return Uow.Context.Ins_Payroll_ReportByCompanyID(
                    accountMapID,
                    dateFrom,
                    dateTo,
                    out_OutResult
                ).ToList();
            }
        }

        public List<Ins_Payroll_ReportSoonLateByCompanyID_Result> Payroll_ReportSoonLateByCompanyID(int accountMapID, DateTime dateFrom, DateTime dateTo, int reportType, out int outResult)
        {
            using (Uow)
            {
                outResult = 0;
                var out_OutResult = new ObjectParameter("TotalAccount", typeof(int));
                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);

                return Uow.Context.Ins_Payroll_ReportSoonLateByCompanyID(
                    accountMapID,
                    dateFrom,
                    dateTo,
                    reportType,
                    out_OutResult
                ).ToList();
            }
        }

        public List<Ins_Payroll_ReportNotInOutByCompanyID_Result> Payroll_ReportNotInOutByCompanyID(int accountMapID, DateTime dateFrom, DateTime dateTo, int reportType, out int outResult)
        {
            using (Uow)
            {
                outResult = 0;
                var out_OutResult = new ObjectParameter("TotalAccount", typeof(int));
                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);

                return Uow.Context.Ins_Payroll_ReportNotInOutByCompanyID(
                    accountMapID,
                    dateFrom,
                    dateTo,
                    reportType,
                    out_OutResult
                ).ToList();
            }
        }

        public List<Ins_Payroll_User_GetListByCompanyID_Result> Payroll_User_GetListByCompanyID(int companyID, DateTime dateFrom, DateTime dateTo)
        {
            using (Uow)
            {
               
                return Uow.Context.Ins_Payroll_User_GetListByCompanyID(
                    companyID,
                    dateFrom,
                    dateTo
                ).ToList();
            }
        }
    }
}
