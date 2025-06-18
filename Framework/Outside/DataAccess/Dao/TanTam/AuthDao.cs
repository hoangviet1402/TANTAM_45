using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
namespace DataAccess.Dao.TanTamDao
{
    /// <summary>
    ///     Sao khi dang ky thanh cong, lan dau tien dang nhan se duoc tang x Xu
    /// </summary>
    public interface IAuthDao : IBaseFactories<DBNull>
    {
        Ins_Account_Login_Result CheckAccountIDExists(int accountId, int companyId);
        Ins_Account_Validata_Result Validate(string accountName, bool isUsePhone);
        List<Ins_Account_UpdateFullName_Result> UpdateFullName(string phone, string mail, string FullName, bool IsUsePhone);
        void RegisterAccount(string phoneCode, string phone, string email, string fullname, string deviceId, out int accountId, out int companyID, out int employeeAccountMapId);
        List<Ins_Account_GetAllCompany_Result> GetCompanyByAccountId(int accountId);
        int InsertEmployeeToken(int employeeAccountMapId, string jwtID_Hash, string refreshToken_Hash, int lifeTime, string ip, string imie);
        int RevokeEmployeeToken(int employeeId, string ip, string imie);
        int UpdateEmployeeJwtID(int employeeId, string jwtID, string ip, string imie);
        Ins_Account_GetTokensByEmployeeID_Result GetTokenInfo(int accountId, int companyId);
        int UpdatePass(int accountId, int companyId, string newPass, string oldPass, int needSetPassword);
    }

    internal class AuthDao : DaoFactories<TanTamEntities, DBNull>, IAuthDao
    {
        public void RegisterAccount(string phoneCode, string phone, string email, string fullname, string deviceId, out int accountId, out int companyID, out int employeeAccountMapId)
        {
            using (Uow)
            {
                accountId = 0;
                companyID = 0;
                employeeAccountMapId = 0;
                var out_accountId = new ObjectParameter("AccountId", typeof(int));
                var out_companyID = new ObjectParameter("CompanyID", typeof(int));
                var out_employeeAccountMapId = new ObjectParameter("EmployeeAccountMapId", typeof(int));
                Uow.Context.Ins_Account_Register(phone, phoneCode, email, fullname, deviceId, out_accountId, out_companyID, out_employeeAccountMapId);

                if (out_accountId != null && out_accountId.Value != null)
                    int.TryParse(out_accountId.Value.ToString(), out accountId);

                if (out_companyID != null && out_companyID.Value != null)
                    int.TryParse(out_companyID.Value.ToString(), out companyID);

                if (out_employeeAccountMapId != null && out_employeeAccountMapId.Value != null)
                    int.TryParse(out_employeeAccountMapId.Value.ToString(), out employeeAccountMapId);
            }          
        }

        public Ins_Account_Login_Result CheckAccountIDExists(int accountId, int companyId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Account_Login(accountId, companyId);                
                return result.FirstOrDefault();
            }           
        }

        public Ins_Account_Validata_Result Validate(string accountName, bool isUsePhone)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Account_Validata(accountName, isUsePhone);
                return data.FirstOrDefault();
            }
        }

        public int UpdatePass(int accountId, int companyId, string newPass, string oldPass, int needSetPassword)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_Account_UpdatePass(accountId, companyId, newPass, oldPass, needSetPassword, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }           
        }

        public List<Ins_Account_GetAllCompany_Result> GetCompanyByAccountId(int accountId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Account_GetAllCompany(accountId);
                return result.ToList();
            }         
        }

        public int InsertEmployeeToken(int employeeAccountMapId, string jwtID_Hash, string refreshToken_Hash, int lifeTime, string ip, string imie)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_Account_InsertTokens(employeeAccountMapId, jwtID_Hash, refreshToken_Hash, lifeTime, ip, imie, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }          
        }

        public int RevokeEmployeeToken(int tokenId, string ip, string imie)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_Account_RevokeToken(tokenId, ip, imie, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

        public int UpdateEmployeeJwtID(int tokenId, string jwtID_Hash, string ip, string imie)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_Account_UpdateToken_JwtID(tokenId, jwtID_Hash, ip, imie, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }           
        }

        public Ins_Account_GetTokensByEmployeeID_Result GetTokenInfo(int accountId, int companyId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Account_GetTokensByEmployeeID(accountId, companyId);
                return result.FirstOrDefault();
            }         
        }

        public List<Ins_Account_UpdateFullName_Result> UpdateFullName(string phone, string mail, string FullName, bool IsUsePhone)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Account_UpdateFullName(phone, mail, FullName, IsUsePhone);
                return data.ToList();
            }
            
        }
    }
}