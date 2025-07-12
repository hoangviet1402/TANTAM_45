using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccess.Dao.TanTam
{
    public interface IPositionDao : IBaseFactories<DBNull>
    {
        int CreatePosition(string name, int departmentId, int companyId);
        List<Ins_CompanyPosition_CreateInAllBranchId_Result> CreatePositionInAllBranches(string name, int companyId, string alias, string code, int expYear);
        List<Ins_EmployeePositionMap_GetCompanyId_Result> EmployeePositionMap_GetCompanyId(int companyId);
        List<Ins_CompanyPosition_CreateInAllDepartmentId_Result> CreatePositionInAllDepartment(string name, int companyId, string alias, string code, int expYear);
        int CreatePosition_Simple(string name, string alias, string code, int companyId, int expYear);
        int CreatePosition_CreateRelate(int branchID, int positionID, int departmentID, int regionId);
    }

    internal class PositionDao : DaoFactories<TanTamEntities, DBNull>, IPositionDao
    {
        public List<Ins_EmployeePositionMap_GetCompanyId_Result> EmployeePositionMap_GetCompanyId(int companyId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_EmployeePositionMap_GetCompanyId(companyId);
                return data.ToList();
            }
        }

        public int CreatePosition(string name, int departmentId, int companyId)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyPosition_Create(name, departmentId, companyId, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }
        public List<Ins_CompanyPosition_CreateInAllBranchId_Result> CreatePositionInAllBranches(string name, int companyId, string alias, string code, int expYear)
        {
            using (Uow)
            {
                return Uow.Context.Ins_CompanyPosition_CreateInAllBranchId(name, alias, code, companyId, expYear).ToList();
            }
        }

        public List<Ins_CompanyPosition_CreateInAllDepartmentId_Result> CreatePositionInAllDepartment(string name, int companyId, string alias, string code, int expYear)
        {
            using (Uow)
            {
                return Uow.Context.Ins_CompanyPosition_CreateInAllDepartmentId(name, alias, code, companyId, expYear).ToList();
            }
        }

        public int CreatePosition_Simple(string name, string alias, string code, int companyId, int expYear)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("CompanyPositionID", typeof(int));
                var data = Uow.Context.Ins_CompanyPosition_CreateSimple(name, alias, code, companyId, expYear,  out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

        public int CreatePosition_CreateRelate(int branchID, int positionID, int departmentID , int regionId)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyPosition_CreateRelate( branchID, positionID, departmentID , regionId, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }
    }
}
