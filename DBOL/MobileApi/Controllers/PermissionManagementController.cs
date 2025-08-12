using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Permission;
using BussinessObject.Permission;
using Logger;
using MyUtility.Extensions;
using BussinessObject.Models.Menu;

namespace MobileApi.Controllers
{
    [RoutePrefix("api/permission-management")]
    public class PermissionManagementController : ApiController
    {
        // --- CLONED FROM PermissionController ---
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
                int typeValue = PermissionHelper.GetPermissionTypeValue(type);
                if (typeValue != PermissionTypeEnum.Web.Value() && typeValue != PermissionTypeEnum.Mobile.Value())
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Type phải là 'web' hoặc 'mobile'";
                    return Ok(response);
                }
                // Sử dụng method mới để hiển thị tất cả groups cho quản lý
                var tree = BoFactory.Permission.BuildPermissionTreeForManagement(type);
                response.Data = tree;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"GetPermissionTree Exception: {ex.Message}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = $"Lỗi hệ thống: {ex.Message}";
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("list-tree-filtered")]
        public IHttpActionResult GetPermissionTreeFiltered(string type)
        {
            var response = new ApiResult<List<PermissionGroupDto>>
            {
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.Success.Text(),
                Data = new List<PermissionGroupDto>()
            };

            try
            {
                int typeValue = PermissionHelper.GetPermissionTypeValue(type);
                if (typeValue != PermissionTypeEnum.Web.Value() && typeValue != PermissionTypeEnum.Mobile.Value())
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Type phải là 'web' hoặc 'mobile'";
                    return Ok(response);
                }
                // Sử dụng method đã lọc để ẩn group cha không có con
                var tree = BoFactory.Permission.BuildPermissionTree(type);
                response.Data = tree;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"GetPermissionTreeFiltered Exception: {ex.Message}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = $"Lỗi hệ thống: {ex.Message}";
            }
            return Ok(response);
        }

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

        // --- PLACEHOLDER: CRUD API cho PermissionGroup và Permission (add, update, delete) ---
        // TODO: Thêm các API sau:
        [HttpPost]
        [Route("group-add")]
        public IHttpActionResult AddPermissionGroup([FromBody] PermissionGroupDto dto)
        {
            var result = BoFactory.Permission.AddPermissionGroup(dto);
            return Ok(result);
        }
        [HttpPost]
        [Route("group-update")]
        public IHttpActionResult UpdatePermissionGroup([FromBody] PermissionGroupDto dto)
        {
            var result = BoFactory.Permission.UpdatePermissionGroup(dto);
            return Ok(result);
        }
        [HttpPost]
        [Route("group-delete")]
        public IHttpActionResult DeletePermissionGroup([FromBody] int id)
        {
            var result = BoFactory.Permission.DeletePermissionGroup(id);
            return Ok(result);
        }
        [HttpPost]
        [Route("permission-add")]
        public IHttpActionResult AddPermission([FromBody] PermissionDto dto)
        {
            var result = BoFactory.Permission.AddPermission(dto);
            return Ok(result);
        }
        [HttpPost]
        [Route("permission-update")]
        public IHttpActionResult UpdatePermission([FromBody] PermissionDto dto)
        {
            var result = BoFactory.Permission.UpdatePermission(dto);
            return Ok(result);
        }
        [HttpPost]
        [Route("permission-delete")]
        public IHttpActionResult DeletePermission([FromBody] int id)
        {
            var result = BoFactory.Permission.DeletePermission(id);
            return Ok(result);
        }

        // CRUD RolePermission
        [HttpGet]
        [Route("role-permission-list")]
        public IHttpActionResult GetRolePermissions(int roleId)
        {
            var result = BoFactory.Permission.GetRolePermissions(roleId);
            return Ok(new ApiResult<List<RolePermissionDto>>
            {
                Data = result,
                Code = ResponseResultEnum.Success.Value(),
                Message = "Lấy danh sách quyền mặc định của role thành công."
            });
        }

        [HttpGet]
        [Route("role-permission-list-by-type")]
        public IHttpActionResult GetRolePermissionsByType(int roleId, int type)
        {
            var result = BoFactory.Permission.GetRolePermissionsByType(roleId, type);
            return Ok(new ApiResult<List<RolePermissionDto>>
            {
                Data = result,
                Code = ResponseResultEnum.Success.Value(),
                Message = $"Lấy danh sách quyền mặc định của role {roleId} theo type {type} thành công."
            });
        }

        [HttpPost]
        [Route("role-permission-add")]
        public IHttpActionResult AddRolePermission([FromBody] RolePermissionDto dto)
        {
            var result = BoFactory.Permission.AddRolePermission(dto.RoleId, dto.PermissionId);
            return Ok(result);
        }

        [HttpPost]
        [Route("role-permission-delete")]
        public IHttpActionResult DeleteRolePermission([FromBody] RolePermissionDto dto)
        {
            var result = BoFactory.Permission.DeleteRolePermission(dto.RoleId, dto.PermissionId);
            return Ok(result);
        }

        [HttpPost]
        [Route("role-permission-delete-all")]
        public IHttpActionResult DeleteAllRolePermissions([FromBody] int roleId)
        {
            var result = BoFactory.Permission.DeleteAllRolePermissions(roleId);
            return Ok(result);
        }

        [HttpGet]
        [Route("menu-tree")]
        public IHttpActionResult GetMenuTree(int roleId)
        {
            var menuTree = BoFactory.Menu.GetMenuTreeByRole(roleId);
            return Ok(new ApiResult<List<MenuDto>>
            {
                Data = menuTree,
                Code = ResponseResultEnum.Success.Value(),
                Message = "Lấy menu thành công"
            });
        }

        [HttpGet]
        [Route("menu-tree-all")]
        public IHttpActionResult GetAllMenuTree()
        {
            var menuTree = BoFactory.Menu.GetAllMenuTree();
            return Ok(new ApiResult<List<MenuDto>>
            {
                Data = menuTree,
                Code = ResponseResultEnum.Success.Value(),
                Message = "Lấy toàn bộ menu thành công"
            });
        }

        [HttpPost]
        [Route("menu-add")]
        public IHttpActionResult AddMenu(MenuDto dto)
        {
            BoFactory.Menu.AddMenu(dto);
            return Ok(new { success = true });
        }

        [HttpPost]
        [Route("menu-update")]
        public IHttpActionResult UpdateMenu(MenuDto dto)
        {
            BoFactory.Menu.UpdateMenu(dto);
            return Ok(new { success = true });
        }

        [HttpPost]
        [Route("menu-delete")]
        public IHttpActionResult DeleteMenu(int id)
        {
            BoFactory.Menu.DeleteMenu(id);
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("menu-role-list")]
        public IHttpActionResult GetMenuRole(int roleId)
        {
            var menuIds = BoFactory.Menu.GetMenuIdsByRole(roleId);
            return Ok(menuIds);
        }

        // Thêm class DTO cho menu role
        public class MenuRoleDto
        {
            public int MenuId { get; set; }
            public int RoleId { get; set; }
        }

        [HttpPost]
        [Route("menu-role-add")]
        public IHttpActionResult AddMenuRole([FromBody] MenuRoleDto dto)
        {
            BoFactory.Menu.AddMenuRole(dto.MenuId, dto.RoleId);
            return Ok(new { success = true });
        }

        [HttpPost]
        [Route("menu-role-delete")]
        public IHttpActionResult DeleteMenuRole([FromBody] MenuRoleDto dto)
        {
            BoFactory.Menu.DeleteMenuRole(dto.MenuId, dto.RoleId);
            return Ok(new { success = true });
        }
    }
} 