using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Linq;

namespace DataAccess.Dao.TanTamDao
{
    public interface IReportDao : IBaseFactories<DBNull>
    {
        Ins_Report_GetEmployeesGrowth_V2_Result GetEmployeesGrowth(int companyId, int daysAgo, int? regionId, int? branchId);
        Ins_Report_GetWorkingTimeStatistics_V2_Result GetWorkingTimeStatistics(int companyId, int daysAgo, int? regionId, int? branchId);
        Ins_Report_GetDashboardDevices_Result GetDashboardDevices(int companyId, DateTime? workingDay = null);
    }

    internal class ReportDao : DaoFactories<TanTamEntities, DBNull>, IReportDao
    {
        public Ins_Report_GetEmployeesGrowth_V2_Result GetEmployeesGrowth(int companyId, int daysAgo, int? regionId, int? branchId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Report_GetEmployeesGrowth_V2(companyId, daysAgo, regionId, branchId);
                return result?.FirstOrDefault();
            }
        }

        public Ins_Report_GetWorkingTimeStatistics_V2_Result GetWorkingTimeStatistics(int companyId, int daysAgo, int? regionId, int? branchId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Report_GetWorkingTimeStatistics_V2(companyId, daysAgo, regionId, branchId);
                return result?.FirstOrDefault();
            }
        }

        public Ins_Report_GetDashboardDevices_Result GetDashboardDevices(int companyId, DateTime? workingDay = null)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Report_GetDashboardDevices(companyId, workingDay);
                return result?.FirstOrDefault();
            }
        }
    }
} 