using System;
using System.Collections.Generic;
using DataAccessMongo.Model.UserLoginLogCollection;

namespace DataAccessMongo.Module.UserLoginLogCollection
{
    public interface IUserLoginLog
    {
        /// <summary>
        ///     Author: CuongPK
        ///     CreateDate: 12/09/2018
        ///     Description: Insert Log Đăng nhập của user
        /// </summary>
        /// <param name="logModel"></param>
        void InsertLoginLog(UserLoginLogModel logModel);

        /// <summary>
        ///     Author: CuongPK
        ///     CreateDate: 12/09/2018
        ///     Description: Lấy Log Đăng nhập của user
        /// </summary>
        /// <returns></returns>
        List<UserLoginLogModel> GetDataUserLog(DateTime fromDate, DateTime toDate, bool isPaging, int pageIndex,
            int pageSize);
    }
}