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
using System.Net.Http;
using MyUtility;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/time-keeper")]
    public class TimeKeeperController : ApiController
    {
        [TanTamApi.JWT.Middleware.Authorize]
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
    }
}