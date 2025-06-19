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
                return DaoFactory.Auth.CheckAccountIDExists(accountId, companyId);
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

                var authdata = DaoFactory.Auth.CheckAccountIDExists(request.UserId ?? 0, request.ShopId ?? 0);

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

        //public ApiResult<AuthResponse> HandleStageAsync_validate(SigninRequest request, bool isUsePhone, string ip, string userAgent)
        //{
        //    var response = new ApiResult<AuthResponse>()
        //    {
        //        Code = ResponseResultEnum.MaintenanceMode.Value(),
        //        Message = ResponseResultEnum.MaintenanceMode.Text()
        //    };

        //    var resultValidate = BoFactory.Auth.ValidateAccountAsync(new ValidateAccountRequest()
        //    {
        //        PhoneCode = request.PhoneCode,
        //        Phone = request.Phone,
        //        Mail = request.Mail
        //    }, isUsePhone);

            
        //    return resultValidate;
        //}

        //public object HandleStageAsync_signin(SigninRequest request, bool isUsePhone, string ip, string userAgent)
        //{
        //    var response = new ApiResult<AuthResponse>()
        //    {
        //        Code = ResponseResultEnum.MaintenanceMode.Value(),
        //        Message = ResponseResultEnum.MaintenanceMode.Text()
        //    };

        //    switch (request.Stage.ToLower())
        //    {
        //        case "validate":
        //            var resultValidate = BoFactory.Auth.ValidateAccountAsync(new ValidateAccountRequest()
        //            {
        //                PhoneCode = request.PhoneCode,
        //                Phone = request.Phone,
        //                Mail = request.Mail
        //            }, isUsePhone);

        //            if (resultValidate.Code == ResponseResultEnum.Success.Value() &&
        //                resultValidate.Data != null && ((ValidateAccountResponse)resultValidate.Data).AccountId != null)
        //            {
        //                response = BoFactory.Auth.GetDataAlterAsync(
        //                    ((ValidateAccountResponse)resultValidate.Data).AccountId.Value,
        //                    isUsePhone,
        //                    request.PhoneCode + request.Phone,
        //                    new List<string>() { "phone" });
        //                return response;
        //            }
        //            return resultValidate;

        //        case "signin":
        //            var result = BoFactory.Auth.SigninAsync(request, isUsePhone, ip, userAgent);
        //            if (result.Code == ResponseResultEnum.Success.Value())
        //            {
        //                var authdata = (Ins_Account_Login_Result)result.Data;
        //                response.Data = GenerateAuthResponse(
        //                 authdata.AccountId,
        //                 authdata.Id,
        //                 authdata.CompanyId,
        //                 authdata.Role ?? 0,
        //                 ip);
        //            }
        //            return response;

        //        default:
        //            response.Message = "Thông tin không hợp lệ. 999";
        //            return response;
        //    }
        //}

        //private object Signup_Validate(SignupRequest request, bool isUsePhone)
        //{

        //    var response = new ApiResult<AuthResponse>()
        //    {
        //        Code = ResponseResultEnum.InvalidInput.Value(),
        //        Message = ResponseResultEnum.InvalidInput.Text()
        //    };

        //    // Validate input
        //    if (isUsePhone)
        //    {
        //        if (string.IsNullOrEmpty(request.Phone) || string.IsNullOrEmpty(request.PhoneCode))
        //        {
        //            response.Message = "Số điện thoại không được để trống.";
        //            return response;
        //        }
        //        if (!ValidationHelper.IsValidPhone(request.PhoneCode + request.Phone))
        //        {
        //            response.Message = "Số điện thoại không hợp lệ.";
        //            return response;
        //        }
        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(request.Mail))
        //        {
        //            response.Message = "Mail không được để trống.";
        //            return response;
        //        }
        //        if (!ValidationHelper.IsValidEmail(request.Mail))
        //        {
        //            response.Message = "Email không hợp lệ.";
        //            return response;
        //        }
        //    }

        //    if (string.IsNullOrEmpty(request.Stage))
        //    {
        //        response.Message = "Thông tin không hợp lệ.";
        //        return response;
        //    }

        //    var validateRequest = new ValidateAccountRequest()
        //    {
        //        PhoneCode = request.PhoneCode,
        //        Phone = request.Phone,
        //        Mail = request.Mail
        //    };

        //    var ip = WebUitility.GetIpAddressRequest();
        //    var imie = "";

        //    switch ((request.Stage ?? string.Empty).ToLower())
        //    {
        //        case "validate":
        //            var resultValidate = BoFactory.Auth.ValidateAccountAsync(validateRequest, isUsePhone);
        //            if (resultValidate.Code == ResponseResultEnum.Success.Value() && resultValidate.Data != null)
        //            {
        //                var resultValidate_date = (ValidateAccountResponse)resultValidate.Data;
        //                if (resultValidate_date != null && resultValidate_date.AccountId != null && resultValidate_date.AccountId > 0)
        //                {
        //                    response = BoFactory.Auth.GetDataAlterAsync(
        //                        resultValidate_date.AccountId.Value,
        //                        isUsePhone,
        //                        request.PhoneCode + request.Phone,
        //                        new List<string>() { "phone" });
        //                    return response;
        //                }
        //            }
        //            return resultValidate;
        //        case "signup":
        //            var result = BoFactory.Auth.UpdateFullNameSigupAsync(
        //                request.PhoneCode,
        //                request.Phone,
        //                request.Mail ?? string.Empty,
        //                request.Fullname ?? string.Empty,
        //                isUsePhone,
        //                ip,
        //                imie
        //                );
        //            if (request.IsMobileMenu == 1 && result.Data != null && result.Code == ResponseResultEnum.Success.Value())
        //            {
        //                var UpdateFullNameSigup_result = (UpdateFullNameSigupResponse)result.Data;
        //                if (UpdateFullNameSigup_result.UserId != null && UpdateFullNameSigup_result.UserId > 0)
        //                {
        //                    var dataAlter = BoFactory.Auth.GetDataAlterAsync(
        //                            UpdateFullNameSigup_result.UserId ?? 0,
        //                            isUsePhone,
        //                            string.Format("{0}{1}", request.PhoneCode, request.Phone),
        //                            new List<string>() { "phone" });

        //                    if (dataAlter.Code == ResponseResultEnum.Success.Value() && dataAlter.Data != null)
        //                    {
        //                        var dataAlter_result = (AuthResponse)dataAlter.Data;
        //                        if (
        //                            (dataAlter_result.Company != null && dataAlter_result.Company.Id > 0)
        //                            &&
        //                            (dataAlter_result.User != null)
        //                            &&
        //                            (dataAlter_result.Company.NeedSetPassword == true)
        //                        )
        //                        {
        //                            response.Data = GenerateAuthResponse(
        //                                dataAlter_result.User.Id ?? 0,
        //                                dataAlter_result.Company.UserId ?? 0,
        //                                dataAlter_result.Company.Id ?? 0,
        //                                dataAlter_result.Company.ClientRole ?? 0, ip);
        //                        }
        //                    }
        //                    return response;
        //                }
        //            }
        //            return result;
        //        default:
        //            response.Message = "Thông tin không hợp lệ. 999";
        //            return response;

        //    }
        //}

        //private object Signup_signup(SignupRequest request, bool isUsePhone)
        //{

        //    var response = new ApiResult<AuthResponse>()
        //    {
        //        Code = ResponseResultEnum.InvalidInput.Value(),
        //        Message = ResponseResultEnum.InvalidInput.Text()
        //    };

        //    // Validate input
        //    if (isUsePhone)
        //    {
        //        if (string.IsNullOrEmpty(request.Phone) || string.IsNullOrEmpty(request.PhoneCode))
        //        {
        //            response.Message = "Số điện thoại không được để trống.";
        //            return response;
        //        }
        //        if (!ValidationHelper.IsValidPhone(request.PhoneCode + request.Phone))
        //        {
        //            response.Message = "Số điện thoại không hợp lệ.";
        //            return response;
        //        }
        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(request.Mail))
        //        {
        //            response.Message = "Mail không được để trống.";
        //            return response;
        //        }
        //        if (!ValidationHelper.IsValidEmail(request.Mail))
        //        {
        //            response.Message = "Email không hợp lệ.";
        //            return response;
        //        }
        //    }

        //    if (string.IsNullOrEmpty(request.Stage))
        //    {
        //        response.Message = "Thông tin không hợp lệ.";
        //        return response;
        //    }

        //    var validateRequest = new ValidateAccountRequest()
        //    {
        //        PhoneCode = request.PhoneCode,
        //        Phone = request.Phone,
        //        Mail = request.Mail
        //    };

        //    var ip = WebUitility.GetIpAddressRequest();
        //    var imie = "";

        //    var result = BoFactory.Auth.UpdateFullNameSigupAsync(
        //                request.PhoneCode,
        //                request.Phone,
        //                request.Mail ?? string.Empty,
        //                request.Fullname ?? string.Empty,
        //                isUsePhone,
        //                ip,
        //                imie
        //                );

        //    if (request.IsMobileMenu == 1 && result.Data != null && result.Code == ResponseResultEnum.Success.Value())
        //    {
        //        var UpdateFullNameSigup_result = (UpdateFullNameSigupResponse)result.Data;
        //        if (UpdateFullNameSigup_result.UserId != null && UpdateFullNameSigup_result.UserId > 0)
        //        {
        //            var dataAlter = BoFactory.Auth.GetDataAlterAsync(
        //                    UpdateFullNameSigup_result.UserId ?? 0,
        //                    isUsePhone,
        //                    string.Format("{0}{1}", request.PhoneCode, request.Phone),
        //                    new List<string>() { "phone" });

        //            if (dataAlter.Code == ResponseResultEnum.Success.Value() && dataAlter.Data != null)
        //            {
        //                var dataAlter_result = (AuthResponse)dataAlter.Data;
        //                if (
        //                    (dataAlter_result.Company != null && dataAlter_result.Company.Id > 0)
        //                    &&
        //                    (dataAlter_result.User != null)
        //                    &&
        //                    (dataAlter_result.Company.NeedSetPassword == true)
        //                )
        //                {
        //                    response.Data = GenerateAuthResponse(
        //                        dataAlter_result.User.Id ?? 0,
        //                        dataAlter_result.Company.UserId ?? 0,
        //                        dataAlter_result.Company.Id ?? 0,
        //                        dataAlter_result.Company.ClientRole ?? 0, ip);
        //                }
        //            }
        //            return response;
        //        }
        //    }
        //    return result;
        //}
    }
}