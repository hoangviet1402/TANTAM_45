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

        /// <summary>
        /// Tạo WiFi sử dụng stored procedure Ins_Wifi_Create
        /// </summary>
        List<Ins_Wifi_Create_Result> CreateWifiWithResult(int radius, int speed, int accuracy, int altitude, double longitude, double latitude, string name, int branchID, string address, string type, bool isBothBssidSsid = false, string bssid = "");
        int CreateWifiAccount(int wifiID, int accountMapID);
        int CreateWifiDepartment(int wifiID, int departmentID);
        int CreateWifiExtraBranch(int wifiID, int branchID);
        /// <summary>
        /// lấy danh sách wifi, phải truyền ít nhất 1 param > 0, param = 0 sẽ bỏ qua ko search
        /// </summary>
        List<Ins_Wifi_Get_Result> WifiGet(int wifiID, int branchID, int departmentID, int accountMapID);
        /// <summary>
        /// lấy danh sách wifi theo company ID
        /// </summary>
        List<Ins_Wifi_Get_ByCompanyId_Result> WifiGetByCompanyId(int companyID, string type);
        /// <summary>
        /// lấy thông tin branch theo wifi ID và branch ID
        /// </summary>
        List<Ins_Wifi_Branch_Get_ByWifiId_And_BranchId_Result> WifiBranchGetByWifiIdAndBranchId(int wifiId, int branchId);
        /// <summary>
        /// lấy thông tin department theo wifi ID và department ID
        /// </summary>
        /// <summary>
        /// Cập nhật WiFi theo WiFi ID
        /// </summary>
        List<Ins_Wifi_Update_ByWifiId_Result> UpdateWifiByWifiId(int wifiId, int radius, int speed, int accuracy, int altitude, double longitude, double latitude, string wifiName, int branchId, string wifiAddress, string wifiType, string bssid, bool isBothBssidSsid = false);

        /// <summary>
        /// Lấy danh sách department IDs theo WiFi ID
        /// </summary>
        List<int> GetWifiDepartmentIds(int wifiId);

        /// <summary>
        /// Xóa tất cả department associations theo WiFi ID
        /// </summary>
        int DeleteWifiDepartments(int wifiId);

        /// <summary>
        /// Lấy danh sách extra branch IDs theo WiFi ID
        /// </summary>
        List<int> GetWifiExtraBranchIds(int wifiId);

        /// <summary>
        /// Xóa tất cả extra branch associations theo WiFi ID
        /// </summary>
        int DeleteWifiExtraBranches(int wifiId);

        /// <summary>
        /// Lấy dữ liệu WiFi mới sau khi cập nhật
        /// </summary>
        Ins_Wifi_Get_Result GetWifiById(int wifiId);

        /// <summary>
        /// Xóa extra branch association theo WiFi ID và Branch ID cụ thể
        /// </summary>
        int DeleteWifiExtraBranchByWifiIdAndBranchId(int wifiId, int branchId);

        /// <summary>
        /// Xóa department association theo WiFi ID và Department ID cụ thể
        /// </summary>
        int DeleteWifiDepartmentByWifiIdAndDepartmentId(int wifiId, int departmentId);

        /// <summary>
        /// Xóa WiFi theo WiFi ID
        /// </summary>
        int DeleteWifiByWifiId(int wifiId);

        /// <summary>
        /// Lấy danh sách WiFi với thông tin đầy đủ theo Company ID
        /// </summary>
        List<Ins_Wifi_Get_ByCompanyId_Result> GetWifiListWithFullInfo(int companyId, string type = "wifi");

        /// <summary>
        /// Lấy danh sách WiFi sử dụng Ins_Wifi_Get_ByCompanyId (temporary)
        /// </summary>
        List<Ins_Wifi_Get_ByCompanyId_Result> GetWifiListUsingCreatePattern(int companyId, string type = "wifi");

        /// <summary>
        /// Lấy thông tin department theo WiFi ID sử dụng Ins_Wifi_Department_Get_ByWifiId_Value
        /// </summary>
        List<Ins_Wifi_Department_Get_ByWifiId_Value_Result> GetWifiDepartmentsByWifiId(int wifiId);

        /// <summary>
        /// Lấy thông tin extra branch theo WiFi ID sử dụng Ins_Wifi_ExtraBranch_Get_ByWifiId_Value
        /// </summary>
        List<Ins_Wifi_ExtraBranch_Get_ByWifiId_Value_Result> GetWifiExtraBranchesByWifiId(int wifiId);

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

        public List<Ins_Wifi_Create_Result> CreateWifiWithResult(int radius, int speed, int accuracy, int altitude, double longitude, double latitude, string name, int branchID, string address, string type, bool isBothBssidSsid = false, string bssid = "")
        {
            using (Uow)
            {
                var out_WifiID = new ObjectParameter("WifiID", typeof(int));
                
                var data = Uow.Context.Ins_Wifi_Create(
                    radius, speed, accuracy, altitude, longitude, latitude, name, branchID, address, type, isBothBssidSsid, bssid, out_WifiID);
                
                return data?.ToList() ?? new List<Ins_Wifi_Create_Result>();
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

        public int CreateWifiExtraBranch(int wifiID, int branchID)
        {
            using (Uow)
            {
                var outResult = 0;

                var out_OutResult = new ObjectParameter("WifiBranchID", typeof(int));

                var data = Uow.Context.Ins_Wifi_ExtraBranch_Insert(wifiID, branchID, out_OutResult);

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

        public List<Ins_Wifi_Get_ByCompanyId_Result> WifiGetByCompanyId(int companyID, string type)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Wifi_Get_ByCompanyId(companyID, type);
                return data.ToList();
            }
        }

        public List<Ins_Wifi_Branch_Get_ByWifiId_And_BranchId_Result> WifiBranchGetByWifiIdAndBranchId(int wifiId, int branchId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Wifi_Branch_Get_ByWifiId_And_BranchId(wifiId, branchId);
                return data.ToList();
            }
        }

        public List<Ins_Wifi_Update_ByWifiId_Result> UpdateWifiByWifiId(int wifiId, int radius, int speed, int accuracy, int altitude, double longitude, double latitude, string wifiName, int branchId, string wifiAddress, string wifiType, string bssid, bool isBothBssidSsid = false)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Wifi_Update_ByWifiId(
                    wifiId, radius, speed, accuracy, altitude, longitude, latitude, 
                    wifiName, branchId, wifiAddress, wifiType, isBothBssidSsid, bssid);
                
                return data?.ToList() ?? new List<Ins_Wifi_Update_ByWifiId_Result>();
            }
        }

        public List<int> GetWifiDepartmentIds(int wifiId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Wifi_Department_Get_ByWifiId(wifiId);
                return data?.Where(x => x.HasValue).Select(x => x.Value).ToList() ?? new List<int>();
            }
        }

        public int DeleteWifiDepartments(int wifiId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Wifi_Department_Delete_ByWifiId(wifiId);
            }
        }

        public List<int> GetWifiExtraBranchIds(int wifiId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Wifi_ExtraBranch_Get_ByWifiId(wifiId);
                return data?.Where(x => x.HasValue).Select(x => x.Value).ToList() ?? new List<int>();
            }
        }

        public int DeleteWifiExtraBranches(int wifiId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Wifi_ExtraBranch_Delete_ByWifiId(wifiId);
            }
        }

        public Ins_Wifi_Get_Result GetWifiById(int wifiId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Wifi_Get(wifiId, 0, 0, 0);
                return data?.FirstOrDefault();
            }
        }

        public int DeleteWifiExtraBranchByWifiIdAndBranchId(int wifiId, int branchId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Wifi_ExtraBranch_Delete_ByWifiId_And_BranchId(wifiId, branchId);
            }
        }

        public int DeleteWifiDepartmentByWifiIdAndDepartmentId(int wifiId, int departmentId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Wifi_Department_Delete_ByWifiId_And_DepartmentId(wifiId, departmentId);
            }
        }

        public int DeleteWifiByWifiId(int wifiId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Wifi_Delete_ByWifiId(wifiId);
            }
        }

        public List<Ins_Wifi_Get_ByCompanyId_Result> GetWifiListWithFullInfo(int companyId, string type = "wifi")
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Wifi_Get_ByCompanyId(companyId, type);
                return data?.ToList() ?? new List<Ins_Wifi_Get_ByCompanyId_Result>();
            }
        }

        public List<Ins_Wifi_Get_ByCompanyId_Result> GetWifiListUsingCreatePattern(int companyId, string type = "wifi")
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Wifi_Get_ByCompanyId(companyId, type);
                
                return data?.ToList() ?? new List<Ins_Wifi_Get_ByCompanyId_Result>();
            }
        }

        public List<Ins_Wifi_Department_Get_ByWifiId_Value_Result> GetWifiDepartmentsByWifiId(int wifiId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Wifi_Department_Get_ByWifiId_Value(wifiId);
                return data?.ToList() ?? new List<Ins_Wifi_Department_Get_ByWifiId_Value_Result>();
            }
        }

        public List<Ins_Wifi_ExtraBranch_Get_ByWifiId_Value_Result> GetWifiExtraBranchesByWifiId(int wifiId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Wifi_ExtraBranch_Get_ByWifiId_Value(wifiId);
                return data?.ToList() ?? new List<Ins_Wifi_ExtraBranch_Get_ByWifiId_Value_Result>();
            }
        }
    }
}
