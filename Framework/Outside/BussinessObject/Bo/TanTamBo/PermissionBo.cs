using BussinessObject.Models.Permission;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using MyUtility.Extensions;
using Logger;
using BussinessObject.Permission;
using EntitiesObject.Entities.TanTamEntities;

namespace BussinessObject.Bo.TanTamBo
{
    public class PermissionBo : BaseBo<DBNull>
    {
        public PermissionBo() : base(DaoFactory.Permission) { }

        public List<PermissionGroupDto> BuildPermissionTree()
        {
            return BuildPermissionTree(null);
        }

        public List<PermissionGroupDto> BuildPermissionTree(string type)
        {
            int typeValue = PermissionHelper.GetPermissionTypeValue(type);

            if (typeValue == 0)
            {
                return new List<PermissionGroupDto>();
            }

            var groupResults = DaoFactory.Permission.GetAllPermissionGroups();
            var permissionResults = DaoFactory.Permission.GetAllPermissions();

            // Mapping sang DTO
            var groups = groupResults.Select(x => new PermissionGroupDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Title = x.Title,
                Label = x.Label,
                Url = x.Url,
                Icon = x.Icon,
                ApiRouteName = x.ApiRouteName,
                IsSystem = x.IsSystem,
                SortIndex = x.SortIndex
            }).ToList();

            var permissions = permissionResults.Select(x => new PermissionDto
            {
                Id = x.Id,
                Name = x.Name,
                Key = x.Key,
                SortIndex = x.SortIndex,
                RouteName = x.RouteName,
                GroupId = x.GroupId,
                Type = x.Type
            }).ToList();

            // Lọc permissions theo type
            permissions = permissions.Where(p => p.Type == typeValue).ToList();

            // Build tree logic
            foreach (var group in groups)
            {
                group.Children = groups.Where(g => g.ParentId == group.Id).ToList();
                group.Permissions = permissions.Where(p => p.GroupId == group.Id).ToList();
            }

            // Hàm đệ quy để lọc group không có con và không có permission
            Func<PermissionGroupDto, bool> hasContent = null;
            hasContent = (group) =>
            {
                // Kiểm tra group có permission không
                bool hasPermissions = group.Permissions != null && group.Permissions.Any();
                
                // Kiểm tra group có con không (đệ quy)
                bool hasChildren = false;
                if (group.Children != null && group.Children.Any())
                {
                    // Lọc children có content
                    group.Children = group.Children.Where(child => hasContent(child)).ToList();
                    hasChildren = group.Children.Any();
                }
                
                // Group có content nếu có permission hoặc có children
                return hasPermissions || hasChildren;
            };

            // Lọc các group gốc có content
            var rootGroups = groups.Where(g => g.ParentId == null).ToList();
            return rootGroups.Where(group => hasContent(group)).ToList();
        }

        public List<PermissionGroupDto> BuildPermissionTreeForManagement(string type)
        {
            int typeValue = PermissionHelper.GetPermissionTypeValue(type);

            if (typeValue == 0)
            {
                return new List<PermissionGroupDto>();
            }

            var groupResults = DaoFactory.Permission.GetAllPermissionGroups();
            var permissionResults = DaoFactory.Permission.GetAllPermissions();

            // Mapping sang DTO
            var groups = groupResults.Select(x => new PermissionGroupDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Title = x.Title,
                Label = x.Label,
                Url = x.Url,
                Icon = x.Icon,
                ApiRouteName = x.ApiRouteName,
                IsSystem = x.IsSystem,
                SortIndex = x.SortIndex
            }).ToList();

            var permissions = permissionResults.Select(x => new PermissionDto
            {
                Id = x.Id,
                Name = x.Name,
                Key = x.Key,
                SortIndex = x.SortIndex,
                RouteName = x.RouteName,
                GroupId = x.GroupId,
                Type = x.Type
            }).ToList();

            // Lọc permissions theo type
            permissions = permissions.Where(p => p.Type == typeValue).ToList();

            // Build tree logic - hiển thị tất cả groups cho quản lý
            foreach (var group in groups)
            {
                group.Children = groups.Where(g => g.ParentId == group.Id).ToList();
                group.Permissions = permissions.Where(p => p.GroupId == group.Id).ToList();
            }

            // Trả về tất cả group gốc cho quản lý
            return groups.Where(g => g.ParentId == null).ToList();
        }

        public List<PermissionGroupDto> BuildPermissionTreeForMobile()
        {
            int typeValue = PermissionHelper.GetPermissionTypeValue("mobile");

            if (typeValue == 0)
            {
                return new List<PermissionGroupDto>();
            }

            var groupResults = DaoFactory.Permission.GetAllPermissionGroups();
            var permissionResults = DaoFactory.Permission.GetAllPermissions();

            // Mapping sang DTO
            var groups = groupResults.Select(x => new PermissionGroupDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Title = x.Title,
                Label = x.Label,
                Url = x.Url,
                Icon = x.Icon,
                ApiRouteName = x.ApiRouteName,
                IsSystem = x.IsSystem,
                SortIndex = x.SortIndex
            }).ToList();

            var permissions = permissionResults.Select(x => new PermissionDto
            {
                Id = x.Id,
                Name = x.Name,
                Key = x.Key,
                SortIndex = x.SortIndex,
                RouteName = x.RouteName,
                GroupId = x.GroupId,
                Type = x.Type
            }).ToList();

            // Lọc permissions theo mobile type
            permissions = permissions.Where(p => p.Type == typeValue).ToList();

            // Lọc groups chỉ lấy những group có routeName bắt đầu bằng "TAB_"
            groups = groups.Where(g => !string.IsNullOrEmpty(g.ApiRouteName) && g.ApiRouteName.StartsWith("TAB_")).ToList();

            // Build tree logic
            foreach (var group in groups)
            {
                group.Children = groups.Where(g => g.ParentId == group.Id).ToList();
                group.Permissions = permissions.Where(p => p.GroupId == group.Id).ToList();
            }

            // Trả về tất cả group gốc (không filter cha không có con)
            return groups.Where(g => g.ParentId == null).ToList();
        }

        public ApiResult<List<int>> GetEmployeePermissionIds(int employeeId, string type)
        {
            var response = new ApiResult<List<int>>
            {
                Data = new List<int>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                int typeValue = PermissionHelper.GetPermissionTypeValue(type);

                if (typeValue == 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Type không hợp lệ.";
                    return response;
                }

                var permissions = DaoFactory.Permission.GetEmployeePermissions(employeeId).Where(p => p.Type == typeValue).ToList();
                var listId = new List<int>();
                if (permissions != null && permissions.Any())
                {
                    if (PermissionHelper.IsSystemAdmin(employeeId))
                    {
                        listId = DaoFactory.Permission.GetAllPermissions().Where(p => p.Type == typeValue).Select(p => p.Id).ToList();
                    }
                    else
                    {
                        listId = permissions.Select(p => p.Id).ToList();
                    }

                    response.Data = listId;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy danh sách permissionId thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Nhân viên không có quyền nào.";
                }
            }
            catch (Exception ex)
            {
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }

        public ApiResult<bool> UpdateEmployeePermissions(UpdateEmployeePermissionsRequest request)
        {
            var response = new ApiResult<bool>
            {
                Data = false,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Validate input
                if (PermissionHelper.IsSystemAdmin(request.EmployeeId))
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Nhân viên là quản trị không cần cập nhật quyền.";
                    return response;
                }

                if (request.PermissionIds == null)
                {
                    request.PermissionIds = new List<int>();
                }

                // Lấy danh sách permission hợp lệ theo type
                int typeValue = PermissionHelper.GetPermissionTypeValue(request.Type);

                if (typeValue == 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Type không hợp lệ.";
                    return response;
                }

                var allPermissions = DaoFactory.Permission.GetAllPermissions();
                var validPermissionIds = new HashSet<int>(allPermissions.Where(p => p.Type == typeValue).Select(p => p.Id));

                // Lọc permissionIds gửi lên chỉ lấy id hợp lệ
                var permissionIdsToAssign = request.PermissionIds.Where(id => validPermissionIds.Contains(id)).ToList();

                // Xóa hết quyền cũ của employee theo type
                DaoFactory.Permission.DeleteEmployeePermissionsByType(request.EmployeeId, typeValue);

                // Thêm mới từng quyền hợp lệ
                foreach (var id in permissionIdsToAssign)
                {
                    DaoFactory.Permission.InsertEmployeePermission(request.EmployeeId, id);
                }

                response.Data = true;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Cập nhật quyền cho nhân viên thành công.";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("PermissionBo.UpdateEmployeePermissions - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public void UpdateEmployeePermissions(int employeeId, List<int> permissionIds, int type)
        {
            // Kiểm tra quyền admin
            bool isAdmin = PermissionHelper.IsSystemAdmin(employeeId);
            
            // Nếu là admin và type = 1 (Web), không cho phép cập nhật
            if (isAdmin && type == 1)
            {
                throw new Exception("Cannot update permissions for admin user on Web type");
            }

            // Xóa tất cả quyền hiện tại của employee theo type
            DaoFactory.Permission.DeleteEmployeePermissionsByType(employeeId, type);

            // Thêm các quyền mới
            foreach (var permissionId in permissionIds)
            {
                DaoFactory.Permission.InsertEmployeePermission(employeeId, permissionId);
            }
        }

        public ApiResult<bool> ResetDefaultPermission(int employeeId, int typeValue)
        {
            var response = new ApiResult<bool>
            {
                Data = false,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                var userRole = PermissionHelper.GetUserRole(employeeId);
                
                switch (userRole)
                {
                    case UserRole.SystemAdmin:
                        // Block processing for SystemAdmin
                        response.Code = ResponseResultEnum.Success.Value();
                        response.Message = "Nhân viên là quản trị không cần cập nhật quyền.";
                        break;

                    case UserRole.Manager:
                        // Grant full permissions to Manager
                        var allPermissions = DaoFactory.Permission.GetAllPermissions().Where(p => p.Type == typeValue).Select(p => p.Id).ToList();
                        
                        // Delete old permissions
                        DaoFactory.Permission.DeleteEmployeePermissionsByType(employeeId, typeValue);
                        
                        // Insert all permissions
                        foreach (var permissionId in allPermissions)
                        {
                            DaoFactory.Permission.InsertEmployeePermission(employeeId, permissionId);
                        }
                        
                        response.Data = true;
                        response.Code = ResponseResultEnum.Success.Value();
                        response.Message = "Đã cấp toàn bộ quyền cho Manager thành công.";
                        break;

                    case UserRole.RegionalManager:
                    case UserRole.BranchManager:
                        // Revoke all existing permissions
                        DaoFactory.Permission.DeleteEmployeePermissionsByType(employeeId, typeValue);
                        
                        response.Data = true;
                        response.Code = ResponseResultEnum.Success.Value();
                        response.Message = "Đã thu hồi toàn bộ quyền thành công.";
                        break;

                    default:
                        response.Code = ResponseResultEnum.Forbidden.Value();
                        response.Message = "Role không được phép thực hiện thao tác này.";
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("PermissionBo.ResetDefaultPermission - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public ApiResult<int> AddPermissionGroup(PermissionGroupDto dto)
        {
            var response = new ApiResult<int> { Code = ResponseResultEnum.Failed.Value(), Message = "" };
            try
            {
                int newId = DaoFactory.Permission.AddPermissionGroup(dto.ParentId, dto.Title, dto.Label, dto.Url, dto.Icon, dto.ApiRouteName, dto.IsSystem, dto.SortIndex);
                response.Data = newId;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Thêm nhóm quyền thành công.";
            }
            catch (Exception ex)
            {
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }
        public ApiResult<bool> UpdatePermissionGroup(PermissionGroupDto dto)
        {
            var response = new ApiResult<bool> { Code = ResponseResultEnum.Failed.Value(), Message = "" };
            try
            {
                DaoFactory.Permission.UpdatePermissionGroup(dto.Id, dto.ParentId, dto.Title, dto.Label, dto.Url, dto.Icon, dto.ApiRouteName, dto.IsSystem, dto.SortIndex);
                response.Data = true;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Cập nhật nhóm quyền thành công.";
            }
            catch (Exception ex)
            {
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }
        public ApiResult<bool> DeletePermissionGroup(int id)
        {
            var response = new ApiResult<bool> { Code = ResponseResultEnum.Failed.Value(), Message = "" };
            try
            {
                DaoFactory.Permission.DeletePermissionGroup(id);
                response.Data = true;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Xóa nhóm quyền thành công.";
            }
            catch (Exception ex)
            {
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }
        public ApiResult<int> AddPermission(PermissionDto dto)
        {
            var response = new ApiResult<int> { Code = ResponseResultEnum.Failed.Value(), Message = "" };
            try
            {
                int newId = DaoFactory.Permission.AddPermission(dto.GroupId, dto.Name, dto.Key, dto.RouteName, dto.SortIndex, dto.Type);
                response.Data = newId;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Thêm quyền thành công.";
            }
            catch (Exception ex)
            {
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }
        public ApiResult<bool> UpdatePermission(PermissionDto dto)
        {
            var response = new ApiResult<bool> { Code = ResponseResultEnum.Failed.Value(), Message = "" };
            try
            {
                // Kiểm tra key mới đã tồn tại cho quyền khác chưa
                var allPermissions = DaoFactory.Permission.GetAllPermissions();
                if (allPermissions.Any(p => p.Key == dto.Key && p.Id != dto.Id))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Key quyền đã tồn tại, vui lòng chọn key khác!";
                    return response;
                }

                DaoFactory.Permission.UpdatePermission(dto.Id, dto.GroupId, dto.Name, dto.Key, dto.RouteName, dto.SortIndex, dto.Type);
                response.Data = true;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Cập nhật quyền thành công.";
            }
            catch (Exception ex)
            {
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }
        public ApiResult<bool> DeletePermission(int id)
        {
            var response = new ApiResult<bool> { Code = ResponseResultEnum.Failed.Value(), Message = "" };
            try
            {
                DaoFactory.Permission.DeletePermission(id);
                response.Data = true;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Xóa quyền thành công.";
            }
            catch (Exception ex)
            {
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }

        // CRUD RolePermission
        public List<RolePermissionDto> GetRolePermissions(int roleId)
        {
            var result = DaoFactory.RolePermission.GetRolePermissions(roleId);
            return result.Select(x => new RolePermissionDto
            {
                RoleId = x.RoleId,
                PermissionId = x.PermissionId,
                PermissionKey = x.PermissionKey,
                PermissionName = x.PermissionName
            }).ToList();
        }

        public List<RolePermissionDto> GetRolePermissionsByType(int roleId, int type)
        {
            var result = DaoFactory.RolePermission.GetRolePermissionsByType(roleId, type);
            return result.Select(x => new RolePermissionDto
            {
                RoleId = x.RoleId,
                PermissionId = x.PermissionId,
                PermissionKey = x.PermissionKey,
                PermissionName = x.PermissionName
            }).ToList();
        }

        public ApiResult<bool> AddRolePermission(int roleId, int permissionId)
        {
            var response = new ApiResult<bool> { Data = false, Code = ResponseResultEnum.Failed.Value(), Message = "" };
            try
            {
                DaoFactory.RolePermission.AddRolePermission(roleId, permissionId);
                response.Data = true;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Thêm quyền mặc định cho role thành công.";
            }
            catch (Exception ex)
            {
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }

        public ApiResult<bool> DeleteRolePermission(int roleId, int permissionId)
        {
            var response = new ApiResult<bool> { Data = false, Code = ResponseResultEnum.Failed.Value(), Message = "" };
            try
            {
                DaoFactory.RolePermission.DeleteRolePermission(roleId, permissionId);
                response.Data = true;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Xóa quyền mặc định của role thành công.";
            }
            catch (Exception ex)
            {
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }

        public ApiResult<bool> DeleteAllRolePermissions(int roleId)
        {
            var response = new ApiResult<bool> { Data = false, Code = ResponseResultEnum.Failed.Value(), Message = "" };
            try
            {
                DaoFactory.RolePermission.DeleteAllRolePermissions(roleId);
                response.Data = true;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Xóa tất cả quyền mặc định của role thành công.";
            }
            catch (Exception ex)
            {
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }
    }
}