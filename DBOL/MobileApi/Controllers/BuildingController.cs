using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.RequestFor;
using Logger;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using TanTamApi.JWT.Helper;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/building")]
    public class BuildingController : ApiController
    {

        [HttpGet, Route("list-device-esp")]
        public IHttpActionResult ListDeviceEsp([FromUri] string mac)
        {
            var response = new ApiResult<List<ListDeviceEspResponse>>
            {
                Data = new List<ListDeviceEspResponse>(),
                Code = ResponseResultEnum.NoData.Value(),
                Message = ResponseResultEnum.NoData.Text(),
            };

            try
            {
                CommonLogger.DefaultLogger.DebugFormat("list-device-esp : {0}", mac);
                response = BoFactory.Building.Device_GetByControllerESP(0, mac);
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
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
