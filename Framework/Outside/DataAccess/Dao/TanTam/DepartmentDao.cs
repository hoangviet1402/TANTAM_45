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
        List<Ins_CompanyDepartment_GetListByCompanyId_Result> GetListByCompanyId(int companyId, int page, int perPage, bool isAll);
        List<Ins_CompanyDepartment_GetListBranchById_Result> GetListBranchById(int departmentId, int companyId);
        int UpdateDepartment(int departmentId, string departmentName, int? isOnboarding, string alias, string code, int companyId);
        int CreateRelate(int departmentId, int branchId);
        int DeleteRelate(int departmentId, int branchId);
        int DeleteDepartment(int departmentId, int companyId);
    }

    internal class DepartmentDao : DaoFactories<TanTamEntities, DBNull>, IDepartmentDao
    {
        public List<Ins_CompanyDepartment_GetListByCompanyId_Result> GetListByCompanyId(int companyId, int page, int perPage, bool isAll)
        {
            using (Uow)
            {
                return Uow.Context.Ins_CompanyDepartment_GetListByCompanyId(companyId, page, perPage, isAll).ToList();
            }
        }

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

        public List<Ins_CompanyDepartment_GetListBranchById_Result> GetListBranchById(int departmentId, int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_CompanyDepartment_GetListBranchById(departmentId, companyId).ToList();
            }
        }

        public int UpdateDepartment(int departmentId, string departmentName, int? isOnboarding, string alias, string code, int companyId)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyDepartment_Update(departmentId, departmentName, isOnboarding, alias, code, companyId, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                {
                    outResult = Convert.ToInt32(out_OutResult.Value);
                }

                return outResult;
            }
        }

        public int CreateRelate(int departmentId, int branchId)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyDepartment_CreateRelate(departmentId, branchId, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                {
                    outResult = Convert.ToInt32(out_OutResult.Value);
                }

                return outResult;
            }
        }

        public int DeleteRelate(int departmentId, int branchId)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyDepartment_DeleteRelate(departmentId, branchId, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                {
                    outResult = Convert.ToInt32(out_OutResult.Value);
                }

                return outResult;
            }
        }

        public int DeleteDepartment(int departmentId, int companyId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_CompanyDepartment_Delete(departmentId, companyId);
                var deletedRows = result.FirstOrDefault() ?? 0;
                return deletedRows;
            }
        }
    }
}
