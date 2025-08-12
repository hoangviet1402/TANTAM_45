using DataAccess;
using BussinessObject.Enum;
using DataAccess.Dao.TanTam;
using System.Collections.Generic;
using System.Linq;

namespace BussinessObject.Permission
{
    public static class PermissionHelper
    {
        public static bool HasPermission(int employeeId, string permissionKey, int role)
        {
            if (role == (int)UserRole.SystemAdmin)
                return true;

            // // Xử lý đặc biệt cho role Employees
            // if (role == (int)UserRole.Employees)
            // {
            //     // Chỉ check quyền thực tế của employee từ EmployeePermission
            //     return DaoFactory.Permission.CheckEmployeePermission(employeeId, permissionKey);
            // }

            return DaoFactory.Permission.CheckEmployeePermission(employeeId, permissionKey);
        }

        public static int GetPermissionTypeValue(string type)
        {
            switch (type.ToLower())
            {
                case "web":
                    return (int)PermissionTypeEnum.Web;
                case "mobile":
                    return (int)PermissionTypeEnum.Mobile;
                default:
                    return 0; // Trả về 0 nếu không match
            }
        }

        public static UserRole GetUserRole(int employeeId)
        {
            var role = DaoFactory.Permission.GetEmployeeRole(employeeId);
            return (UserRole)role;
        }

        public static bool IsSystemAdmin(int employeeId)
        {
            var role = GetUserRole(employeeId);
            return role == UserRole.SystemAdmin;
        }

        /// <summary>
        /// Kiểm tra và trả về danh sách các quyền hợp lệ từ danh sách quyền đầu vào
        /// </summary>
        /// <param name="employeeId">ID của nhân viên</param>
        /// <param name="permissionKeys">Danh sách các key quyền cần kiểm tra</param>
        /// <param name="role">Role của user</param>
        /// <returns>Danh sách các quyền hợp lệ</returns>
        public static List<string> GetValidPermissions(int employeeId, List<string> permissionKeys, int role)
        {
            // Nếu là SystemAdmin thì trả về tất cả quyền
            if (role == (int)UserRole.SystemAdmin)
                return permissionKeys;

            // Lấy tất cả quyền của employee (cho tất cả roles)
            var userPermissions = DaoFactory.Permission.GetEmployeePermissions(employeeId);
            var userPermissionKeys = userPermissions.Select(x => x.Key).ToList();

            // Trả về các quyền có trong danh sách đầu vào và có trong quyền của user
            return permissionKeys.Where(key => userPermissionKeys.Contains(key)).ToList();
        }

        /// <summary>
        /// Kiểm tra và trả về danh sách các quyền hợp lệ từ danh sách quyền đầu vào (overload với role từ employeeId)
        /// </summary>
        /// <param name="employeeId">ID của nhân viên</param>
        /// <param name="permissionKeys">Danh sách các key quyền cần kiểm tra</param>
        /// <returns>Danh sách các quyền hợp lệ</returns>
        public static List<string> GetValidPermissions(int employeeId, List<string> permissionKeys)
        {
            var role = DaoFactory.Permission.GetEmployeeRole(employeeId);
            return GetValidPermissions(employeeId, permissionKeys, role);
        }
    }
} 