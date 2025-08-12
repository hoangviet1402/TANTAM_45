using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Permission;
using BussinessObject.Permission;
using DataAccess;
using Logger;
using MyUtility.Extensions;
using TanTamApi.JWT.Helper;
using TanTamApi.JWT.Middleware;
using BussinessObject.Bo.TanTamBo;

namespace MobileApi.Controllers
{
    [ApiAuthorize(UserRole.SystemAdmin)]
    [RoutePrefix("api/permissions")]
    public class PermissionController : ApiController
    {
        /// <summary>
        /// Lấy cây permission (có thể lọc theo type: web/mobile)
        /// </summary>
        [HttpGet]
        [Route("list-tree")]
        public IHttpActionResult GetPermissionTree(string type)
        {
            var response = new ApiResult<List<PermissionGroupDto>>
            {
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.Success.Text(),
                Data = new List<PermissionGroupDto>()
            };

            try
            {
                // Validate type parameter
                int typeValue = PermissionHelper.GetPermissionTypeValue(type);

                if (typeValue != PermissionTypeEnum.Web.Value() && typeValue != PermissionTypeEnum.Mobile.Value())
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Type phải là 'web' hoặc 'mobile'";
                    return Ok(response);
                }

                // Xử lý đặc biệt cho mobile type
                List<PermissionGroupDto> tree;
                if (type.ToLower() == "mobile")
                {
                    tree = BoFactory.Permission.BuildPermissionTreeForMobile();
                }
                else
                {
                    tree = BoFactory.Permission.BuildPermissionTree(type);
                }
                response.Data = tree;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetPermissionTree Exception: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
            }

            return Ok(response);
        }

        /// <summary>
        /// Lấy danh sách permissionId của employee
        /// </summary>
        [HttpGet]
        [Route("list-id-available")]
        public IHttpActionResult GetEmployeePermissionIds(int employeeId, string type)
        {
            var response = new ApiResult<List<int>>
            {
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.Success.Text(),
                Data = new List<int>()
            };

            try
            {
                if (employeeId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp employeeId hợp lệ.";
                    return Ok(response);
                }

                int typeValue = PermissionHelper.GetPermissionTypeValue(type);

                if (typeValue != PermissionTypeEnum.Web.Value() && typeValue != PermissionTypeEnum.Mobile.Value())
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Type phải là 'web' hoặc 'mobile'";
                    return Ok(response);
                }

                var result = BoFactory.Permission.GetEmployeePermissionIds(employeeId, type);
                response.Data = result.Data;
                response.Message = "Lấy danh sách permissionId của employee thành công";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetEmployeePermissionIds Exception: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
            }

            return Ok(response);
        }

        /// <summary>
        /// Cập nhật quyền cho employee theo type (web/mobile)
        /// </summary>
        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateEmployeePermissions(UpdateEmployeePermissionsRequest request)
        {
            var response = new ApiResult<bool>
            {
                Data = false,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Dữ liệu gửi lên không hợp lệ.";
                    return Ok(response);
                }

                if (request.EmployeeId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp employeeId hợp lệ.";
                    return Ok(response);
                }

                int typeValue = PermissionHelper.GetPermissionTypeValue(request.Type);

                if (typeValue != PermissionTypeEnum.Web.Value() && typeValue != PermissionTypeEnum.Mobile.Value())
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Type phải là 'web' hoặc 'mobile'.";
                    return Ok(response);
                }

                response = BoFactory.Permission.UpdateEmployeePermissions(request);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("UpdateEmployeePermissions Exception: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
            }

            return Ok(response);
        }

        /// <summary>
        /// Reset quyền mặc định cho employee theo role
        /// SystemAdmin: Chặn không cho xử lý
        /// Manager: Set full quyền
        /// RegionalManager, BranchManager: Xóa hết quyền
        /// </summary>
        [HttpPost]
        [Route("reset-default-permission")]
        public IHttpActionResult ResetDefaultPermission(int employeeId, string type)
        {
            var response = new ApiResult<bool>
            {
                Data = false,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                if (employeeId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp employeeId hợp lệ.";
                    return Ok(response);
                }

                int typeValue = PermissionHelper.GetPermissionTypeValue(type);

                if (typeValue != PermissionTypeEnum.Web.Value() && typeValue != PermissionTypeEnum.Mobile.Value())
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Type phải là 'web' hoặc 'mobile'.";
                       return Ok(response);
                }
                
                response = BoFactory.Permission.ResetDefaultPermission(employeeId, typeValue);

                return Ok(response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("ResetDefaultPermission Exception: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Ok(response);
            }
        }
    }
}