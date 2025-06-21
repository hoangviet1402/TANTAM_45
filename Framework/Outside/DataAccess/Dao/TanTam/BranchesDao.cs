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
        int CreateBranche(string Name, string Address, string RegionId, int IsOnboarding, float Latitude, float Longitude, int companyId, string Alias, string Code);
        List<Ins_CompanyBranch_GetAllByCompany_Result> GetAllBranchs(int companyId, out int total);
    }

    internal class BranchesDao : DaoFactories<TanTamEntities, DBNull>, IBranchesDao
    {
        public int CreateBranche(string Name, string Address, string RegionId, int IsOnboarding, float Latitude, float Longitude, int companyId, string Alias, string Code)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_CompanyBranch_Create(Name, Address, RegionId, IsOnboarding, Latitude, Longitude, companyId, Alias, Code, out_OutResult);

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
    }
}
