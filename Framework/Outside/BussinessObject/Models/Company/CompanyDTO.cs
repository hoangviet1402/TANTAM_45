using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace BussinessObject.Models.Company
{
    public class CompanyDetailRequest
    {
        [JsonProperty("shop", NullValueHandling = NullValueHandling.Ignore)]
        public int CompanyId { get; set; }

        [JsonProperty("userid", NullValueHandling = NullValueHandling.Ignore)]
        public int AccountId { get; set; }
    }

    public class CompanyDetailResponse
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("address_lat", NullValueHandling = NullValueHandling.Ignore)]
        public double? AddressLat { get; set; }

        [JsonProperty("address_lng", NullValueHandling = NullValueHandling.Ignore)]
        public double? AddressLng { get; set; }

        [JsonProperty("tel", NullValueHandling = NullValueHandling.Ignore)]
        public string Tel { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("package", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Package { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("website", NullValueHandling = NullValueHandling.Ignore)]
        public string Website { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("type_of_business", NullValueHandling = NullValueHandling.Ignore)]
        public string TypeOfBusiness { get; set; }

        [JsonProperty("tax_code", NullValueHandling = NullValueHandling.Ignore)]
        public string TaxCode { get; set; }

        [JsonProperty("bank_account", NullValueHandling = NullValueHandling.Ignore)]
        public string BankAccount { get; set; }

        [JsonProperty("bank", NullValueHandling = NullValueHandling.Ignore)]
        public string Bank { get; set; }

        [JsonProperty("founded_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? FoundedDate { get; set; }

        [JsonProperty("scale", NullValueHandling = NullValueHandling.Ignore)]
        public string Scale { get; set; }

        [JsonProperty("charter_capital", NullValueHandling = NullValueHandling.Ignore)]
        public string CharterCapital { get; set; }

        [JsonProperty("logo", NullValueHandling = NullValueHandling.Ignore)]
        public string Logo { get; set; }

        [JsonProperty("brand_logo", NullValueHandling = NullValueHandling.Ignore)]
        public string BrandLogo { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty("r_gold", NullValueHandling = NullValueHandling.Ignore)]
        public int RGold { get; set; }

        [JsonProperty("y_gold", NullValueHandling = NullValueHandling.Ignore)]
        public int YGold { get; set; }

        [JsonProperty("is_shopee", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsShopee { get; set; }

        [JsonProperty("recruitment_template_id", NullValueHandling = NullValueHandling.Ignore)]
        public string RecruitmentTemplateId { get; set; }

        [JsonProperty("employee_excel_version", NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeExcelVersion { get; set; }

        [JsonProperty("tutorial", NullValueHandling = NullValueHandling.Ignore)]
        public ShopTutorial Tutorial { get; set; }

        [JsonProperty("get_started_step", NullValueHandling = NullValueHandling.Ignore)]
        public int GetStartedStep { get; set; }

        [JsonProperty("packages", NullValueHandling = NullValueHandling.Ignore)]
        public Packages Packages { get; set; }

        [JsonProperty("surveys", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> Surveys { get; set; }

        [JsonProperty("is_using_camera_ai", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsUsingCameraAi { get; set; }

        [JsonProperty("clock_setting", NullValueHandling = NullValueHandling.Ignore)]
        public ClockSetting ClockSetting { get; set; }

        [JsonProperty("timeFormat", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeFormat { get; set; }

        [JsonProperty("dateFormat", NullValueHandling = NullValueHandling.Ignore)]
        public string DateFormat { get; set; }

        [JsonProperty("is_using_onleave_v2", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsUsingOnleaveV2 { get; set; }

        [JsonProperty("first_step_modal_off", NullValueHandling = NullValueHandling.Ignore)]
        public int FirstStepModalOff { get; set; }

        [JsonProperty("setup_steps", NullValueHandling = NullValueHandling.Ignore)]
        public List<SetupStep> SetupSteps { get; set; }

        [JsonProperty("integration", NullValueHandling = NullValueHandling.Ignore)]
        public Integration Integration { get; set; }

        [JsonProperty("talent_management", NullValueHandling = NullValueHandling.Ignore)]
        public bool TalentManagement { get; set; }

        [JsonProperty("elearning_management", NullValueHandling = NullValueHandling.Ignore)]
        public bool ElearningManagement { get; set; }
    }

    public class Packages
    {
        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public PackageInfo Info { get; set; }

        [JsonProperty("packages", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, PackageDetail> PackageDetails { get; set; }
    }

    public class PackageInfo
    {
        [JsonProperty("start_package", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartPackage { get; set; }

        [JsonProperty("end_package", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndPackage { get; set; }

        [JsonProperty("users", NullValueHandling = NullValueHandling.Ignore)]
        public int Users { get; set; }

        [JsonProperty("active_users", NullValueHandling = NullValueHandling.Ignore)]
        public int ActiveUsers { get; set; }

        [JsonProperty("is_trial", NullValueHandling = NullValueHandling.Ignore)]
        public int IsTrial { get; set; }

        [JsonProperty("is_full", NullValueHandling = NullValueHandling.Ignore)]
        public int IsFull { get; set; }

        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public int Price { get; set; }
    }

    public class PackageDetail
    {
        [JsonProperty("shop_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ShopId { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("roles", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Roles { get; set; }

        [JsonProperty("users", NullValueHandling = NullValueHandling.Ignore)]
        public int Users { get; set; }

        [JsonProperty("start_package", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartPackage { get; set; }

        [JsonProperty("end_package", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndPackage { get; set; }

        [JsonProperty("is_trial", NullValueHandling = NullValueHandling.Ignore)]
        public int IsTrial { get; set; }

        [JsonProperty("en", NullValueHandling = NullValueHandling.Ignore)]
        public PackageLanguage En { get; set; }

        [JsonProperty("ja", NullValueHandling = NullValueHandling.Ignore)]
        public PackageLanguage Ja { get; set; }

        [JsonProperty("is_available", NullValueHandling = NullValueHandling.Ignore)]
        public int IsAvailable { get; set; }
    }

    public class PackageLanguage
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class ClockSetting
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("auto_fill_shift", NullValueHandling = NullValueHandling.Ignore)]
        public int AutoFillShift { get; set; }

        [JsonProperty("list_enable_clock", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ListEnableClock { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("is_using_timekeeper_data", NullValueHandling = NullValueHandling.Ignore)]
        public int IsUsingTimekeeperData { get; set; }

        [JsonProperty("using_mobile_same_timekeeper", NullValueHandling = NullValueHandling.Ignore)]
        public int UsingMobileSameTimekeeper { get; set; }

        [JsonProperty("is_employee_register_shift", NullValueHandling = NullValueHandling.Ignore)]
        public int IsEmployeeRegisterShift { get; set; }

        [JsonProperty("is_auto_copy_employee_shift", NullValueHandling = NullValueHandling.Ignore)]
        public int IsAutoCopyEmployeeShift { get; set; }

        [JsonProperty("is_not_mobile_check_inout", NullValueHandling = NullValueHandling.Ignore)]
        public int IsNotMobileCheckInout { get; set; }

        [JsonProperty("number_week_register_shift", NullValueHandling = NullValueHandling.Ignore)]
        public int NumberWeekRegisterShift { get; set; }

        [JsonProperty("is_require_detect_image_clock", NullValueHandling = NullValueHandling.Ignore)]
        public int IsRequireDetectImageClock { get; set; }

        [JsonProperty("is_allow_timekeeping_when_ratio_low", NullValueHandling = NullValueHandling.Ignore)]
        public int IsAllowTimekeepingWhenRatioLow { get; set; }

        [JsonProperty("ratio_check_image_tanca_kiot", NullValueHandling = NullValueHandling.Ignore)]
        public int RatioCheckImageTancaKiot { get; set; }

        [JsonProperty("shift_suggested", NullValueHandling = NullValueHandling.Ignore)]
        public int ShiftSuggested { get; set; }

        [JsonProperty("is_clock_offline", NullValueHandling = NullValueHandling.Ignore)]
        public int IsClockOffline { get; set; }

        [JsonProperty("lock_shift_registration", NullValueHandling = NullValueHandling.Ignore)]
        public int LockShiftRegistration { get; set; }

        [JsonProperty("is_system_auto_clockout", NullValueHandling = NullValueHandling.Ignore)]
        public int IsSystemAutoClockout { get; set; }

        [JsonProperty("use_publish_shifts", NullValueHandling = NullValueHandling.Ignore)]
        public int UsePublishShifts { get; set; }
    }

    public class SetupStep
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public int? Code { get; set; }

        [JsonProperty("weight", NullValueHandling = NullValueHandling.Ignore)]
        public int Weight { get; set; }

        [JsonProperty("is_done", NullValueHandling = NullValueHandling.Ignore)]
        public int IsDone { get; set; }
    }

    public class Integration
    {
        [JsonProperty("digital_signature", NullValueHandling = NullValueHandling.Ignore)]
        public bool DigitalSignature { get; set; }
    }

    public class CreateCompanyRequest
    {
        [Required(ErrorMessage = "Tên công ty là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên công ty không được vượt quá 100 ký tự")]
        [JsonProperty("full_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [StringLength(250, ErrorMessage = "Địa chỉ không được vượt quá 250 ký tự")]
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
    }

    public class ShopUpdateRequest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("address_lat", NullValueHandling = NullValueHandling.Ignore)]
        public double? AddressLat { get; set; }

        [JsonProperty("address_lng", NullValueHandling = NullValueHandling.Ignore)]
        public double? AddressLng { get; set; }

        [JsonProperty("tel", NullValueHandling = NullValueHandling.Ignore)]
        public string Tel { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("package", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Package { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("website", NullValueHandling = NullValueHandling.Ignore)]
        public string Website { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("type_of_business", NullValueHandling = NullValueHandling.Ignore)]
        public string TypeOfBusiness { get; set; }

        [JsonProperty("tax_code", NullValueHandling = NullValueHandling.Ignore)]
        public string TaxCode { get; set; }

        [JsonProperty("bank_account", NullValueHandling = NullValueHandling.Ignore)]
        public string BankAccount { get; set; }

        [JsonProperty("bank", NullValueHandling = NullValueHandling.Ignore)]
        public string Bank { get; set; }

        [JsonProperty("founded_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? FoundedDate { get; set; }

        [JsonProperty("scale", NullValueHandling = NullValueHandling.Ignore)]
        public string Scale { get; set; }

        [JsonProperty("charter_capital", NullValueHandling = NullValueHandling.Ignore)]
        public string CharterCapital { get; set; }

        [JsonProperty("logo", NullValueHandling = NullValueHandling.Ignore)]
        public string Logo { get; set; }

        [JsonProperty("brand_logo", NullValueHandling = NullValueHandling.Ignore)]
        public string BrandLogo { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty("r_gold", NullValueHandling = NullValueHandling.Ignore)]
        public int RGold { get; set; }

        [JsonProperty("y_gold", NullValueHandling = NullValueHandling.Ignore)]
        public int YGold { get; set; }

        [JsonProperty("is_shopee", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsShopee { get; set; }

        [JsonProperty("recruitment_template_id", NullValueHandling = NullValueHandling.Ignore)]
        public string RecruitmentTemplateId { get; set; }

        [JsonProperty("employee_excel_version", NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeExcelVersion { get; set; }

        [JsonProperty("tutorial", NullValueHandling = NullValueHandling.Ignore)]
        public ShopTutorial Tutorial { get; set; }

        [JsonProperty("get_started_step", NullValueHandling = NullValueHandling.Ignore)]
        public int GetStartedStep { get; set; }
    }

    public class ShopTutorial
    {
        [JsonProperty("c1", NullValueHandling = NullValueHandling.Ignore)]
        public int C1 { get; set; }

        [JsonProperty("c2", NullValueHandling = NullValueHandling.Ignore)]
        public int C2 { get; set; }

        [JsonProperty("c3", NullValueHandling = NullValueHandling.Ignore)]
        public int C3 { get; set; }
    }
}