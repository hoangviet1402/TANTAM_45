namespace BussinessObject.Enum
{
    /// <summary>
    /// Enum định nghĩa các trạng thái của nhân viên
    /// </summary>
    public enum EmployeeStatusEnum
    {
        /// <summary>
        /// Đang hoạt động
        /// </summary>
        Active = 1,

        /// <summary>
        /// Đã nghỉ việc
        /// </summary>
        IsQuit = 2,

        /// <summary>
        /// Ngưng hoạt động
        /// </summary>
        InActive = 3,

        /// <summary>
        /// Chưa làm việc
        /// </summary>
        NotWorking = 4,

        /// <summary>
        /// Lấy tất cả
        /// </summary>
        All = -99,
    }
}