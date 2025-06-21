using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccess.Dao.TanTam
{
    public interface ICompanyDao : IBaseFactories<DBNull>
    {
        int Employee_AddIntoBranch(int accountId, int branchId, bool isPrimary);
        Ins_Company_GetInforAccount_Result GetAccountInfo(int accountId, int companyId);
        Ins_Company_GetInfor_Result GetCompanyInfo(int companyId);
        List<Ins_Company_GetSetupStep_Result> GetCompanyGetSetupStep(int companyId, int accountId);
        
        
        int CreatePosition(string name, int departmentId, int companyId);
        
        List<Ins_CompanyPosition_CreateInAllBranchId_Result> CreatePositionInAllBranches(string name, int companyId, string alias, string code, int expYear);
        int UpdateCompanyStep(int companyId, int code);
        List<Ins_Business_GetList_Result> BusinessGetList();
        void UpdateInfoWhenSinup(int accountId, int companyId, string companyName, string alias, float latitude, float longitude, string companyNumberEmploye
                                      , string companyAddress, string email, string hearAbout, string usePurpose, string businesFieldIds);
        
    }

    internal class CompanyDao : DaoFactories<TanTamEntities, DBNull>, ICompanyDao
    {
        public int Employee_AddIntoBranch(int accountId, int branchId, bool isPrimary)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_Employee_AddIntoBranch(accountId, branchId, isPrimary, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }
        public Ins_Company_GetInforAccount_Result GetAccountInfo(int accountId, int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Company_GetInforAccount(accountId, companyId).FirstOrDefault();
            }
        }
        public Ins_Company_GetInfor_Result GetCompanyInfo(int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Company_GetInfor(companyId).FirstOrDefault();
            }
        }
        public List<Ins_Company_GetSetupStep_Result> GetCompanyGetSetupStep(int companyId, int accountId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Company_GetSetupStep(companyId).ToList();
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
        public int UpdateCompanyStep(int companyId, int code)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_ComPany_UpdateSetupStep(companyId, code, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }           
        }
        public List<Ins_Business_GetList_Result> BusinessGetList()
        {
            using (Uow)
            {
                return Uow.Context.Ins_Business_GetList().ToList();
            }
        }
        public void UpdateInfoWhenSinup(int accountId, int companyId, string companyName, string alias, float latitude, float longitude, string companyNumberEmploye
                                      , string companyAddress, string email, string hearAbout, string usePurpose, string businesFieldIds)
        {
            using (Uow)
            {
                Uow.Context.Ins_Company_UpdateInfoWhenSinup(accountId, companyId, companyName, alias, latitude, longitude, companyNumberEmploye
                                      , companyAddress, email, hearAbout, usePurpose, businesFieldIds);
            }
        }
    }
}
