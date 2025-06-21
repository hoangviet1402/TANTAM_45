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
    }

    internal class PositionDao : DaoFactories<TanTamEntities, DBNull>, IPositionDao
    {
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
    }
}
