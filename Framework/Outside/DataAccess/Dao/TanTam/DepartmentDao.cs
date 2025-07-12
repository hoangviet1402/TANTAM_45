using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccess.Dao.TanTam
{
    public interface IDepartmentDao : IBaseFactories<DBNull>
    {
        int CreateDepartment(string name, int branchId, int companyId);
        List<Ins_CompanyDepartment_CreateInAllBranchId_Result> CreateDepartmentInAllBranches(string name, int companyId, string alias, string code, int isOnboarding);
        int CreateDepartmentInAllBranches_Simple(string name, int companyId, int branchID, string alias, string code, int isOnboarding);
        List<Ins_CompanyDepartment_GetAll_Result> GetAllDepartments(int companyId);
        List<Ins_CompanyDepartment_SelectByBrandId_Result> GetAllDepartmentsByBranch(int companyId, int branchID);
        List<Ins_EmployeeDepartmentMap_GetCompanyId_Result> EmployeeDepartmentMap_GetCompanyId(int companyId);
    }

    internal class DepartmentDao : DaoFactories<TanTamEntities, DBNull>, IDepartmentDao
    {
        public List<Ins_EmployeeDepartmentMap_GetCompanyId_Result> EmployeeDepartmentMap_GetCompanyId(int companyId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_EmployeeDepartmentMap_GetCompanyId(companyId);
                return data.ToList();
            }
        }

        public List<Ins_CompanyDepartment_SelectByBrandId_Result> GetAllDepartmentsByBranch(int companyId, int branchID)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_CompanyDepartment_SelectByBrandId(companyId, branchID);
                return data.ToList();
            }
        }

        public List<Ins_CompanyDepartment_CreateInAllBranchId_Result> CreateDepartmentInAllBranches(string name, int companyId, string alias, string code, int isOnboarding)
        {
            using (Uow)
            {
                return Uow.Context.Ins_CompanyDepartment_CreateInAllBranchId(name, isOnboarding , alias, code, companyId).ToList();
            }
        }

        public int CreateDepartmentInAllBranches_Simple(string name, int companyId, int branchID, string alias, string code, int isOnboarding)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("CompanyDepartment", typeof(int));
                var data =  Uow.Context.Ins_CompanyDepartment_CreateSimple(name, isOnboarding, alias, code, companyId, branchID, out_OutResult);
                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }
        public int CreateDepartment(string name, int branchId, int companyId)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyDepartment_Create(name, branchId, companyId, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }
        public List<Ins_CompanyDepartment_GetAll_Result> GetAllDepartments(int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_CompanyDepartment_GetAll(companyId).ToList();
            }
        }
    }
}
