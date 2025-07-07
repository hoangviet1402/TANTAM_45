using DataAccess.EF;
using DataAccess.Interface;
using DataAccess.Model.Shift;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;


namespace DataAccess.Dao.Shift
{
    public interface IWifiDao : IBaseFactories<DBNull>
    {
        int CreateWifi(int radius,int speed,int accuracy, int altitude, float longitude, float latitude,string name, int branchID, string address, int type);
        int CreateWifiAccount(int wifiID, int accountMapID);
        int CreateWifiDepartment(int wifiID, int departmentID);
        /// <summary>
        /// lấy danh sách wifi, phải truyền ít nhất 1 param > 0, param = 0 sẽ bỏ qua ko search
        /// </summary>
        List<Ins_Wifi_Get_Result> WifiGet(int wifiID, int branchID, int departmentID, int accountMapID);

    }
    internal class WifiDao : DaoFactories<TanTamEntities, DBNull>, IWifiDao
    {
        public int CreateWifi(int radius, int speed, int accuracy, int altitude, float longitude, float latitude, string name, int branchID, string address, int type)
        {
            using (Uow)
            {
                var outResult = 0;

                var out_OutResult = new ObjectParameter("WifiID", typeof(int));

                var data = Uow.Context.Ins_Wifi_Insert(
                    radius, speed, accuracy, altitude, longitude, latitude, name, branchID, address, type, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

        public int CreateWifiAccount(int wifiID, int accountMapID)
        {
            using (Uow)
            {
                var outResult = 0;

                var out_OutResult = new ObjectParameter("WifiAccountID", typeof(int));

                var data = Uow.Context.Ins_Wifi_Account_Insert(wifiID, accountMapID, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

        public int CreateWifiDepartment(int wifiID, int departmentID)
        {
            using (Uow)
            {
                var outResult = 0;

                var out_OutResult = new ObjectParameter("WifiDepartmentID", typeof(int));

                var data = Uow.Context.Ins_Wifi_Department_Insert(wifiID, departmentID, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

        public List<Ins_Wifi_Get_Result> WifiGet(int wifiID, int branchID, int departmentID, int accountMapID)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Wifi_Get(wifiID, branchID, departmentID,accountMapID);
                return data.ToList();
            }
        }
    }
}
