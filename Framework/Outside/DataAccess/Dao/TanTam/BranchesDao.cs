using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccess.Dao.TanTam
{
    public interface IBranchesDao : IBaseFactories<DBNull>
    {
        int CreateBranche(string Name, string Address, int regionId, int IsOnboarding, float Latitude, float Longitude, int companyId, string Alias, string Code);
        List<Ins_CompanyBranch_GetAllByCompany_Result> GetAllBranchs(int companyId, out int total);
        List<Ins_EmployeeBranchMap_GetByEmployeeId_Result> AccountGetAllBranchs(int accountMapId);
        List<Ins_CompanyRegion_GetListByCompany_Result> GetAllRegion(int companyId);
        int CreateCompanyRegion(string regionName, int companyID, string color, string code, int sortIndex, string description, string alias);
        void UpdateCompanyRegion(string regionName, int companyID, string color, string code, int sortIndex, string description, string alias, int idRegion);
        List<Ins_EmployeeBranchMap_GetByBranchId_Result> EmployeeBranchMap_GetByBranchId(int branchid, int companyId, bool isGetAll = true);
        int CreateBranch_CreateRelate(int branchID, int regionId);
    }

    internal class BranchesDao : DaoFactories<TanTamEntities, DBNull>, IBranchesDao
    {
        public int CreateBranche(string Name, string Address, int regionId, int IsOnboarding, float Latitude, float Longitude, int companyId, string Alias, string Code)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyBranch_Create(Name, Address, regionId, IsOnboarding, Latitude, Longitude, companyId, Alias, Code, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

        public List<Ins_CompanyBranch_GetAllByCompany_Result> GetAllBranchs(int companyId, out int total)
        {
            using (Uow)
            {
                total = 0;
                var out_total = new ObjectParameter("Total", typeof(int));

                var data = Uow.Context.Ins_CompanyBranch_GetAllByCompany(companyId, 100000, out_total);

                if (out_total != null && out_total.Value != null)
                    int.TryParse(out_total.Value.ToString(), out total);

                return data.ToList();
            }
        }

        public List<Ins_EmployeeBranchMap_GetByEmployeeId_Result> AccountGetAllBranchs(int accountMapId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_EmployeeBranchMap_GetByEmployeeId(accountMapId);

                return data.ToList();
            }
        }

        public int CreateCompanyRegion(string regionName, int companyID, string color, string code, int sortIndex, string description, string alias)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("RegionID", typeof(int));
                var data = Uow.Context.Ins_CompanyRegion_Create(regionName,companyID, color, code,sortIndex, description, alias, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

        public void UpdateCompanyRegion(string regionName, int companyID, string color, string code, int sortIndex, string description, string alias, int idRegion)
        {
            using (Uow)
            {
                var outResult = 0;
                Uow.Context.Ins_CompanyRegion_Update(regionName, companyID, color, code, sortIndex, description, alias, idRegion);
               
            }
        }

        public List<Ins_CompanyRegion_GetListByCompany_Result> GetAllRegion(int companyId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_CompanyRegion_GetListByCompany(companyId);
              
                return data.ToList();
            }
        }

        public List<Ins_EmployeeBranchMap_GetByBranchId_Result> EmployeeBranchMap_GetByBranchId(int branchid , int companyId, bool isGetAll = true)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_EmployeeBranchMap_GetByBranchId(branchid, companyId, isGetAll);

                return data.ToList();
            }
        }

        public int CreateBranch_CreateRelate(int branchID, int regionId)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyBranch_CreateRelate(branchID, regionId, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }
    }
}
