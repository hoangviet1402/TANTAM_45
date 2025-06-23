using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Auth;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TanTamApi.Helper;
using TanTamApi.JWT.Helper;
using WebUtility;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        
        [JWT.Middleware.Authorize]
        [HttpPost, Route("refreshtoken")]
        public HttpResponseMessage RefreshToken([FromBody] string refreshToken)
        {
            var response = new ApiResult<RefeshTokenResponse>()
            {
                Data = new RefeshTokenResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            var companyId = JwtHelper.GetCompanyIdFromToken(Request);
            var accountId = JwtHelper.GetAccountIdFromToken(Request);
            var jwtID = JwtHelper.GetJwtIdFromToken(Request);
            
            // Validate input (có thể thêm FluentValidation ở đây)
            if (string.IsNullOrEmpty(refreshToken))
            {
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "Vui lòng nhập đủ thông tin.";
                return Request.CreateResponse(HttpStatusCode.OK, response );
            }

            // Validate input length for security
            if (refreshToken.Length > 500)
            {
                CommonLogger.DefaultLogger.WarnFormat("RefreshToken: Suspicious long refresh token from accountId {0}, companyId {1}", accountId, companyId);
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "Thông tin không hợp lệ.";
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            if (accountId <= 0)
            {
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "Thông tin không được xác thực.";
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            try
            {
                // Implement refresh token logic here
                var tokenInfo = BoFactory.Auth.GetTokenInfo(accountId, companyId);
                if (tokenInfo != null)
                {
                    if (tokenInfo.RefreshToken.Equals(SecurityCommon.sha256_hash(refreshToken), StringComparison.OrdinalIgnoreCase) == false)
                    {
                        CommonLogger.DefaultLogger.WarnFormat("RefreshToken: Invalid refresh token for accountId {0}, companyId {1}", accountId, companyId);
                        response.Code = ResponseResultEnum.InvalidToken.Value();
                        response.Message = $"Phiên đăng nhập Không tồn tại.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else if (tokenInfo.JwtID.Equals(SecurityCommon.sha256_hash(jwtID), StringComparison.OrdinalIgnoreCase) == false)
                    {
                        CommonLogger.DefaultLogger.WarnFormat("RefreshToken: Invalid JWT ID for accountId {0}, companyId {1}", accountId, companyId);
                        response.Code = ResponseResultEnum.InvalidToken.Value();
                        response.Message = $"Phiên đăng nhập Không tồn tại.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else if (tokenInfo.Expires < DateTime.Now)
                    {
                        CommonLogger.DefaultLogger.InfoFormat("RefreshToken: Expired token for accountId {0}, companyId {1}", accountId, companyId);
                        response.Code = ResponseResultEnum.TokenExpired.Value();
                        response.Message = $"Phiên đăng nhập hết hạn vui lòng đăng nhập lại.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else if (tokenInfo.AccountIsActive ?? true == false)
                    {
                        CommonLogger.DefaultLogger.WarnFormat("RefreshToken: Locked account for accountId {0}, companyId {1}", accountId, companyId);
                        response.Code = ResponseResultEnum.AccountLocked.Value();
                        response.Message = $"Tài Khoản nhân viên này hiện bị khóa.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else if (tokenInfo.CompanyIsActive ?? true == false)
                    {
                        CommonLogger.DefaultLogger.WarnFormat("RefreshToken: Locked company for accountId {0}, companyId {1}", accountId, companyId);
                        response.Code = ResponseResultEnum.AccountLocked.Value();
                        response.Message = $"Công ty nhân viên đang làm việc hiện bị khóa.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }

                    else if (!tokenInfo.IsActive)
                    {
                        CommonLogger.DefaultLogger.WarnFormat("RefreshToken: Inactive employee for accountId {0}, companyId {1}", accountId, companyId);
                        response.Code = ResponseResultEnum.AccountLocked.Value();
                        response.Message = $"Nhân viên đang làm việc hiện bị khóa.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }

                    // Xử lý tạo accessToken mới
                    var newJwtID = "";
                    var ip = WebUitility.GetIpAddressRequest();
                    var imie = "";
                    var newAccessToken = JwtHelper.GenerateAccessToken(
                        tokenInfo.AccountId, 
                        tokenInfo.EmployeesInfoId ?? 0,
                        tokenInfo.CompanyId, 
                        tokenInfo.Role ?? 0, 
                    out newJwtID);

                    var isUpdateAccessToken = BoFactory.Auth.UpdateEmployeeJwtID(tokenInfo.Id, newJwtID, ip, imie);

                    if (isUpdateAccessToken > 0)
                    {
                        CommonLogger.DefaultLogger.InfoFormat("RefreshToken: Successfully refreshed token for accountId {0}, companyId {1}", accountId, companyId);
                        response.Data = new RefeshTokenResponse()
                        {
                            AccessToken = newAccessToken,
                        };
                        response.Code = ResponseResultEnum.Success.Value();

                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else
                    {
                        CommonLogger.DefaultLogger.ErrorFormat("RefreshToken: Failed to update JWT ID for accountId {0}, companyId {1}", accountId, companyId);
                        response.Code = ResponseResultEnum.AccountLocked.Value();
                        response.Message = $"Không tạo được token.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                }
                else
                {
                    CommonLogger.DefaultLogger.WarnFormat("RefreshToken: Token info not found for accountId {0}, companyId {1}", accountId, companyId);
                    response.Code = ResponseResultEnum.InvalidToken.Value();
                    response.Message = $"Phiên đăng nhập Không tồn tại.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("RefreshTokenAsync Exception refreshToken {0},jwtID {1}, accountId {2},companyId {3}  EX:", refreshToken , jwtID , accountId, companyId, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        [JWT.Middleware.Authorize]
        [HttpPost, Route("logout")]
        public HttpResponseMessage Logout([FromBody] string refreshToken)
        {
            try
            {
                var response = new ApiResult<bool>()
                {
                    Data = false,
                    Code = ResponseResultEnum.ServiceUnavailable.Value(),
                    Message = ResponseResultEnum.ServiceUnavailable.Text()
                };

                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountId = JwtHelper.GetAccountIdFromToken(Request);
                var ip = WebUitility.GetIpAddressRequest();
                var imie = "";

                // Validate input (có thể thêm FluentValidation ở đây)
                if (string.IsNullOrEmpty(refreshToken))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập đủ thông tin.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                // Implement refresh token logic here
                var tokenInfo = BoFactory.Auth.GetTokenInfo(accountId, companyId);
                if (tokenInfo != null && tokenInfo.RefreshToken.Equals(refreshToken, StringComparison.OrdinalIgnoreCase))
                {
                    var isUpdateOrInsertAccountToken = BoFactory.Auth.RevokeEmployeeToken(tokenInfo.Id, ip, imie); ;
                    if (isUpdateOrInsertAccountToken > 0)
                    {
                        response.Data = true;
                        response.Code = ResponseResultEnum.Success.Value();                        
                    }
                    else
                    {
                        response.Code = ResponseResultEnum.InvalidToken.Value();
                        response.Message = "Sai thông tin.";                       
                    }
                }
                else
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không tồn tại.";                   
                }
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("GetEmployee ID: Exception.", ex);
                return Request.CreateResponse(HttpStatusCode.OK,
                    new { message = "Đã xảy ra lỗi trong quá trình Login (-1)." });
            }
        }

        //[Authorize]
        [HttpPost, Route("set-password-new-user")]
        public HttpResponseMessage CreatePass([FromBody] CreatePassRequest request)
        {
            try
            {
                var response = new ApiResult<bool>()
                {
                    Data = false,
                    Code = ResponseResultEnum.ServiceUnavailable.Value(),
                    Message = ResponseResultEnum.ServiceUnavailable.Text()
                };

                // Validate input (có thể thêm FluentValidation ở đây)
                if (string.IsNullOrEmpty(request.Password))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập mật khẩu mới.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (string.IsNullOrEmpty(request.ComfirmPass))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng xác nhận mật khẩu.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (request.ComfirmPass.Equals(request.Password) == false)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Xác nhận mật khẩu không khớp với mật khẩu được nhập.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountId = JwtHelper.GetAccountIdFromToken(Request);
                if(companyId == 0)
                {
                    companyId = request.ShopId ?? 0;
                }

                if (accountId == 0)
                {
                    accountId = request.UserId ?? 0;
                }
                var ip = WebUitility.GetIpAddressRequest();
                var imie = "";

                try
                {
                    

                    // Implement refresh token logic here
                    var updatePass = BoFactory.Auth.UpdatePass(accountId, companyId, request.Password, "", 1);
                    if (updatePass > 0)
                    {
                        response.Data = true;
                        response.Code = ResponseResultEnum.Success.Value();
                        response.Message = "Cập nhật mật khẩu thành công";                        
                    }
                    else
                    {
                        response.Code = ResponseResultEnum.InvalidToken.Value();
                        response.Message = $"Sai thông tin.";
                    }
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat("UpdatePassAsync Exception  {0} int newPass {1}, int comfirmPass {2}, EX : ", accountId, request.Password, request.ComfirmPass, ex);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = ResponseResultEnum.SystemError.Text();
                   
                }
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("GetEmployee ID: Exception.", ex);
                return Request.CreateResponse(HttpStatusCode.OK,
                    new { message = "Đã xảy ra lỗi trong quá trình Login (-1)." });
            }
        }

        [JWT.Middleware.Authorize]
        [HttpPost, Route("changepass")]
        public HttpResponseMessage ChangePass([FromBody] ChangePassRequest request)
        {
            try
            {
                var response = new ApiResult<bool>()
                {
                    Data = false,
                    Code = ResponseResultEnum.ServiceUnavailable.Value(),
                    Message = ResponseResultEnum.ServiceUnavailable.Text()
                };

                // Validate input (có thể thêm FluentValidation ở đây)
                if (string.IsNullOrEmpty(request.NewPass))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập mật khẩu mới.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (string.IsNullOrEmpty(request.OldPass))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng xác nhận mật khẩu cũ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (request.NewPass.Equals(request.OldPass) == true)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Mật khẩu cũ và mới trùng nhau.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountId = JwtHelper.GetAccountIdFromToken(Request);
                var ip = WebUitility.GetIpAddressRequest();
                var imie = "";

                try
                {                    
                    // Implement refresh token logic here
                    var updatePass = BoFactory.Auth.UpdatePass(accountId, companyId, request.NewPass, request.OldPass, 0);
                    if (updatePass > 0)
                    {
                        response.Data = true;
                        response.Code = ResponseResultEnum.Success.Value();
                        response.Message = "Cập nhật mật khẩu thành công";
                        
                    }
                    else
                    {
                        response.Code = ResponseResultEnum.InvalidToken.Value();
                        response.Message = $"Sai thông tin.";
                        
                    }
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat("UpdatePassAsync Exception  {0} int newPass {1}, int comfirmPass {2}, EX : ", accountId, request.NewPass, request.OldPass, ex);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = ResponseResultEnum.SystemError.Text();                    
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (ArgumentException ex)
            {
                CommonLogger.DefaultLogger.Error("Login Tham số không hợp lệ. Lỗi: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.OK, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("GetEmployee ID: Exception.", ex);
                return Request.CreateResponse(HttpStatusCode.OK,
                    new { message = "Đã xảy ra lỗi trong quá trình Login (-1)." });
            }
        }

        [HttpPost, Route("signup/phone")]
        public HttpResponseMessage SignupPhone([FromBody] SignupRequest request)
        {
            var response = new ApiResult<AuthResponse>()
            {
                Code = ResponseResultEnum.InvalidInput.Value(),
                Message = ResponseResultEnum.InvalidInput.Text()
            };
            var isUsePhone = true;
            // Validate input
            if (string.IsNullOrEmpty(request.Phone) || string.IsNullOrEmpty(request.PhoneCode))
            {
                response.Message = "Số điện thoại không được để trống.";
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            request.Phone = StringCommon.ExtractCoreNumber(request.Phone);

            if (!ValidationHelper.IsValidPhone(request.PhoneCode + request.Phone))
            {
                response.Message = "Số điện thoại không hợp lệ.";
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            if (string.IsNullOrEmpty(request.Stage))
            {
                response.Message = "Thông tin không hợp lệ.";
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            var ip = WebUitility.GetIpAddressRequest();
            var imie = "";

            try
            {
                switch (request.Stage.ToLower())
                {
                    case "validate":
                        var resultValidate = BoFactory.Auth.ValidateAccountAsync(new ValidateAccountRequest()
                        {
                            PhoneCode = request.PhoneCode,
                            Phone = request.Phone,
                            Mail = request.Mail
                        }, isUsePhone);
                        if (resultValidate.Code == ResponseResultEnum.Success.Value() && resultValidate.Data != null)
                        {
                            var resultValidate_date = (ValidateAccountResponse)resultValidate.Data;
                            if (resultValidate_date != null && resultValidate_date.AccountId != null && resultValidate_date.AccountId > 0)
                            {
                                response = BoFactory.Auth.GetDataAlterAsync(
                                    resultValidate_date.AccountId.Value,
                                    isUsePhone,
                                    request.PhoneCode + request.Phone,
                                    new List<string>() { "phone" });
                                return Request.CreateResponse(HttpStatusCode.OK, response);
                            }
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, resultValidate);
                    case "signup":
                        var result = BoFactory.Auth.UpdateFullNameSigupAsync(
                            request.PhoneCode,
                            request.Phone,
                            request.Mail ?? string.Empty,
                            request.Fullname ?? string.Empty,
                            isUsePhone,
                            ip,
                            imie
                            );
                        if (result.Data != null && result.Code == ResponseResultEnum.Success.Value()) // request.IsMobileMenu == 1 && 
                        {
                            var UpdateFullNameSigup_result = (UpdateFullNameSigupResponse)result.Data;
                            response = JwtHelper.GenerateAuthResponse(
                                           UpdateFullNameSigup_result.UserId ?? 0,
                                           UpdateFullNameSigup_result.AccountMapId ?? 0,                                           
                                           UpdateFullNameSigup_result.ShopId ?? 0,
                                           UpdateFullNameSigup_result.ClientRole ?? 0, ip);
                            return Request.CreateResponse(HttpStatusCode.OK, response);
                            //if (UpdateFullNameSigup_result.UserId != null && UpdateFullNameSigup_result.UserId > 0)
                            //{
                            //    var dataAlter = BoFactory.Auth.GetDataAlterAsync(
                            //            UpdateFullNameSigup_result.UserId ?? 0,
                            //            isUsePhone,
                            //            string.Format("{0}{1}", request.PhoneCode, request.Phone),
                            //            new List<string>() { "phone" });

                            //    if (dataAlter.Code == ResponseResultEnum.Success.Value() && dataAlter.Data != null)
                            //    {
                            //        var dataAlter_result = (AuthResponse)dataAlter.Data;
                            //        if (
                            //            (dataAlter_result.Company != null && dataAlter_result.Company.Id > 0)
                            //            &&
                            //            (dataAlter_result.User != null)
                            //            &&
                            //            (dataAlter_result.Company.NeedSetPassword == true)
                            //        )
                            //        {
                            //            response = JwtHelper.GenerateAuthResponse(
                            //                dataAlter_result.User.Id ?? 0,
                            //                dataAlter_result.Company.UserId ?? 0,
                            //                dataAlter_result.Company.Id ?? 0,
                            //                dataAlter_result.Company.ClientRole ?? 0, ip);
                            //        }
                            //    }
                            //    return Request.CreateResponse(HttpStatusCode.OK, response);
                            //}
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    default:
                        response.Message = "Thông tin không hợp lệ. 999";
                        return Request.CreateResponse(HttpStatusCode.OK, response);

                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("SignupPhone Exception: ", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Thông tin không hợp lệ. 9999";
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        [HttpPost, Route("signup/mail")]
        public HttpResponseMessage SignupMail([FromBody] SignupRequest request)
        {
            var response = new ApiResult<AuthResponse>()
            {
                Code = ResponseResultEnum.InvalidInput.Value(),
                Message = ResponseResultEnum.InvalidInput.Text()
            };
            var isUsePhone = false;
            // Validate input
            if (string.IsNullOrEmpty(request.Mail))
            {
                response.Message = "Mail không được để trống.";
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            if (!ValidationHelper.IsValidEmail(request.Mail))
            {
                response.Message = "Email không hợp lệ.";
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            if (string.IsNullOrEmpty(request.Stage))
            {
                response.Message = "Thông tin không hợp lệ.";
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            var ip = WebUitility.GetIpAddressRequest();
            var imie = "";

            try
            {
                switch (request.Stage.ToLower())
                {
                    case "validate":
                        var resultValidate = BoFactory.Auth.ValidateAccountAsync(new ValidateAccountRequest()
                        {
                            PhoneCode = request.PhoneCode,
                            Phone = request.Phone,
                            Mail = request.Mail
                        }, isUsePhone);
                        if (resultValidate.Code == ResponseResultEnum.Success.Value() && resultValidate.Data != null)
                        {
                            var resultValidate_date = (ValidateAccountResponse)resultValidate.Data;
                            if (resultValidate_date != null && resultValidate_date.AccountId != null && resultValidate_date.AccountId > 0)
                            {
                                response = BoFactory.Auth.GetDataAlterAsync(
                                    resultValidate_date.AccountId.Value,
                                    isUsePhone,
                                    request.PhoneCode + request.Phone,
                                    new List<string>() { "phone" });
                                return Request.CreateResponse(HttpStatusCode.OK, response);
                            }
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, resultValidate);
                    case "signup":
                        var result = BoFactory.Auth.UpdateFullNameSigupAsync(
                            request.PhoneCode,
                            request.Phone,
                            request.Mail ?? string.Empty,
                            request.Fullname ?? string.Empty,
                            isUsePhone,
                            ip,
                            imie
                            );
                        if (request.IsMobileMenu == 1 && result.Data != null && result.Code == ResponseResultEnum.Success.Value())
                        {
                            var UpdateFullNameSigup_result = (UpdateFullNameSigupResponse)result.Data;
                            if (UpdateFullNameSigup_result.UserId != null && UpdateFullNameSigup_result.UserId > 0)
                            {
                                var dataAlter = BoFactory.Auth.GetDataAlterAsync(
                                        UpdateFullNameSigup_result.UserId ?? 0,
                                        isUsePhone,
                                        string.Format("{0}{1}", request.PhoneCode, request.Phone),
                                        new List<string>() { "phone" });

                                if (dataAlter.Code == ResponseResultEnum.Success.Value() && dataAlter.Data != null)
                                {
                                    var dataAlter_result = (AuthResponse)dataAlter.Data;
                                    if (
                                        (dataAlter_result.Company != null && dataAlter_result.Company.Id > 0)
                                        &&
                                        (dataAlter_result.User != null)
                                        &&
                                        (dataAlter_result.Company.NeedSetPassword == true)
                                    )
                                    {
                                        response = JwtHelper.GenerateAuthResponse(
                                            dataAlter_result.User.Id ?? 0,
                                            dataAlter_result.Company.UserId ?? 0,
                                            dataAlter_result.Company.Id ?? 0,
                                            dataAlter_result.Company.ClientRole ?? 0, ip);
                                    }
                                }
                                return Request.CreateResponse(HttpStatusCode.OK, response);
                            }
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    default:
                        response.Message = "Thông tin không hợp lệ. 999";
                        return Request.CreateResponse(HttpStatusCode.OK, response);

                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("SignupPhone Exception: ", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Thông tin không hợp lệ. 9999";
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        [HttpPost, Route("phone/signin-v2")]
        public HttpResponseMessage PhoneSignin([FromBody] SigninRequest request)
        {
            var response = new ApiResult<AuthResponse>()
            {
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var isUsePhone = true;
                var ip = WebUitility.GetIpAddressRequest();
                var userAgent = "";
                request.Phone = StringCommon.ExtractCoreNumber(request.Phone);

                if (!ValidateInput(request, isUsePhone, out var errorMsg))
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult<int?> { Code = ResponseResultEnum.InvalidInput.Value(), Message = errorMsg });

                switch (request.Stage.ToLower())
                {
                    case "validate":
                        var resultValidate = BoFactory.Auth.ValidateAccountAsync(new ValidateAccountRequest()
                        {
                            PhoneCode = request.PhoneCode,
                            Phone = request.Phone,
                            Mail = request.Mail
                        }, isUsePhone);

                        if (resultValidate.Code == ResponseResultEnum.Success.Value() &&
                            resultValidate.Data != null && ((ValidateAccountResponse)resultValidate.Data).AccountId != null)
                        {
                            response = BoFactory.Auth.GetDataAlterAsync(
                                ((ValidateAccountResponse)resultValidate.Data).AccountId.Value,
                                isUsePhone,
                                request.PhoneCode + request.Phone,
                                new List<string>() { "phone" });
                            return Request.CreateResponse(HttpStatusCode.OK, response);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, resultValidate);

                    case "signin":
                        var result = BoFactory.Auth.SigninAsync(request, isUsePhone, ip, userAgent);
                        if (result.Code == ResponseResultEnum.Success.Value())
                        {
                            var authdata = (Ins_Account_Login_Result)result.Data;
                            response = JwtHelper.GenerateAuthResponse(
                                authdata.AccountId,
                                authdata.Id,
                                authdata.CompanyId,
                                authdata.Role ?? 0,
                                ip
                            );
                        }
                        else
                        {
                            response.Code = result.Code;
                            response.Message = result.Message;
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, response);

                    default:
                        response.Code = ResponseResultEnum.SystemError.Value();
                        response.Message = "Thông tin không hợp lệ. 999";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("PhoneSignin Exception: ", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Thông tin không hợp lệ. 9999";
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        [HttpPost, Route("mail/signin-v2")]
        public HttpResponseMessage MailSignin([FromBody] SigninRequest request)
        {
            var response = new ApiResult<AuthResponse>()
            {
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var isUsePhone = false;
                var ip = WebUitility.GetIpAddressRequest();
                var userAgent = "";

                if (!ValidateInput(request, isUsePhone, out var errorMsg))
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult<int?> { Code = ResponseResultEnum.InvalidInput.Value(), Message = errorMsg });

                switch (request.Stage.ToLower())
                {
                    case "validate":
                        var resultValidate = BoFactory.Auth.ValidateAccountAsync(new ValidateAccountRequest()
                        {
                            PhoneCode = request.PhoneCode,
                            Phone = request.Phone,
                            Mail = request.Mail
                        }, isUsePhone);

                        if (resultValidate.Code == ResponseResultEnum.Success.Value() &&
                            resultValidate.Data != null && ((ValidateAccountResponse)resultValidate.Data).AccountId != null)
                        {
                            response = BoFactory.Auth.GetDataAlterAsync(
                                ((ValidateAccountResponse)resultValidate.Data).AccountId.Value,
                                isUsePhone,
                                request.PhoneCode + request.Phone,
                                new List<string>() { "phone" });
                            return Request.CreateResponse(HttpStatusCode.OK, response);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, resultValidate);

                    case "signin":
                        var result = BoFactory.Auth.SigninAsync(request, isUsePhone, ip, userAgent);
                        if (result.Code == ResponseResultEnum.Success.Value())
                        {
                            var authdata = (Ins_Account_Login_Result)result.Data;
                            response = JwtHelper.GenerateAuthResponse(
                                authdata.AccountId,
                                authdata.Id,
                                authdata.CompanyId,
                                authdata.Role ?? 0,
                                ip
                            );
                        }
                        else
                        {
                            response.Code = result.Code;
                            response.Message = result.Message;
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, response);

                    default:
                        response.Code = ResponseResultEnum.SystemError.Value();
                        response.Message = "Thông tin không hợp lệ. 999";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("MailSignin Exception: ", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Thông tin không hợp lệ. 9999";
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        private bool ValidateInput(SigninRequest request, bool isUsePhone, out string errorMsg)
        {
            errorMsg = string.Empty;

            if (string.IsNullOrEmpty(request.Stage))
            {
                errorMsg = "Thông tin không hợp lệ.";
                return false;
            }

            if (request.Stage == "signin")
            {
                if (string.IsNullOrEmpty(request.Password))
                {
                    errorMsg = "Vui lòng nhập mật khẩu.";
                    return false;
                }
                return true;
            }
            else
            {
                if (isUsePhone)
                {
                    if (string.IsNullOrEmpty(request.Phone) || string.IsNullOrEmpty(request.PhoneCode))
                    {
                        errorMsg = "Số điện thoại không được để trống.";
                        return false;
                    }
                    if (!ValidationHelper.IsValidPhone(request.PhoneCode + request.Phone))
                    {
                        errorMsg = "Số điện thoại không hợp lệ.";
                        return false;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(request.Mail))
                    {
                        errorMsg = "Email không được để trống.";
                        return false;
                    }
                    if (!ValidationHelper.IsValidEmail(request.Mail))
                    {
                        errorMsg = "Email không hợp lệ.";
                        return false;
                    }
                }
            }
            
            return true;
        }
    }
}