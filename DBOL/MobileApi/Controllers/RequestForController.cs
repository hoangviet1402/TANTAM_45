using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Report;
using BussinessObject.Models.RequestFor;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http;
using TanTamApi.JWT.Helper;
using TanTamApi.JWT.Middleware;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/request")]
    public class RequestForController : ApiController
    {
        
        [HttpGet]
        [Route("list-request-types")]
        public IHttpActionResult RequestTypesGetList()
        {
            
            var response = new ApiResult<RequestTypeResponse>
            {
                Data = new RequestTypeResponse(),
                Code = ResponseResultEnum.NoData.Value(),
                Message = ResponseResultEnum.NoData.Text(),
            };

            try
            {
                response = BoFactory.RequestFor.RequestTypes_GetAll(0);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("TimesheetTabWorkingHours Exception CompanyId: {0}, EX: {1}",
                    JwtHelper.GetCompanyIdFromToken(Request), ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        [ApiAuthorize]
        [HttpPost]
        [Route("onleave/add")]
        public IHttpActionResult RequestSend([FromBody] RequestForRequest request)
        {

            var response = new ApiResult<RequestForResponse>
            {
                Data = new RequestForResponse(),
                Code = ResponseResultEnum.NoData.Value(),
                Message = ResponseResultEnum.NoData.Text(),
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountMapID = JwtHelper.GetAccountIdFromToken(Request);
                if(companyId <= 0 || accountMapID <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidToken.Value();
                    response.Message = "Phiên đăng nhập không hợp lệ";
                }
                response = BoFactory.RequestFor.Request_CreateRequestWithShift(request, accountMapID, companyId);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("send-request Exception CompanyId: {0}, EX: {1}",
                    JwtHelper.GetCompanyIdFromToken(Request), ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        [HttpGet]
        [Route("list-request")]
        public IHttpActionResult RequestGetList()
        {

            var response = new ApiResult<RequestTypeResponse>
            {
                Data = new RequestTypeResponse(),
                Code = ResponseResultEnum.NoData.Value(),
                Message = ResponseResultEnum.NoData.Text(),
            };

            try
            {
                response = BoFactory.RequestFor.RequestTypes_GetAll(0);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("TimesheetTabWorkingHours Exception CompanyId: {0}, EX: {1}",
                    JwtHelper.GetCompanyIdFromToken(Request), ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        //Lấy số lượng trạng thái request
        [HttpGet]
        [Route("employee-shift-request-tabs")]
        public IHttpActionResult EmployeeShiftRequestTabs()
        {

            var response = new ApiResult<RequestTypeResponse>
            {
                Data = new RequestTypeResponse(),
                Code = ResponseResultEnum.NoData.Value(),
                Message = ResponseResultEnum.NoData.Text(),
            };

            try
            {
                response = BoFactory.RequestFor.RequestTypes_GetAll(0);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("TimesheetTabWorkingHours Exception CompanyId: {0}, EX: {1}",
                    JwtHelper.GetCompanyIdFromToken(Request), ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }
    }
} 