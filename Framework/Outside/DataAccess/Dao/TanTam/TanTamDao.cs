using DataAccess.EF;
using DataAccess.Interface;
using System;

namespace DataAccess.Dao.TanTamDao
{
    /// <summary>
    ///     Sao khi dang ky thanh cong, lan dau tien dang nhan se duoc tang x Xu
    /// </summary>
    public interface ITanTamDao : IBaseFactories<DBNull>
    {
       
    }

    internal class TanTamDao : DaoFactories<TanTamEntities, DBNull>, ITanTamDao
    {
        
    }
}