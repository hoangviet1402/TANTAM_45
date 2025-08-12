using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.RequestFor;
using DataAccess;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BussinessObject.Bo.TanTamBo
{
    public class BuildingBo : BaseBo<DBNull>
    {
        public BuildingBo()
            : base(DaoFactory.TanTam)
        {
        }
        public ApiResult<List<ListDeviceEspResponse>> Device_GetByControllerESP(int companyId, string MAC)
        {
            var response = new ApiResult<List<ListDeviceEspResponse>>
            {
                Data = new List<ListDeviceEspResponse>(),
                Code = ResponseResultEnum.NoData.Value(),
                Message = ResponseResultEnum.NoData.Text(),
            };
            var data = DaoFactory.Building.Device_GetByControllerESP(companyId, MAC);
            response.Data = data.Select(x => new ListDeviceEspResponse()
            {
                DeviceStatus = x.DeviceStatus ?? 0,
                GPIO = x.GPIOPin ?? 0
            }).ToList();

            return response;
        }
    }
}