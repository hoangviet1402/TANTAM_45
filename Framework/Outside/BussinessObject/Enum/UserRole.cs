using System.ComponentModel;

namespace BussinessObject.Enum
{
    /// <summary>
    /// Enum định nghĩa các vai trò người dùng trong hệ thống
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// tài khoảnđầu tiên tạo tài khoản cho công ty
        /// </summary>
        [Description("Quản lý")]
        SystemAdmin = 1,

        /// <summary>
        /// quản lý công ty
        /// </summary>
        [Description("Quản lý")]
        Manager = 2,

        /// <summary>
        /// quản lý vùng
        /// </summary>
        [Description("Quản lý vùng")]
        RegionalManager = 3,

        /// <summary>
        /// quản lý chi nhánh
        /// </summary>
        [Description("Quản lý chi nhánh")]
        BranchManager = 4,

        /// <summary>
        /// quyền măc định khi thêm nhân viên
        /// </summary>
        [Description("Nhân viên")]
        Employees = 10
    }
}