using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Shift;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TanTamApi.JWT.Helper;
using TanTamApi.JWT.Middleware;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/time-keeper")]
    public class TimeKeeperController : ApiController
    {
        [ApiAuthorize]
        [HttpPost,Route("list-timekeeper-log")]
        public HttpResponseMessage ListTimekeeperLog([FromBody] ListTimekeeperLogRequest request)
        {
            var response = new ApiResult<ListTimekeeperLogReponse>()
            {
                Data = new ListTimekeeperLogReponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);
                DateTime dateFrom = DateTime.Now.GetBeginOfDay();
                DateTime dateTo = DateTime.Now.GetEndOfDay();

                if (string.IsNullOrEmpty(request.from_date) == false && string.IsNullOrEmpty(request.to_date) == false)
                {
                    dateFrom = DateTime.ParseExact(
                        request.from_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );

                    dateTo = DateTime.ParseExact(
                    request.to_date,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture
                );
                }

                response = BoFactory.Timekeeper.Timekeeper_log_GetListByAccountMapID(companyId, employeeId , dateFrom, dateTo);
                CommonLogger.PerformanceLogger.DebugFormat("ListTimekeeperLog response {0}", JsonConvert.SerializeObject(response));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ListTimekeeperLog Exception.", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost,Route("get-checked-time-employee-shift-v2")]
        public HttpResponseMessage ListTimekeeperLogV2([FromBody] ListTimekeeperLogRequestV2 request)
        {
            var response = new ApiResult<ListTimekeeperLogReponse>()
            {
                Data = new ListTimekeeperLogReponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);

                response = BoFactory.Timekeeper.Timekeeper_log_GetListByAccountMapID_v2(companyId, request.employee_id, request.employee_shift_id);
                CommonLogger.PerformanceLogger.DebugFormat("ListTimekeeperLog response {0}", JsonConvert.SerializeObject(response));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ListTimekeeperLog Exception.", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}