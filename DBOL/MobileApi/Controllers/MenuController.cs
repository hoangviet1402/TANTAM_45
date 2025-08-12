using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using MyUtility.Extensions;
using TanTamApi.Models;
using TanTamApi.JWT.Middleware;
using TanTamApi.JWT.Helper;
using BussinessObject;
using BussinessObject.Models.Menu;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/menu")]
    public class MenuController : ApiController
    {
        /// <summary>
        /// Lấy danh sách menu dạng tree
        /// </summary>
        [ApiAuthorize]
        [HttpGet]
        [Route("list-tree")]
        public IHttpActionResult GetMenuTree()
        {
            var response = new ApiResult<List<MenuDto>>
            {
                Data = new List<MenuDto>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };
            try
            {
                int roleInt = JwtHelper.GetRoleFromToken(Request);
                if (roleInt <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidToken.Value();
                    response.Message = "Không xác định được quyền từ token.";
                    return Content(HttpStatusCode.Unauthorized, response);
                }

                // Lấy menu tree từ BO
                var menuTree = BoFactory.Menu.GetMenuTreeByRole(roleInt);
                response.Data = menuTree;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy danh sách menu thành công";
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = $"Lỗi hệ thống: {ex.Message}";
                return Content(HttpStatusCode.OK, response);
            }
        }
    }
}