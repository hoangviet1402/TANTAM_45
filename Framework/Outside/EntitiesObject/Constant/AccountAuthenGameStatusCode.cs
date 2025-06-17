namespace EntitiesObject.Constant
{
    /// <summary>
    ///     Ma loi su dung chung cho tat ca API
    /// </summary>
    public enum AccountAuthenGameStatusCode
    {
        /// <summary>
        ///     Khởi tạo
        /// </summary>
        Create = 0,

        /// <summary>
        ///     Đã xác thực trước đó
        /// </summary>
        AuthenBefore = 1,

        /// <summary>
        ///     thành công
        /// </summary>
        Success = 2
    }
}