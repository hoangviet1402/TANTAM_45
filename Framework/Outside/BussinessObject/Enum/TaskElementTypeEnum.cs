namespace BussinessObject.Enum
{
    /// <summary>
    /// Enum cho các loại field type trong task
    /// </summary>
    public enum TaskFieldEnum
    {
        /// <summary>
        /// Priority - Độ ưu tiên
        /// </summary>
        priority_id = 1,

        /// <summary>
        /// AssignedId - Người phụ trách
        /// </summary>
        assigned_id = 2,

        /// <summary>
        /// Deadline - Ngày hết hạn
        /// </summary>
        deadline = 3,

        /// <summary>
        /// label_ids - Nhãn
        /// </summary>
        label_ids = 4,

        /// <summary>
        /// created_user_id - Người tạo
        /// </summary>
        created_user_id = 5,

        /// <summary>
        /// created_at - Ngày tạo
        /// </summary>
        created_at = 6,

        /// <summary>
        /// collaborators - cộng tác viên
        /// </summary>
        collaborators = 7,
    }

    public enum TaskFieldElementTypeEnum
    {
        dropdown = 1,
        text = 2,
        number = 3,
    }
}
