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
        List<Ins_CompanyDepartment_CreateInAllBranchId_Result> CreateDepartmentInAllBranches(string name, int companyId, string alias, string code);
        List<Ins_CompanyDepartment_GetAll_Result> GetAllDepartments(int companyId);
    }

    internal class DepartmentDao : DaoFactories<TanTamEntities, DBNull>, IDepartmentDao
    {
        public List<Ins_CompanyDepartment_CreateInAllBranchId_Result> CreateDepartmentInAllBranches(string name, int companyId, string alias, string code)
        {
            using (Uow)
            {
                return Uow.Context.Ins_CompanyDepartment_CreateInAllBranchId(name, alias, code, companyId).ToList();
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
