using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Shift;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using Newtonsoft.Json;
using TanTamApi.JWT.Helper;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/wifi")]
    public class WifiController : ApiController
    {
        /// <summary>
        /// Tạo WiFi nâng cao với nhiều liên kết
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost]
        [Route("add-wifi")]
        public IHttpActionResult CreateWifiAdvanced([FromBody] WifiCreateAdvancedRequest request)
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var userId = JwtHelper.GetAccountIdFromToken(Request);

                if (companyId <= 0 || userId <= 0)
                {
                    return Content(
                        HttpStatusCode.Unauthorized,
                        new ApiResult<WifiCreateAdvancedResponse>
                        {
                            Code = ResponseResultEnum.InvalidToken.Value(),
                            Message = "Phiên đăng nhập không hợp lệ",
                            Data = new WifiCreateAdvancedResponse(),
                        }
                    );
                }

                if(request.type == null || request.type == "")
                {
                    request.type = "wifi";
                }

                var result = BoFactory.Wifi.CreateWifiAdvanced(request, companyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "WifiController.CreateWifiAdvanced - Error occurred: {0}",
                    ex
                );
                return Content(
                    HttpStatusCode.InternalServerError,
                    new ApiResult<WifiCreateAdvancedResponse>
                    {
                        Code = ResponseResultEnum.SystemError.Value(),
                        Message = "Đã xảy ra lỗi trong quá trình xử lý",
                        Data = new WifiCreateAdvancedResponse(),
                    }
                );
            }
        }

        /// <summary>
        /// Cập nhật WiFi theo WiFi ID
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost]
        [Route("update-wifi")]
        public IHttpActionResult UpdateWifi([FromBody] WifiUpdateRequest request)
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var userId = JwtHelper.GetAccountIdFromToken(Request);

                if (companyId <= 0 || userId <= 0)
                {
                    return Content(
                        HttpStatusCode.Unauthorized,
                        new ApiResult<WifiUpdateResponse>
                        {
                            Code = ResponseResultEnum.InvalidToken.Value(),
                            Message = "Phiên đăng nhập không hợp lệ",
                            Data = new WifiUpdateResponse(),
                        }
                    );
                }
                if (request.type == null || request.type == "")
                {
                    request.type = "wifi";
                }

                var result = BoFactory.Wifi.UpdateWifi(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "WifiController.UpdateWifi - Error occurred: {0}",
                    ex
                );
                return Content(
                    HttpStatusCode.InternalServerError,
                    new ApiResult<WifiUpdateResponse>
                    {
                        Code = ResponseResultEnum.SystemError.Value(),
                        Message = "Đã xảy ra lỗi trong quá trình xử lý",
                        Data = new WifiUpdateResponse(),
                    }
                );
            }
        }

        /// <summary>
        /// Lấy danh sách WiFi theo điều kiện
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost]
        [Route("list-wifi")]
        public IHttpActionResult GetWifiList([FromBody] WifiListRequestAdvanced request)
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var userId = JwtHelper.GetAccountIdFromToken(Request);

                if (companyId <= 0 || userId <= 0)
                {
                    return Content(
                        HttpStatusCode.Unauthorized,
                        new ApiResult<WifiListAdvancedResponse>
                        {
                            Code = ResponseResultEnum.InvalidToken.Value(),
                            Message = "Phiên đăng nhập không hợp lệ",
                            Data = new WifiListAdvancedResponse(),
                        }
                    );
                }
                if (request == null)
                {
                    request = new WifiListRequestAdvanced();
                    request.page = 1;
                    request.per_page = 15;
                    request.type = null;
                }

                if(request.type == null || request.type == "")
                {
                    request.type = "wifi";
                }
                
                // TYPE-SAFE ROUTING - Handle different return types
                if (request.type == "wifi")
                {
                    var wifiResult = BoFactory.Wifi.GetWifiListAdvanced(request, companyId);
                    return Ok(wifiResult);
                }
                else if(request.type == "location")
                {
                    var gpsResult = BoFactory.Wifi.GetGPSListAdvanced(request, companyId);
                    return Ok(gpsResult);
                }
                else
                {
                    // Default to WiFi list for any other type
                    var response = new ApiResult<WifiListAdvancedResponse>();
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "WifiController.GetWifiList - Error occurred: {0}",
                    ex
                );
                return Content(
                    HttpStatusCode.InternalServerError,
                    new ApiResult<WifiListAdvancedResponse>
                    {
                        Code = ResponseResultEnum.SystemError.Value(),
                        Message = "Đã xảy ra lỗi trong quá trình xử lý",
                        Data = new WifiListAdvancedResponse(),
                    }
                );
            }
        }

        /// <summary>
        /// Xóa WiFi theo WiFi ID
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost]
        [Route("delete-wifi")]
        public IHttpActionResult DeleteWifi(int wifiId)
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var userId = JwtHelper.GetAccountIdFromToken(Request);

                if (companyId <= 0 || userId <= 0)
                {
                    return Content(
                        HttpStatusCode.Unauthorized,
                        new ApiResult<string>
                        {
                            Code = ResponseResultEnum.InvalidToken.Value(),
                            Message = "Phiên đăng nhập không hợp lệ",
                            Data = string.Empty,
                        }
                    );
                }

                if (wifiId <= 0)
                {
                    return Content(
                        HttpStatusCode.BadRequest,
                        new ApiResult<string>
                        {
                            Code = ResponseResultEnum.InvalidData.Value(),
                            Message = "WiFi ID không hợp lệ",
                            Data = string.Empty,
                        }
                    );
                }

                var result = BoFactory.Wifi.DeleteWifi(wifiId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "WifiController.DeleteWifi - Error occurred: {0}",
                    ex
                );
                return Content(
                    HttpStatusCode.InternalServerError,
                    new ApiResult<string>
                    {
                        Code = ResponseResultEnum.SystemError.Value(),
                        Message = "Đã xảy ra lỗi trong quá trình xử lý",
                        Data = string.Empty,
                    }
                );
            }
        }
    }
}
