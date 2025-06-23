using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Dao.TanTamDao
{
    /// <summary>
    /// Interface for User data access operations
    /// </summary>
    public interface IUserDao : IBaseFactories<DBNull>
    {
        List<Ins_User_GetList_Result> GetUserList(int? page, int? limit, string search, bool? status, int? departmentId, int? roleId);
        Ins_User_GetDetail_Result GetUserDetail(int userId, int companyId);
    }

    /// <summary>
    /// Implementation of User data access operations
    /// </summary>
    internal class UserDao : DaoFactories<TanTamEntities, DBNull>, IUserDao
    {
        public List<Ins_User_GetList_Result> GetUserList(int? page, int? limit, string search, bool? status, int? departmentId, int? roleId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_User_GetList(page, limit, search, status, departmentId, roleId).ToList();
            }
        }

        public Ins_User_GetDetail_Result GetUserDetail(int userId, int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_User_GetDetail(userId, companyId).FirstOrDefault();
            }
        }
    }
} 