using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Auth;
using DataAccess;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BussinessObject.Bo.TanTamBo
{
    public class AuthBo : BaseBo<DBNull>
    {
        public AuthBo()
            : base(DaoFactory.Auth)
        {
        }

        public Ins_Account_Login_Result Login(int accountId, int companyId)
        {
            try
            {
                return DaoFactory.Auth.Login(accountId, companyId);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("AuthBo => Login", ex);
                return new Ins_Account_Login_Result();
            }
        }

        public Ins_Account_Validata_Result Validate(string accountName, bool isUsePhone)
        {
            try
            {
                return DaoFactory.Auth.Validate(accountName, isUsePhone);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("AuthBo => Validate", ex);
                return new Ins_Account_Validata_Result();
            }
        }

        public List<Ins_Account_UpdateFullName_Result> UpdateFullName(string phone, string mail, string FullName, bool IsUsePhone)
        {
            try
            {
                return DaoFactory.Auth.UpdateFullName(phone, mail,  FullName, IsUsePhone);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("AuthBo => UpdateFullName", ex);
                return new List<Ins_Account_UpdateFullName_Result>();
            }
        }

        public void RegisterAccount(string phoneCode, string phone, string email, string fullname, string deviceId, out int accountId, out int companyID, out int employeeAccountMapId)
        {
            accountId = 0;
            companyID = 0;
            employeeAccountMapId = 0;

            try
            {
               
                DaoFactory.Auth.RegisterAccount(phoneCode, phone, email, fullname, deviceId, out accountId, out companyID, out employeeAccountMapId);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("AuthBo => RegisterAccount", ex);
            }
        }

        public int InsertEmployeeToken(int employeeAccountMapId, string jwtID_Hash, string refreshToken_Hash, int lifeTime, string ip, string imie)
        {
            try
            {
                return DaoFactory.Auth.InsertEmployeeToken(employeeAccountMapId, jwtID_Hash, refreshToken_Hash, lifeTime, ip, imie);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("AuthBo => InsertEmployeeToken", ex);
                return 0;
            }
        }

        public int RevokeEmployeeToken(int employeeId, string ip, string imie)
        {
            try
            {

                return DaoFactory.Auth.RevokeEmployeeToken(employeeId, ip, imie);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("AuthBo => RevokeEmployeeToken", ex);
                return 0;
            }
        }

        public int UpdateEmployeeJwtID(int employeeId, string jwtID, string ip, string imie)
        {
            try
            {
                return DaoFactory.Auth.UpdateEmployeeJwtID(employeeId, jwtID, ip, imie);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("AuthBo => UpdateEmployeeJwtID", ex);
                return 0;
            }
        }

        public Ins_Account_GetTokensByEmployeeID_Result GetTokenInfo(int accountId, int companyId)
        {
            try
            {
                return DaoFactory.Auth.GetTokenInfo(accountId, companyId);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("AuthBo => UpdateEmployeeJwtID", ex);
                return new Ins_Account_GetTokensByEmployeeID_Result(); ;
            }
        }

        public int UpdatePass(int accountId, int companyId, string newPass, string oldPass, int needSetPassword)
        {
            try
            {
                return DaoFactory.Auth.UpdatePass(accountId, companyId, newPass, oldPass, needSetPassword);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("AuthBo => UpdatePass", ex);
                return 0;
            }
        }
        public ApiResult<ValidateAccountResponse> ValidateAccountAsync(ValidateAccountRequest request, bool isUsePhone)
        {
            var response = new ApiResult<ValidateAccountResponse>()
            {
                Data = new ValidateAccountResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var accountName = "";
                if (isUsePhone)
                {
                    accountName = $"{request.PhoneCode}{request.Phone}";
                }
                else
                {
                    accountName = request.Mail;
                }

                var authdata =  DaoFactory.Auth.Validate(accountName ?? "", isUsePhone);
                if (authdata == null || authdata.AccountId <= 0)
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = string.Format("Tài khoản {0} không tồn tại.", accountName);
                    response.Data = new ValidateAccountResponse()
                    {
                        Phone = isUsePhone ? request.Phone : "",
                        PhoneCode = isUsePhone ? request.PhoneCode : "+84",
                        Email = isUsePhone == false ? request.Mail : "",
                        Fullname = "",
                        Name = "",
                        Provider = isUsePhone ? "phone" : "mail",
                        IsNoOtpFlow = isUsePhone ? 1 : 0
                    };
                }
                else
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.Success.Text();
                    response.Data = new ValidateAccountResponse()
                    {
                        AccountId = authdata.AccountId
                    };
                }

            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("LoginAsync Exception {0} , EX ", JsonConvert.SerializeObject(request) , ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                response.Data = null;
            }

            return response;
        }

        public ApiResult<AuthResponse> GetDataAlterAsync(int accountId, bool isUsePhone, string accountName, List<string> signinMethods)
        {
            var response = new ApiResult<AuthResponse>()
            {
                Data = new AuthResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };
            var _data = new AuthResponse();
            if (accountId <= 0)
            {
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "Thông tin không được xác thực.";
                return response;
            }

            try
            {
                _data.SigninMethods = signinMethods;

                Ins_Account_GetAllCompany_Result company = new Ins_Account_GetAllCompany_Result()
                {
                    CompanyId = 0,
                    EmployeesInfoId = 0,
                    Role = UserRole.SystemAdmin.Value(),
                };

                var list_companys = DaoFactory.Auth.GetCompanyByAccountId(accountId);

                // tài khoản đã được add 1 công ty
                if (list_companys.Count == 1)
                {
                    company = list_companys.FirstOrDefault();
                    if (company == null) //Chưa tạo doanh nghiệp
                    {
                        response.Code = ResponseResultEnum.CompanyNoData.Value();
                        response.Message = $"Tài khoản chưa được tạo bất kỳ doanh nghiệp nào.";
                        _data.User = new AuthUserResponse()
                        {
                            Id = accountId,
                        };
                        company = new Ins_Account_GetAllCompany_Result()
                        {
                            CompanyId = 0,
                            EmployeesInfoId = 0,
                            Role = UserRole.SystemAdmin.Value(),
                        };
                    }
                    else // doanh nghiệp ok
                    {
                        _data.User = new AuthUserResponse()
                        {
                            Name = company.EmployeesFullName,
                            Id = accountId,
                            ClientRole = company.Role.ToString(),
                            Phone = isUsePhone ? accountName : null,
                            Email = isUsePhone ? null : accountName,
                        };
                        _data.Company = new AuthCompanyResponse()
                        {
                            Name = company.CompanyFullName,
                            ShopUsername = company.CompanyFullName,
                            Id = company.CompanyId,
                            IsNewUser = company.IsNewUser,
                            NeedSetPassword = company.NeedSetPassword,
                            UserId = company.EmployeeAccountMapId,
                            ClientRole = company.Role
                        };
                        response.Data = _data;
                        response.Code = ResponseResultEnum.Success.Value();
                        response.Message = ResponseResultEnum.Success.Text();
                    }
                }
                else if (list_companys.Count > 1)// có nhiều hơn  1 doanh nghiệp
                {

                    _data.ListCompanies = new List<AuthCompaniesResponse>();
                    _data.ListCompanies = list_companys.Select(x => new AuthCompaniesResponse()
                    {
                        ClientRole = x.Role.ToString(),
                        EmployeeName = x.EmployeesFullName,
                        UserId = x.EmployeesInfoId,
                        NeedSetPassword = x.NeedSetPassword,
                        Id = x.CompanyId,
                        Name = x.CompanyFullName,
                        IsNewUser = x.IsNewUser
                    }).ToList();

                    _data.User = new AuthUserResponse()
                    {
                        Id = accountId
                    };
                    response.Data = _data;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = $"Vui lòng chọn doanh nghiệp.";
                }
                else
                {
                    response.Code = ResponseResultEnum.CompanyNoData.Value();
                    response.Message = $"Tài khoản chưa được tạo bất kỳ doanh nghiệp nào.";
                    _data.User = new AuthUserResponse()
                    {
                        Id = accountId,
                    };
                    response.Data = _data;
                    return response;
                }

            }
            catch (Exception ex)
            {
                //LoggerHelper.Error($"LoginAsync Exception", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                response.Data = null;
            }

            return response;
        }


        public ApiResult<Ins_Account_Login_Result> SigninAsync(SigninRequest request, bool isUsePhone, string ip, string imie)
        {
            var response = new ApiResult<Ins_Account_Login_Result>()
            {
                Data = new Ins_Account_Login_Result(),
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var accountName = "";
                var signinMethods = new List<string>();

                var authdata = DaoFactory.Auth.Login(request.UserId ?? 0, request.ShopId ?? 0);

                if (authdata == null || authdata.AccountId <= 0 || authdata.PasswordHash != (string.IsNullOrEmpty(request.Password) ? "" : SecurityCommon.sha256_hash(request.Password)))
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Mật khẩu không chính xác.";
                    response.Data = null;
                    return response;
                }

                if (authdata.IsActive == false)
                {
                    //LoggerHelper.Warning($"LoginAsync email {accountName} này đã bị khóa.");
                    response.Code = ResponseResultEnum.AccountLocked.Value();
                    response.Message = $"Tài khoản {accountName} đã bị khóa.";
                    response.Data = null;
                    return response;
                }

                response.Code = ResponseResultEnum.Success.Value();
                response.Data = GetDataAlterAsync(authdata.AccountId, isUsePhone, accountName, signinMethods);
                
            }
            catch (Exception ex)
            {
                //LoggerHelper.Error($"LoginAsync Exception {JsonConvert.SerializeObject(request)}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                response.Data = null;
            }

            return response;
        }

        public ApiResult<UpdateFullNameSigupResponse> UpdateFullNameSigupAsync(string phoneCode, string phone, string mail, string fullName, bool isUsePhone, string ip, string imie)
        {
            var response = new ApiResult<UpdateFullNameSigupResponse>()
            {
                Data = new UpdateFullNameSigupResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            if (string.IsNullOrEmpty(fullName))
            {
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "Vui lòng nhập tên.";
                return response;
            }
            try
            {
                if (isUsePhone == true && string.IsNullOrEmpty(mail))
                {
                    mail = $"{phone}@mail.com";
                }
                else if (isUsePhone == false && string.IsNullOrEmpty(phone))
                {
                    phone = StringCommon.GenerateUniqueNumber(11);
                }

                var updatedFullNames = DaoFactory.Auth.UpdateFullName(string.Format("{0}{1}", phoneCode, phone), mail, fullName, isUsePhone);
                var accountId = 0;
                var companyId = 0;
                var employeeAccountMapId = 0;
                if (updatedFullNames == null || updatedFullNames.Count() <= 0 || updatedFullNames.FirstOrDefault().AccountID <= 0)
                {
                    DaoFactory.Auth.RegisterAccount(phoneCode, phone, mail, fullName, imie, out accountId,out companyId,out employeeAccountMapId);
                }
                else
                {
                    accountId = updatedFullNames.FirstOrDefault().AccountID ?? 0;
                    companyId = updatedFullNames.FirstOrDefault().CompanyId ?? 0;
                    employeeAccountMapId = updatedFullNames.FirstOrDefault().EmployeeAccountMapID ?? 0;
                }

                response.Data = new UpdateFullNameSigupResponse
                {
                    UserId = accountId,
                    ShopId = companyId
                };
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Cập nhật tên thành công.";
            }
            catch (Exception ex)
            {
//LoggerHelper.Error($"UpdateFullNameAsync Exception phone {phone} |mail {mail}, fullName {fullName}, isUsePhone {isUsePhone}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
            }

            return response;
        }
    }
}