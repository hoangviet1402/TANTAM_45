using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace BussinessObject.Bo.Shift
{
    public class ShiftAssignmentBo : BaseBo<DBNull>
    {
        public ShiftAssignmentBo()
            : base(DaoFactory.ShiftAssignment)
        {
        }
    }
}
