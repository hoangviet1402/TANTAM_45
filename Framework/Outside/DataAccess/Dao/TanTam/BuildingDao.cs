using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccess.Dao.TanTamDao
{
    public interface IBuildingDao : IBaseFactories<DBNull>
    {
        List<Ins_Device_GetByControllerESP_Result> Device_GetByControllerESP(int CompanyId, string MAC);
    }

    internal class BuildingDao : DaoFactories<TanTamEntities, DBNull>, IBuildingDao
    {
        public List<Ins_Device_GetByControllerESP_Result> Device_GetByControllerESP(int companyId, string MAC)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Device_GetByControllerESP(companyId, MAC);
                return result.ToList();
            }
        }

    }
} 