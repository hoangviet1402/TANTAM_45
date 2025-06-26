using System;
using System.Data;
using System.Net;
using System.Web.Http;
using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Shift;
using BussinessObject.Models.OpenShift;
using Logger;
using TanTamApi.JWT.Helper;
using MyUtility.Extensions;
using System.Linq;

namespace TanTamApi.Controllers
{
    /// <summary>
    /// OpenShift management API controller
    /// </summary>
    [RoutePrefix("api/openshift")]
    public class OpenShiftController : ApiController
    {
        // TODO: Implement API endpoints

        [TanTamApi.JWT.Middleware.Authorize]
        [HttpGet]
        [Route("list")]
        public IHttpActionResult ListOpenShift([FromUri] ListOpenShiftRequest request)
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);

                if (companyId <= 0 || employeeId <= 0)
                {
                    return Content(HttpStatusCode.Unauthorized, new ApiResult<object>
                    {
                        Code = ResponseResultEnum.InvalidToken.Value(),
                        Message = "Phiên đăng nhập không hợp lệ"
                    });
                }

                var result = BoFactory.OpenShift.GetList(companyId, employeeId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"ListOpenShift Exception.", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<object>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý."
                });
            }
        }

        /// <summary>
        /// Create open shift
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost]
        [Route("create")]
        public IHttpActionResult CreateOpenShift([FromBody] CreateOpenShiftRequest request)
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);

                if (companyId <= 0 || employeeId <= 0)
                {
                    return Content(HttpStatusCode.Unauthorized, new ApiResult<object>
                    {
                        Code = ResponseResultEnum.InvalidToken.Value(),
                        Message = "Phiên đăng nhập không hợp lệ"
                    });
                }

                var result = BoFactory.OpenShift.Create(companyId, employeeId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"CreateOpenShift Exception.", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<object>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý."
                });
            }
        }

        /// <summary>
        /// Get shift list by working day
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpGet]
        [Route("shift-list-by-working-day")]
        public IHttpActionResult GetShiftListByWorkingDay([FromUri] ShiftListByWorkingDayRequest request)
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);

                if (companyId <= 0 || employeeId <= 0)
                {
                    return Content(HttpStatusCode.Unauthorized, new ApiResult<object>
                    {
                        Code = ResponseResultEnum.InvalidToken.Value(),
                        Message = "Phiên đăng nhập không hợp lệ"
                    });
                }

                var result = BoFactory.OpenShift.GetShiftListByWorkingDay(companyId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"GetShiftListByWorkingDay Exception.", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<object>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý."
                });
            }
        }

        /// <summary>
        /// Delete open shift
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost]
        [Route("delete/{id}")]
        public IHttpActionResult DeleteOpenShift(int id)
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);

                if (companyId <= 0 || employeeId <= 0)
                {
                    return Content(HttpStatusCode.Unauthorized, new ApiResult<object>
                    {
                        Code = ResponseResultEnum.InvalidToken.Value(),
                        Message = "Phiên đăng nhập không hợp lệ"
                    });
                }

                if (id <= 0)
                {
                    return Content(HttpStatusCode.BadRequest, new ApiResult<object>
                    {
                        Code = ResponseResultEnum.InvalidInput.Value(),
                        Message = "ID ca làm mở không hợp lệ"
                    });
                }

                var result = BoFactory.OpenShift.Delete(companyId, employeeId, id);
                
                if (result.Code == ResponseResultEnum.Success.Value())
                {
                    return Ok(result);
                }
                else if (result.Code == ResponseResultEnum.InvalidInput.Value())
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, result);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"DeleteOpenShift Exception.", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<object>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý."
                });
            }
        }

        /// <summary>
        /// Get open shift detail
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpGet]
        [Route("detail/{id}")]
        public IHttpActionResult GetOpenShiftDetail(int id)
        {
            try
            {
                // ✅ JWT AUTHENTICATION
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);

                if (companyId <= 0 || employeeId <= 0)
                {
                    return Content(HttpStatusCode.Unauthorized, new ApiResult<object>
                    {
                        Code = ResponseResultEnum.InvalidToken.Value(),
                        Message = "Phiên đăng nhập không hợp lệ"
                    });
                }

                // ✅ PARAMETER VALIDATION
                if (id <= 0)
                {
                    return Content(HttpStatusCode.BadRequest, new ApiResult<object>
                    {
                        Code = ResponseResultEnum.InvalidInput.Value(),
                        Message = "ID ca làm mở không hợp lệ"
                    });
                }

                // ✅ CALL BO - OPTIMIZED VERSION (Single DB call)
                var result = BoFactory.OpenShift.GetDetail(companyId, id);
                
                // ✅ RETURN APPROPRIATE STATUS
                if (result.Code == ResponseResultEnum.Success.Value())
                {
                    return Ok(result);
                }
                else if (result.Code == ResponseResultEnum.NotFound.Value())
                {
                    return Content(HttpStatusCode.NotFound, result);
                }
                else if (result.Code == ResponseResultEnum.InvalidInput.Value())
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
                else if (result.Code == ResponseResultEnum.Failed.Value())
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, result);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"GetOpenShiftDetail Exception.", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<object>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý."
                });
            }
        }

        /// <summary>
        /// Publish open shifts (set is_draft = false)
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost]
        [Route("publish")]
        public IHttpActionResult PublishOpenShift([FromBody] PublishOpenShiftRequest request)
        {
            try
            {
                // ✅ JWT AUTHENTICATION
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);

                if (companyId <= 0 || employeeId <= 0)
                {
                    return Content(HttpStatusCode.Unauthorized, new ApiResult<object>
                    {
                        Code = ResponseResultEnum.InvalidToken.Value(),
                        Message = "Phiên đăng nhập không hợp lệ"
                    });
                }

                // ✅ PARAMETER VALIDATION
                if (request?.ids == null || !request.ids.Any())
                {
                    return Content(HttpStatusCode.BadRequest, new ApiResult<object>
                    {
                        Code = ResponseResultEnum.InvalidInput.Value(),
                        Message = "Vui lòng cung cấp danh sách ID ca làm mở"
                    });
                }

                // ✅ CALL BO
                var result = BoFactory.OpenShift.Publish(companyId, employeeId, request);
                
                // ✅ RETURN APPROPRIATE STATUS
                if (result.Code == ResponseResultEnum.Success.Value())
                    return Ok(result);
                else if (result.Code == ResponseResultEnum.InvalidInput.Value())
                    return Content(HttpStatusCode.BadRequest, result);
                else if (result.Code == ResponseResultEnum.NotFound.Value())
                    return Content(HttpStatusCode.NotFound, result);
                else if (result.Code == ResponseResultEnum.Failed.Value())
                    return Content(HttpStatusCode.BadRequest, result);
                else
                    return Content(HttpStatusCode.InternalServerError, result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"PublishOpenShift Exception.", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<object>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý."
                });
            }
        }
    }
} 