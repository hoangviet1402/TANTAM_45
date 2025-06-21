using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EF;
using DataAccess.Interface;

namespace DataAccess.Dao.Shift
{
    public interface IShiftDao : IBaseFactories<DBNull>
    {
    }

    internal class ShiftDao : DaoFactories<TanTamEntities, DBNull>, IShiftDao
    {

    }
}
