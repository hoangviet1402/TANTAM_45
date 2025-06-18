using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Auth;
using Logger;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TanTamApi.Helper;
using TanTamApi.JWT.Helper;
using WebUtility;

namespace TanTamApi.Controllers
{
    public class AuthController : ApiController
    {

        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost, Route("api/auth/refreshtoken")]
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
                    if (tokenInfo.RefreshToken.Equals(AESHelper.HashPassword(refreshToken), StringComparison.OrdinalIgnoreCase) == false)
                    {
                        response.Code = ResponseResultEnum.InvalidToken.Value();
                        response.Message = $"Phiên đăng nhập Không tồn tại.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else if (tokenInfo.JwtID.Equals(AESHelper.HashPassword(jwtID), StringComparison.OrdinalIgnoreCase) == false)
                    {
                        response.Code = ResponseResultEnum.InvalidToken.Value();
                        response.Message = $"Phiên đăng nhập Không tồn tại.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else if (tokenInfo.Expires < DateTime.Now)
                    {
                        response.Code = ResponseResultEnum.TokenExpired.Value();
                        response.Message = $"Phiên đăng nhập hết hạn vui lòng đăng nhập lại.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else if (tokenInfo.AccountIsActive ?? true == false)
                    {
                        response.Code = ResponseResultEnum.AccountLocked.Value();
                        response.Message = $"Tài Khoản nhân viên này hiện bị khóa.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else if (tokenInfo.CompanyIsActive ?? true == false)
                    {
                        response.Code = ResponseResultEnum.AccountLocked.Value();
                        response.Message = $"Công ty nhân viên đang làm việc hiện bị khóa.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }

                    else if (!tokenInfo.IsActive)
                    {
                        response.Code = ResponseResultEnum.AccountLocked.Value();
                        response.Message = $"Nhân viên đang làm việc hiện bị khóa.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }

                    // Xử lý tạo accessToken mới
                    var newJwtID = "";
                    var ip = WebUitility.GetIpAddressRequest();
                    var imie = "";
                    var configuration = new AppSettingsConfiguration();
                    var newAccessToken = JwtHelper.GenerateAccessToken(
                        tokenInfo.AccountId, 
                        tokenInfo.EmployeesInfoId ?? 0,
                        tokenInfo.CompanyId, 
                        tokenInfo.Role ?? 0, 
                        configuration, 
                    out newJwtID);

                    var isUpdateAccessToken = BoFactory.Auth.UpdateEmployeeJwtID(tokenInfo.Id, newJwtID, ip, imie);

                    if (isUpdateAccessToken > 0)
                    {
                        response.Data = new RefeshTokenResponse()
                        {
                            AccessToken = newAccessToken,
                        };
                        response.Code = ResponseResultEnum.Success.Value();

                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else
                    {
                        response.Code = ResponseResultEnum.AccountLocked.Value();
                        response.Message = $"Không tạo được token.";
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                }
                else
                {
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

        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost, Route("api/auth/logout")]
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { message = "Đã xảy ra lỗi trong quá trình Login (-1)." });
            }
        }

        //[Authorize]
        [HttpPost, Route("api/auth/set-password-new-user")]
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { message = "Đã xảy ra lỗi trong quá trình Login (-1)." });
            }
        }

        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost, Route("api/auth/changepass")]
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

                if (request.NewPass.Equals(request.OldPass) == false)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Xác nhận mật khẩu không khớp với mật khẩu được nhập.";
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("GetEmployee ID: Exception.", ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { message = "Đã xảy ra lỗi trong quá trình Login (-1)." });
            }
        }

        [HttpPost, Route("api/auth/signup/phone")]
        public HttpResponseMessage SignupPhone([FromBody] SignupRequest request)
        {
            try
            {
                var result = SignupCommon(request, true);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost, Route("api/auth/signup/mail")]
        public HttpResponseMessage SignupMail([FromBody] SignupRequest request)
        {
            try
            {
                var result = SignupCommon(request, false);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost, Route("api/auth/phone/signin-v2")]
        public HttpResponseMessage PhoneSignin([FromBody] SigninRequest request)
        {
            try
            {
                var isUsePhone = true;
                if (!ValidateInput(request, isUsePhone, out var errorMsg))
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult<int?> { Code = ResponseResultEnum.InvalidInput.Value(), Message = errorMsg });

                var result = HandleStageAsync(request, isUsePhone, "", "");
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost, Route("api/auth/mail/signin-v2")]
        public HttpResponseMessage MailSignin([FromBody] SigninRequest request)
        {
            try
            {
                var isUsePhone = false;
                if (!ValidateInput(request, isUsePhone, out var errorMsg))
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult<int?> { Code = ResponseResultEnum.InvalidInput.Value(), Message = errorMsg });

                var result = HandleStageAsync(request, isUsePhone, "", "");
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        private HttpResponseMessage HandleException(Exception ex, string message = "Đã xảy ra lỗi trong quá trình xử lý (-1).")
        {
            if (ex is ArgumentException)
            {
                CommonLogger.DefaultLogger.Error("Tham số không hợp lệ. Lỗi: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = ex.Message });
            }
            CommonLogger.DefaultLogger.Error("Exception.", ex);
            return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message });
        }

        private bool ValidateInput(SigninRequest request, bool isUsePhone, out string errorMsg)
        {
            errorMsg = string.Empty;

            if (request.Stage == "signin")
            {
                return true;
            }

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
            if (string.IsNullOrEmpty(request.Stage))
            {
                errorMsg = "Thông tin không hợp lệ.";
                return false;
            }
            return true;
        }

        private HttpResponseMessage HandleStageAsync(SigninRequest request, bool isUsePhone, string ip, string userAgent)
        {
            var response = new ApiResult<int?>()
            {
                Code = ResponseResultEnum.InvalidInput.Value(),
                Message = ResponseResultEnum.InvalidInput.Text()
            };

            var validateRequest = new ValidateAccountRequest()
            {
                PhoneCode = request.PhoneCode,
                Phone = request.Phone,
                Mail = request.Mail
            };

            switch (request.Stage.ToLower())
            {
                case "validate":
                    var resultValidate = BoFactory.Auth.ValidateAccountAsync(validateRequest, isUsePhone);

                    if (resultValidate.Code == ResponseResultEnum.Success.Value() &&
                        resultValidate.Data != null && ((ValidateAccountResponse)resultValidate.Data).AccountId != null)
                    {
                        var dataAlter = BoFactory.Auth.GetDataAlterAsync(
                            ((ValidateAccountResponse)resultValidate.Data).AccountId.Value,
                            isUsePhone,
                            request.PhoneCode + request.Phone,
                            new List<string>() { "phone" });
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, dataAlter);
                    }
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resultValidate);

                case "signin":
                    if (isUsePhone && string.IsNullOrEmpty(request.Password))
                    {
                        response.Message = "Vui lòng nhập mật khẩu";
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
                    }
                    var result = BoFactory.Auth.SigninAsync(request, isUsePhone, ip, userAgent);
                    if(result.Code == )
                    response.Data = GenerateAndSaveTokensAsync(
                        authdata.AccountId,
                        authdata.Id, //employeeAccountMapId
                        authdata.CompanyId,
                        authdata.Role ?? 0,
                        ip,
                        imie,
                        (accessToken, refreshToken, accountId, CompanyId) => new AuthResponse
                        {
                            AccessToken = accessToken,
                            RefreshToken = refreshToken,
                            UserId = accountId,
                            ShopId = CompanyId,
                        }
                    );
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, result);

                default:
                    response.Message = "Thông tin không hợp lệ. 999";
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        private async HttpResponseMessage SignupCommon(SignupRequest request, bool isUsePhone)
        {
            var ip = HttpContextExtensions.GetClientIpAddress(HttpContext);
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var response = new ApiResult<int?>()
            {
                Code = ResponseResultEnum.InvalidInput.Value(),
                Message = ResponseResultEnum.InvalidInput.Text()
            };

            // Validate input
            if (isUsePhone)
            {
                if (string.IsNullOrEmpty(request.Phone) || string.IsNullOrEmpty(request.PhoneCode))
                {
                    response.Message = "Số điện thoại không được để trống.";
                    return Ok(response);
                }
                if (!ValidationHelper.IsValidPhone(request.PhoneCode + request.Phone))
                {
                    response.Message = "Số điện thoại không hợp lệ.";
                    return Ok(response);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(request.Mail))
                {
                    response.Message = "Mail không được để trống.";
                    return Ok(response);
                }
                if (!ValidationHelper.IsValidEmail(request.Mail))
                {
                    response.Message = "Email không hợp lệ.";
                    return Ok(response);
                }
            }

            if (string.IsNullOrEmpty(request.Stage))
            {
                response.Message = "Thông tin không hợp lệ.";
                return Ok(response);
            }

            var validateRequest = new ValidateAccountRequest()
            {
                PhoneCode = request.PhoneCode,
                Phone = request.Phone,
                Mail = request.Mail
            };

            switch ((request.Stage ?? string.Empty).ToLower())
            {
                case "validate":
                    var resultValidate = await _authService.ValidateAccountAsync(validateRequest, isUsePhone);
                    if (resultValidate.Code == ResponseResultEnum.Success.Value())
                    {
                        if (resultValidate.Data != null && resultValidate.Data.AccountId != null && resultValidate.Data.AccountId > 0)
                        {
                            var dataAlter = await _authService.GetDataAlterAsync(
                                resultValidate.Data.AccountId.Value,
                                isUsePhone,
                                request.PhoneCode + request.Phone,
                                new List<string>() { "phone" });
                            return Ok(dataAlter);
                        }
                        return Ok(resultValidate);
                    }
                    return Ok(resultValidate);
                case "signup":
                    var result = await _authService.UpdateFullNameSigupAsync(
                        request.PhoneCode,
                        request.Phone,
                        request.Mail ?? string.Empty,
                        request.Fullname ?? string.Empty,
                        isUsePhone,
                        ip,
                        imie: userAgent);
                    if (request.IsMobileMenu == 1 && result.Data != null && result.Code == ResponseResultEnum.Success.Value() && result.Data.UserId != null && result.Data.UserId > 0)
                    {
                        var dataAlter = await _authService.GetDataAlterAsync(
                                result.Data.UserId ?? 0,
                                isUsePhone,
                                request.PhoneCode + request.Phone,
                                new List<string>() { "phone" });
                        if (
                            dataAlter.Code == ResponseResultEnum.Success.Value()
                            &&
                            (dataAlter.Data != null)
                            &&
                            (dataAlter.Data.Company != null && dataAlter.Data.Company.Id > 0)
                            &&
                            (dataAlter.Data.User != null)
                            &&
                            (dataAlter.Data.Company.NeedSetPassword == true)
                        )
                        {
                            var createToken = await _authService.GenerateAndSaveTokensAsync(
                                dataAlter.Data.User.Id ?? 0,
                                dataAlter.Data.Company.UserId ?? 0,
                                dataAlter.Data.Company.Id ?? 0,
                                dataAlter.Data.Company.ClientRole ?? 0,
                                "ip",
                                "imie",
                                (accessToken, refreshToken, accountId, companyId) => new UpdateFullNameSigupResponse
                                {
                                    AccessToken = accessToken,
                                    RefreshToken = refreshToken,
                                }
                            );
                            dataAlter.Data.RefreshToken = createToken.RefreshToken;
                            dataAlter.Data.AccessToken = createToken.AccessToken;
                        }
                        return Ok(dataAlter);
                    }
                    return Ok(result);
                default:
                    response.Message = "Thông tin không hợp lệ. 999";
                    return Ok(response);

            }
        }

        // Test method để kiểm tra việc lấy thông tin từ JWT token
        [TanTamApi.JWT.Middleware.Authorize]
        [System.Web.Http.HttpGet, Route("api/auth/test-token")]
        public HttpResponseMessage TestToken()
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountId = JwtHelper.GetAccountIdFromToken(Request);
                var employeeId = JwtHelper.GetEmployeeIdFromToken(Request);
                var role = JwtHelper.GetRoleFromToken(Request);
                var jwtId = JwtHelper.GetJwtIdFromToken(Request);

                var result = new
                {
                    CompanyId = companyId,
                    AccountId = accountId,
                    EmployeeId = employeeId,
                    Role = role,
                    JwtId = jwtId,
                    Message = "Token information retrieved successfully"
                };

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Error retrieving token information", error = ex.Message });
            }
        }

        // Ví dụ: Có thể bỏ [FromBody] khi dùng query string
        [HttpGet, Route("api/auth/get-user")]
        public HttpResponseMessage GetUser(int userId, int companyId) // Không cần [FromBody]
        {
            // userId và companyId sẽ lấy từ query string: /api/auth/get-user?userId=123&companyId=456
            return Request.CreateResponse(HttpStatusCode.OK, new { UserId = userId, CompanyId = companyId });
        }

        // Ví dụ: Có thể bỏ [FromBody] khi dùng route parameter
        [HttpGet, Route("api/auth/user/{userId}/company/{companyId}")]
        public HttpResponseMessage GetUserByRoute(int userId, int companyId) // Không cần [FromBody]
        {
            // userId và companyId sẽ lấy từ route: /api/auth/user/123/company/456
            return Request.CreateResponse(HttpStatusCode.OK, new { UserId = userId, CompanyId = companyId });
        }

        // Ví dụ: Có thể bỏ [FromBody] khi dùng form data
        [HttpPost, Route("api/auth/upload")]
        public HttpResponseMessage UploadFile(string fileName, string description) // Không cần [FromBody]
        {
            // fileName và description sẽ lấy từ form data
            return Request.CreateResponse(HttpStatusCode.OK, new { FileName = fileName, Description = description });
        }


        private TResponse GenerateAndSaveTokensAsync<TResponse>(int accountId, int employeeAccountMapId, int companyId, int role, string ip, string imie, Func<string, string, int, int, TResponse> responseFactory)
        {
            string jwtID;
            var _configuration = new AppSettingsConfiguration();
            var accessToken = JwtHelper.GenerateAccessToken(accountId, employeeAccountMapId, companyId, role, _configuration, out jwtID);
            var refreshToken = JwtHelper.GenerateRefreshToken();
            int lifeTime = 30;
            var configValue = _configuration["Token:RefreshTokenLifeTime"];
            if (int.TryParse(configValue, out int parsedLifeTime))
            {
                lifeTime = parsedLifeTime;
            }

            BoFactory.Auth.InsertEmployeeToken(employeeAccountMapId, jwtID, refreshToken, lifeTime, ip, imie);
            return responseFactory(accessToken, refreshToken, accountId, companyId);
        }
    }
}