using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BussinessObject.Models.Shift
{
    #region Create Wifi Request/Response
    public class WifiCreateRequest
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string address { get; set; }

        [JsonProperty("radius", NullValueHandling = NullValueHandling.Ignore)]
        public int radius { get; set; }

        [JsonProperty("speed", NullValueHandling = NullValueHandling.Ignore)]
        public int speed { get; set; }

        [JsonProperty("accuracy", NullValueHandling = NullValueHandling.Ignore)]
        public int accuracy { get; set; }

        [JsonProperty("altitude", NullValueHandling = NullValueHandling.Ignore)]
        public int altitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public float longitude { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public float latitude { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int branch_id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public int type { get; set; }

        [JsonProperty("is_both_bssid_ssid", NullValueHandling = NullValueHandling.Ignore)]
        public bool is_both_bssid_ssid { get; set; }
    }

    public class WifiCreateResponse
    {
        [JsonProperty("wifi_id", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_id { get; set; }
    }

    /// <summary>
    /// Response cho tạo WiFi với thông tin chi tiết từ Ins_Wifi_Create
    /// </summary>
    public class WifiCreateWithResultResponse
    {
        [JsonProperty("wifi_id", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_id { get; set; }

        [JsonProperty("radius", NullValueHandling = NullValueHandling.Ignore)]
        public int radius { get; set; }

        [JsonProperty("speed", NullValueHandling = NullValueHandling.Ignore)]
        public int speed { get; set; }

        [JsonProperty("accuracy", NullValueHandling = NullValueHandling.Ignore)]
        public int accuracy { get; set; }

        [JsonProperty("altitude", NullValueHandling = NullValueHandling.Ignore)]
        public int altitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public double longitude { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public double latitude { get; set; }

        [JsonProperty("wifi_name", NullValueHandling = NullValueHandling.Ignore)]
        public string wifi_name { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int branch_id { get; set; }

        [JsonProperty("wifi_address", NullValueHandling = NullValueHandling.Ignore)]
        public string wifi_address { get; set; }

        [JsonProperty("wifi_type", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_type { get; set; }

        [JsonProperty("bssid", NullValueHandling = NullValueHandling.Ignore)]
        public string bssid { get; set; }

        [JsonProperty("is_both_bssid_ssid", NullValueHandling = NullValueHandling.Ignore)]
        public bool is_both_bssid_ssid { get; set; }

        [JsonProperty("create_date", NullValueHandling = NullValueHandling.Ignore)]
        public string create_date { get; set; }

        [JsonProperty("update_date", NullValueHandling = NullValueHandling.Ignore)]
        public string update_date { get; set; }
    }
    #endregion

    #region Update Wifi Request/Response
    public class WifiUpdateRequest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string address { get; set; }

        [JsonProperty("radius", NullValueHandling = NullValueHandling.Ignore)]
        public int radius { get; set; }

        [JsonProperty("speed", NullValueHandling = NullValueHandling.Ignore)]
        public int speed { get; set; }

        [JsonProperty("accuracy", NullValueHandling = NullValueHandling.Ignore)]
        public int accuracy { get; set; }

        [JsonProperty("altitude", NullValueHandling = NullValueHandling.Ignore)]
        public int altitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public float longitude { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public float latitude { get; set; }

        [JsonProperty("bssid", NullValueHandling = NullValueHandling.Ignore)]
        public string bssid { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int branch_id { get; set; }

        [JsonProperty("department_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> department_ids { get; set; }

        [JsonProperty("user_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> user_ids { get; set; }

        [JsonProperty("extra_branch_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> extra_branch_ids { get; set; }

        [JsonProperty("is_both_bssid_ssid", NullValueHandling = NullValueHandling.Ignore)]
        public bool is_both_bssid_ssid { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }
    }

    public class WifiUpdateResponse
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty("bssid", NullValueHandling = NullValueHandling.Ignore)]
        public string bssid { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public string branch_id { get; set; }

        [JsonProperty("branch_obj", NullValueHandling = NullValueHandling.Ignore)]
        public WifiBranchObject branch_obj { get; set; }

        [JsonProperty("department_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> department_ids { get; set; }

        [JsonProperty("department_obj", NullValueHandling = NullValueHandling.Ignore)]
        public List<WifiDepartmentObject> department_obj { get; set; }

        [JsonProperty("extra_branch_obj", NullValueHandling = NullValueHandling.Ignore)]
        public List<WifiBranchObject> extra_branch_obj { get; set; }

        [JsonProperty("user_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> user_ids { get; set; }

        [JsonProperty("extra_branch_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> extra_branch_ids { get; set; }

        [JsonProperty("is_both_bssid_ssid", NullValueHandling = NullValueHandling.Ignore)]
        public int is_both_bssid_ssid { get; set; }
    }
    #endregion

    #region Create Advanced Wifi Request/Response
    public class WifiCreateAdvancedRequest
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty("bssid", NullValueHandling = NullValueHandling.Ignore)]
        public string bssid { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int branch_id { get; set; }

        [JsonProperty("department_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> department_ids { get; set; }

        [JsonProperty("user_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> user_ids { get; set; }

        [JsonProperty("extra_branch_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> extra_branch_ids { get; set; }

        [JsonProperty("is_both_bssid_ssid", NullValueHandling = NullValueHandling.Ignore)]
        public bool is_both_bssid_ssid { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string address { get; set; }

        [JsonProperty("radius", NullValueHandling = NullValueHandling.Ignore)]
        public int radius { get; set; }

        [JsonProperty("speed", NullValueHandling = NullValueHandling.Ignore)]
        public int speed { get; set; }

        [JsonProperty("accuracy", NullValueHandling = NullValueHandling.Ignore)]
        public int accuracy { get; set; }

        [JsonProperty("altitude", NullValueHandling = NullValueHandling.Ignore)]
        public int altitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public float longitude { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public float latitude { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }
    }

    /// <summary>
    /// Response cho tạo WiFi nâng cao
    /// </summary>
    public class WifiCreateAdvancedResponse
    {
        public int id { get; set; }
        public string bssid { get; set; }
        public string name { get; set; }
        public WifiBranchObject branch_obj { get; set; }
        public bool is_both_bssid_ssid { get; set; }
        public Nullable<int> branch_id { get; set; }
        public String created_at { get; set; }
        public String updated_at { get; set; }
        public List<WifiDepartmentObject> department_obj { get; set; }
        public List<WifiBranchObject> extra_branch_obj { get; set; }
        public string message { get; set; }
    }

    /// <summary>
    /// Object chi nhánh trong response WiFi
    /// </summary>
    public class WifiBranchObject
    {
        public int id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
    }

    /// <summary>
    /// Object phòng ban trong response WiFi
    /// </summary>
    public class WifiDepartmentObject
    {
        public int id { get; set; }
        public string name { get; set; }
        public int parent_id { get; set; }
    }
    #endregion

    #region Create Wifi Account Request/Response
    public class WifiAccountCreateRequest
    {
        [JsonProperty("wifi_id", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_id { get; set; }

        [JsonProperty("account_map_id", NullValueHandling = NullValueHandling.Ignore)]
        public int account_map_id { get; set; }
    }

    public class WifiAccountCreateResponse
    {
        [JsonProperty("wifi_account_id", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_account_id { get; set; }
    }
    #endregion

    #region Create Wifi Department Request/Response
    public class WifiDepartmentCreateRequest
    {
        [JsonProperty("wifi_id", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_id { get; set; }

        [JsonProperty("department_id", NullValueHandling = NullValueHandling.Ignore)]
        public int department_id { get; set; }
    }

    public class WifiDepartmentCreateResponse
    {
        [JsonProperty("wifi_department_id", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_department_id { get; set; }
    }
    #endregion

    #region Create Wifi User Request/Response
    public class WifiUserCreateRequest
    {
        [JsonProperty("wifi_id", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_id { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int user_id { get; set; }
    }

    public class WifiUserCreateResponse
    {
        [JsonProperty("wifi_user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_user_id { get; set; }
    }
    #endregion

    public class WifiListRequest
    {
        [JsonProperty("wifi_id", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_id { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int branch_id { get; set; }

        [JsonProperty("department_id", NullValueHandling = NullValueHandling.Ignore)]
        public int department_id { get; set; }

        [JsonProperty("account_map_id", NullValueHandling = NullValueHandling.Ignore)]
        public int account_map_id { get; set; }
    }

    public class WifiListRequestAdvanced
    {
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int page { get; set; }

        [JsonProperty("per_page", NullValueHandling = NullValueHandling.Ignore)]
        public int per_page { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }
    }

    public class WifiListResponse
    {
        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<WifiInfo> items { get; set; }
    }

    public class WifiInfo
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string address { get; set; }

        [JsonProperty("radius", NullValueHandling = NullValueHandling.Ignore)]
        public int radius { get; set; }

        [JsonProperty("speed", NullValueHandling = NullValueHandling.Ignore)]
        public int speed { get; set; }

        [JsonProperty("accuracy", NullValueHandling = NullValueHandling.Ignore)]
        public int accuracy { get; set; }

        [JsonProperty("altitude", NullValueHandling = NullValueHandling.Ignore)]
        public int altitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public float longitude { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public float latitude { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public int type { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int branch_id { get; set; }

        [JsonProperty("department_id", NullValueHandling = NullValueHandling.Ignore)]
        public int department_id { get; set; }

        [JsonProperty("account_map_id", NullValueHandling = NullValueHandling.Ignore)]
        public int account_map_id { get; set; }

        [JsonProperty("bssid", NullValueHandling = NullValueHandling.Ignore)]
        public string bssid { get; set; }

        [JsonProperty("wifi_account_id", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_account_id { get; set; }

        [JsonProperty("wifi_department_id", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_department_id { get; set; }
    }

    /// <summary>
    /// Response cho GetWifiListAdvanced - trả về list WiFi
    /// </summary>
    public class WifiListAdvancedResponse
    {
        public List<WifiAdvancedInfo> items { get; set; }
        public PaginationMeta meta { get; set; }
    }

    public class GPSListAdvancedResponse
    {
        public List<GPSAdvancedInfo> items { get; set; }
        public PaginationMeta meta { get; set; }
    }

    /// <summary>
    /// Pagination metadata cho response
    /// </summary>
    public class PaginationMeta
    {
        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public int total { get; set; }

        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int count { get; set; }

        [JsonProperty("per_page", NullValueHandling = NullValueHandling.Ignore)]
        public int per_page { get; set; }

        [JsonProperty("current_page", NullValueHandling = NullValueHandling.Ignore)]
        public int current_page { get; set; }

        [JsonProperty("total_pages", NullValueHandling = NullValueHandling.Ignore)]
        public int total_pages { get; set; }
    }

    /// <summary>
    /// Thông tin WiFi trong list advanced
    /// </summary>
    public class WifiAdvancedInfo
    {
        public Nullable<int> id { get; set; }
        public string bssid { get; set; }
        public string name { get; set; }
        public WifiBranchObject branch_obj { get; set; }
        public bool is_both_bssid_ssid { get; set; }
        public int branch_id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public List<WifiDepartmentObject> department_obj { get; set; }
        public List<WifiBranchObject> extra_branch_obj { get; set; }
        public string address { get; set; }
        public int radius { get; set; }
        public int speed { get; set; }
        public int accuracy { get; set; }
        public int altitude { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public int type { get; set; }
    }

    /// <summary>
    /// Thông tin WiFi trong list advanced
    /// </summary>
    public class GPSAdvancedInfo
    {
        public Nullable<int> id { get; set; }
        public string name { get; set; }
        public int branch_id { get; set; }
        public WifiBranchObject branch_obj { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public List<WifiDepartmentObject> department_obj { get; set; }
        public List<WifiBranchObject> extra_branch_obj { get; set; }
        public string address { get; set; }
        public int radius { get; set; }
        public int speed { get; set; }
        public int accuracy { get; set; }
        public int altitude { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public int type { get; set; }
    }

    #region Get Wifi By Company ID Request/Response
    public class WifiGetByCompanyIdRequest
    {
        [JsonProperty("company_id", NullValueHandling = NullValueHandling.Ignore)]
        public int company_id { get; set; }
    }
    #endregion

    #region Get Wifi Branch Request/Response
    public class WifiBranchGetRequest
    {
        [JsonProperty("wifi_id", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_id { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int branch_id { get; set; }
    }

    public class WifiBranchGetResponse
    {
        [JsonProperty("branch_list", NullValueHandling = NullValueHandling.Ignore)]
        public List<WifiBranchObject> branch_list { get; set; }
    }
    #endregion

    public class WifiDepartmentGetRequest
    {
        [JsonProperty("wifi_id", NullValueHandling = NullValueHandling.Ignore)]
        public int wifi_id { get; set; }

        [JsonProperty("department_id", NullValueHandling = NullValueHandling.Ignore)]
        public int department_id { get; set; }
    }

    public class WifiDepartmentGetResponse
    {
        [JsonProperty("department_list", NullValueHandling = NullValueHandling.Ignore)]
        public List<WifiDepartmentObject> department_list { get; set; }
    }

}