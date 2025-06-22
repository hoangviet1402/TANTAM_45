using DataAccess;
using System;

namespace BussinessObject.Bo.TanTamBo
{
    public class TanTamBo : BaseBo<DBNull>
    {
        public TanTamBo()
            : base(DaoFactory.TanTam)
        {
        }
    }
}