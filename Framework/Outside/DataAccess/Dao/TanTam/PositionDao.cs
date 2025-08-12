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
        List<Ins_CompanyPosition_GetListByCompanyId_Result> GetListByCompanyId(int companyId, int page, int perPage, bool isAll);
        List<Ins_CompanyPosition_GetBranchesById_Result> GetBranchesById(int positionId, int companyId);
        List<Ins_CompanyPosition_GetDepartmentsById_Result> GetDepartmentsById(int positionId, int companyId);
        int UpdatePosition(int positionId, string name, string alias, string code, int companyId, int expYear, string description = null, int? sortIndex = null);
        int CreatePositionBranchRelate(int positionId, int branchId, int companyId);
        int DeletePositionBranchRelate(int positionId, int branchId, int companyId);
        int CreatePositionDepartmentRelate(int positionId, int departmentId, int companyId);
        int DeletePositionDepartmentRelate(int positionId, int departmentId, int companyId);
        int DeletePosition(int positionId, int companyId);
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
        
        public List<Ins_CompanyPosition_GetListByCompanyId_Result> GetListByCompanyId(int companyId, int page, int perPage, bool isAll)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_CompanyPosition_GetListByCompanyId(companyId, page, perPage, isAll);
                return data.ToList();
            }
        }

        public List<Ins_CompanyPosition_GetBranchesById_Result> GetBranchesById(int positionId, int companyId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_CompanyPosition_GetBranchesById(positionId, companyId);
                return data.ToList();
            }
        }

        public List<Ins_CompanyPosition_GetDepartmentsById_Result> GetDepartmentsById(int positionId, int companyId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_CompanyPosition_GetDepartmentsById(positionId, companyId);
                return data.ToList();
            }
        }

        public int UpdatePosition(int positionId, string name, string alias, string code, int companyId, int expYear, string description = null, int? sortIndex = null)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyPosition_Update(positionId, name, alias, code, companyId, expYear, description, sortIndex, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }
        
        // Position-Branch Relationship Methods
        public int CreatePositionBranchRelate(int positionId, int branchId, int companyId)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyPosition_CreateBranchRelate(positionId, branchId, companyId, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

        public int DeletePositionBranchRelate(int positionId, int branchId, int companyId)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyPosition_DeleteBranchRelate(positionId, branchId, companyId, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }
        
        // Position-Department Relationship Methods
        public int CreatePositionDepartmentRelate(int positionId, int departmentId, int companyId)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyPosition_CreateDepartmentRelate(positionId, departmentId, companyId, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

        public int DeletePositionDepartmentRelate(int positionId, int departmentId, int companyId)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyPosition_DeleteDepartmentRelate(positionId, departmentId, companyId, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }
        
        public int DeletePosition(int positionId, int companyId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_CompanyPosition_Delete(positionId, companyId);
                var result = data.FirstOrDefault();
                return result ?? 0;
            }
        }
    }
}
