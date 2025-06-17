namespace EntitiesObject.Message
{
    /// <summary>
    ///     Ma loi su dung chung cho tat ca API
    /// </summary>
    public enum CommonCode
    {
        Success = 0,

        /// <summary>
        ///     Loi sai du lieu input
        /// </summary>
        InvalidData = 1000,

        /// <summary>
        ///     Loi phat sinh trong he thong
        /// </summary>
        SystemError = 1001,

        /// <summary>
        ///     Ssai chu ky dien tu
        /// </summary>
        InvalidSign = 1002,

        /// <summary>
        ///     Token het han
        /// </summary>
        TokenExpire = 1003,

        /// <summary>
        ///     Dang xu ly
        /// </summary>
        Processing = 1004,

        /// <summary>
        ///     lỗi dưới store
        /// </summary>
        StoreError = 1005
    }
}