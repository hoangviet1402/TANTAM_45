using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace BussinessObject.Bo.Shift
{
    public class ShiftBo : BaseBo<DBNull>
    {
        public ShiftBo()
            : base(DaoFactory.Shift)
        {
        }
    }
}
