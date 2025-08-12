using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Shift;
using DataAccess;
using DataAccess.Model.Shift;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using Newtonsoft.Json;
using ResxLanguagesUtility;
using ResxLanguagesUtility.Enums;

namespace BussinessObject.Bo.Shift
{
    public class WifiBo : BaseBo<DBNull>
    {
        public WifiBo()
            : base(DaoFactory.Wifi) { }

        /// <summary>
        /// Tạo WiFi nâng cao với nhiều liên kết - OPTIMIZED VERSION
        /// </summary>
        public ApiResult<WifiListAdvancedResponse> CreateWifiAdvanced(
            WifiCreateAdvancedRequest request,
            int companyId
        )
        {
            var response = new ApiResult<WifiListAdvancedResponse>()
            {
                Data = new WifiListAdvancedResponse()
                {
                    items = new List<WifiAdvancedInfo>(),
                    meta = new PaginationMeta(),
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                var validationResult = ValidateWifiCreateRequest(request);
                if (!validationResult.IsValid)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = validationResult.ErrorMessage;
                    return response;
                }
                var type = GetWifiTypeFromRequest(request.type);

                var wifiResult = CreateWifiWithOptimizedData(request, type);
                if (wifiResult == null || wifiResult.WifiID <= 0)
                {
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Tạo WiFi thất bại - không thể tạo WiFi cơ bản";
                    return response;
                }

                CreateWifiAssociations(wifiResult.WifiID, request, response.Data);
                var wifiList = DaoFactory.Wifi.GetWifiListUsingCreatePattern(companyId, type);

                if (wifiList != null && wifiList.Any())
                {
                    response.Data.items = wifiList
                        .Select(wifi => new WifiAdvancedInfo
                        {
                            id = wifi.WifiID,
                            bssid = wifi.Bssid ?? "",
                            name = wifi.WifiName ?? "",
                            branch_id = wifi.BranchID ?? 0,
                            branch_obj = GetBranchObject(wifi.WifiID ?? 0, wifi.BranchID ?? 0),
                            is_both_bssid_ssid = wifi.IsBothBssidSsid ?? false,
                            created_at = wifi.CreateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                            updated_at = wifi.UpdateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                            department_obj = GetDepartmentObject(wifi.WifiID ?? 0),
                            extra_branch_obj = GetExtraBranchObject(wifi.WifiID ?? 0),
                            address = wifi.WifiAddress ?? "",
                            radius = wifi.Radius ?? 0,
                            speed = wifi.Speed ?? 0,
                            accuracy = wifi.Accuracy ?? 0,
                            altitude = wifi.Altitude ?? 0,
                            longitude = (float)(wifi.Longitude ?? 0),
                            latitude = (float)(wifi.Latitude ?? 0),
                            type = type,
                        })
                        .ToList();

                    // Set pagination metadata for create response
                    response.Data.meta = new PaginationMeta
                    {
                        total = wifiList.Count(),
                        count = response.Data.items.Count,
                        per_page = response.Data.items.Count, // Show all records for create response
                        current_page = 1,
                        total_pages = 1,
                    };
                }
                else
                {
                    // Set empty pagination metadata
                    response.Data.meta = new PaginationMeta
                    {
                        total = 0,
                        count = 0,
                        per_page = 0,
                        current_page = 1,
                        total_pages = 0,
                    };
                }

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Tạo WiFi nâng cao thành công";

                CommonLogger.DefaultLogger.InfoFormat(
                    "WifiBo.CreateWifiAdvanced - Successfully created WiFi ID: {0}, Name: {1}",
                    wifiResult.WifiID,
                    wifiResult.WifiName
                );
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (
                    entityEx.InnerException != null
                    && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx
                )
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error(
                        "WifiBo.CreateWifiAdvanced - Entity Framework Error",
                        entityEx
                    );
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi database trong quá trình xử lý";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "WifiBo.CreateWifiAdvanced - Unexpected Error: {0}",
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }

            return response;
        }

        /// <summary>
        /// Validation helper method - Tách riêng validation logic
        /// </summary>
        private ValidationResult ValidateWifiCreateRequest(WifiCreateAdvancedRequest request)
        {
            if (request == null)
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Dữ liệu không hợp lệ",
                };

            if (string.IsNullOrWhiteSpace(request.name))
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Tên WiFi không được để trống",
                };

            if (request.branch_id <= 0)
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "ID chi nhánh không hợp lệ",
                };

            if (request.radius < 0)
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Bán kính WiFi không được âm",
                };

            if (request.accuracy < 0)
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Độ chính xác không được âm",
                };

            return new ValidationResult { IsValid = true, ErrorMessage = string.Empty };
        }

        /// <summary>
        /// Validation result class - Thay thế tuple cho .NET Framework cũ
        /// </summary>
        private class ValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }
        }

        /// <summary>
        /// Create WiFi với optimized data handling - Giảm số lần gọi FirstOrDefault()
        /// </summary>
        private Ins_Wifi_Create_Result CreateWifiWithOptimizedData(
            WifiCreateAdvancedRequest request, int type
        )
        {
            var dataResult = DaoFactory.Wifi.CreateWifiWithResult(
                request.radius,
                request.speed,
                request.accuracy,
                request.altitude,
                request.longitude,
                request.latitude,
                request.name,
                request.branch_id,
                request.address,
                type,
                request.is_both_bssid_ssid,
                request.bssid ?? ""
            );

            return dataResult?.FirstOrDefault();
        }

        /// <summary>
        /// Create WiFi associations với parallel processing - Tối ưu performance
        /// </summary>
        private void CreateWifiAssociations(
            int wifiId,
            WifiCreateAdvancedRequest request,
            WifiListAdvancedResponse responseData
        )
        {
            var branchId = request.branch_id;

            var tasks = new List<Task>();

            // Create user associations
            if (request.user_ids?.Any() == true)
            {
                tasks.Add(Task.Run(() => CreateUserAssociations(wifiId, request.user_ids)));
            }

            // Create department associations
            if (request.department_ids?.Any() == true)
            {
                tasks.Add(
                    Task.Run(() => CreateDepartmentAssociations(wifiId, request.department_ids))
                );
            }

            // Create extra branch associations
            if (request.extra_branch_ids?.Any() == true)
            {
                tasks.Add(
                    Task.Run(() => CreateExtraBranchAssociations(wifiId, request.extra_branch_ids))
                );
            }

            //  WAIT FOR ALL TASKS - Đảm bảo tất cả associations được tạo xong
            if (tasks.Any())
            {
                Task.WaitAll(tasks.ToArray());
            }

            //  KHÔNG CẦN BUILD DEPARTMENT OBJECTS - Vì response giờ là WifiListAdvancedResponse
            // Department objects sẽ được build trong danh sách WiFi
        }

        /// <summary>
        /// Create user associations với error handling
        /// </summary>
        private void CreateUserAssociations(int wifiId, List<string> userIds)
        {
            foreach (var userId in userIds)
            {
                try
                {
                    if (int.TryParse(userId, out int userMapId) && userMapId > 0)
                    {
                        var accountId = DaoFactory.Wifi.CreateWifiAccount(wifiId, userMapId);
                        if (accountId <= 0)
                        {
                            CommonLogger.DefaultLogger.WarnFormat(
                                "WifiBo.CreateUserAssociations - Failed to create account association for WiFi: {0}, User: {1}",
                                wifiId,
                                userMapId
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat(
                        "WifiBo.CreateUserAssociations - Error creating user association: {0}",
                        ex
                    );
                }
            }
        }

        /// <summary>
        /// Create department associations với error handling
        /// </summary>
        private void CreateDepartmentAssociations(int wifiId, List<string> departmentIds)
        {
            if (departmentIds == null || !departmentIds.Any())
            {
                CommonLogger.DefaultLogger.InfoFormat(
                    "WifiBo.CreateDepartmentAssociations - No department IDs provided for WiFi: {0}",
                    wifiId
                );
                return;
            }

            foreach (var deptId in departmentIds)
            {
                try
                {
                    if (int.TryParse(deptId, out int departmentId) && departmentId > 0)
                    {
                        var deptAssociationId = DaoFactory.Wifi.CreateWifiDepartment(
                            wifiId,
                            departmentId
                        );
                        if (deptAssociationId <= 0)
                        {
                            CommonLogger.DefaultLogger.WarnFormat(
                                "WifiBo.CreateDepartmentAssociations - Failed to create department association for WiFi: {0}, Department: {1}",
                                wifiId,
                                departmentId
                            );
                        }
                        else
                        {
                            CommonLogger.DefaultLogger.InfoFormat(
                                "WifiBo.CreateDepartmentAssociations - Successfully created department association for WiFi: {0}, Department: {1}, AssociationId: {2}",
                                wifiId,
                                departmentId,
                                deptAssociationId
                            );
                        }
                    }
                    else
                    {
                        CommonLogger.DefaultLogger.WarnFormat(
                            "WifiBo.CreateDepartmentAssociations - Invalid department ID format: {0} for WiFi: {1}",
                            deptId,
                            wifiId
                        );
                    }
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat(
                        "WifiBo.CreateDepartmentAssociations - Error creating department association for WiFi: {0}, Department: {1}, Error: {2}",
                        wifiId,
                        deptId,
                        ex
                    );
                }
            }
        }

        /// <summary>
        /// Create extra branch associations - Không cần response building vì response giờ là WifiListAdvancedResponse
        /// </summary>
        private void CreateExtraBranchAssociations(int wifiId, List<string> extraBranchIds)
        {
            if (extraBranchIds == null || !extraBranchIds.Any())
            {
                CommonLogger.DefaultLogger.InfoFormat(
                    "WifiBo.CreateExtraBranchAssociations - No extra branch IDs provided for WiFi: {0}",
                    wifiId
                );
                return;
            }

            foreach (var extraBranchId in extraBranchIds)
            {
                try
                {
                    if (int.TryParse(extraBranchId, out int branchIdExtra) && branchIdExtra > 0)
                    {
                        var extraBranchAssociationId = DaoFactory.Wifi.CreateWifiExtraBranch(
                            wifiId,
                            branchIdExtra
                        );
                        if (extraBranchAssociationId <= 0)
                        {
                            CommonLogger.DefaultLogger.WarnFormat(
                                "WifiBo.CreateExtraBranchAssociations - Failed to create extra branch association for WiFi: {0}, Branch: {1}",
                                wifiId,
                                branchIdExtra
                            );
                        }
                        else
                        {
                            CommonLogger.DefaultLogger.InfoFormat(
                                "WifiBo.CreateExtraBranchAssociations - Successfully created extra branch association for WiFi: {0}, Branch: {1}, AssociationId: {2}",
                                wifiId,
                                branchIdExtra,
                                extraBranchAssociationId
                            );
                        }
                    }
                    else
                    {
                        CommonLogger.DefaultLogger.WarnFormat(
                            "WifiBo.CreateExtraBranchAssociations - Invalid extra branch ID format: {0} for WiFi: {1}",
                            extraBranchId,
                            wifiId
                        );
                    }
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat(
                        "WifiBo.CreateExtraBranchAssociations - Error creating extra branch association for WiFi: {0}, Branch: {1}, Error: {2}",
                        wifiId,
                        extraBranchId,
                        ex
                    );
                }
            }
        }

        /// <summary>
        /// Lấy danh sách WiFi nâng cao theo Company ID với phân trang
        /// </summary>
        public ApiResult<WifiListAdvancedResponse> GetWifiListAdvanced(
            WifiListRequestAdvanced request,
            int companyId
        )
        {
            var response = new ApiResult<WifiListAdvancedResponse>()
            {
                Data = new WifiListAdvancedResponse()
                {
                    items = new List<WifiAdvancedInfo>(),
                    meta = new PaginationMeta(),
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Validation cho request
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Dữ liệu request không hợp lệ";
                    return response;
                }

                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "ID công ty không hợp lệ";
                    return response;
                }

                var type = GetWifiTypeFromRequest(request.type);

                // Pagination parameters
                var perPage = request.per_page > 0 ? request.per_page : 15; // Default page size
                var currentPage = request.page > 0 ? request.page : 1;
                var offset = (currentPage - 1) * perPage;

                // Get total count first
                var totalWifiList = DaoFactory.Wifi.GetWifiListUsingCreatePattern(companyId, type);
                var totalCount = totalWifiList?.Count() ?? 0;

                // Calculate pagination metadata
                var totalPages = (int)Math.Ceiling((double)totalCount / perPage);
                var actualCount = Math.Min(perPage, totalCount - offset);

                // Apply pagination to data
                var paginatedWifiList =
                    totalWifiList?.Skip(offset).Take(perPage).ToList()
                    ?? new List<Ins_Wifi_Get_ByCompanyId_Result>();

                if (paginatedWifiList.Any())
                {
                    response.Data.items = paginatedWifiList
                        .Select(wifi => new WifiAdvancedInfo
                        {
                            id = wifi.WifiID,
                            bssid = wifi.Bssid ?? "",
                            name = wifi.WifiName ?? "",
                            branch_id = wifi.BranchID ?? 0,
                            branch_obj = GetBranchObject(wifi.WifiID ?? 0, wifi.BranchID ?? 0),
                            is_both_bssid_ssid = wifi.IsBothBssidSsid ?? false,
                            created_at = wifi.CreateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                            updated_at = wifi.UpdateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                            department_obj = GetDepartmentObject(wifi.WifiID ?? 0),
                            extra_branch_obj = GetExtraBranchObject(wifi.WifiID ?? 0),
                            address = wifi.WifiAddress ?? "",
                            radius = wifi.Radius ?? 0,
                            speed = wifi.Speed ?? 0,
                            accuracy = wifi.Accuracy ?? 0,
                            altitude = wifi.Altitude ?? 0,
                            longitude = (float)(wifi.Longitude ?? 0),
                            latitude = (float)(wifi.Latitude ?? 0),
                            type = type,
                        })
                        .ToList();

                    // Set pagination metadata
                    response.Data.meta = new PaginationMeta
                    {
                        total = totalCount,
                        count = response.Data.items.Count,
                        per_page = perPage,
                        current_page = currentPage,
                        total_pages = totalPages,
                    };

                    CommonLogger.DefaultLogger.InfoFormat(
                        "WifiBo.GetWifiListAdvanced - Successfully retrieved {0} WiFi records (Page {1}/{2}) for Company ID: {3}",
                        response.Data.items.Count,
                        currentPage,
                        totalPages,
                        companyId
                    );
                }
                else
                {
                    // Set empty pagination metadata
                    response.Data.meta = new PaginationMeta
                    {
                        total = 0,
                        count = 0,
                        per_page = perPage,
                        current_page = currentPage,
                        total_pages = 0,
                    };

                    CommonLogger.DefaultLogger.InfoFormat(
                        "WifiBo.GetWifiListAdvanced - No WiFi records found for Company ID: {0} (Page {1})",
                        companyId,
                        currentPage
                    );
                }

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy danh sách WiFi nâng cao thành công";
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (
                    entityEx.InnerException != null
                    && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx
                )
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error(
                        "WifiBo.GetWifiListAdvanced - Entity Framework Error",
                        entityEx
                    );
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi database trong quá trình lấy danh sách WiFi";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "WifiBo.GetWifiListAdvanced - Unexpected Error: {0}",
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }

            return response;
        }

        /// <summary>
        /// Lấy danh sách GPS nâng cao theo Company ID với phân trang
        /// </summary>
        public ApiResult<GPSListAdvancedResponse> GetGPSListAdvanced(
            WifiListRequestAdvanced request,
            int companyId
        )
        {
            var response = new ApiResult<GPSListAdvancedResponse>()
            {
                Data = new GPSListAdvancedResponse()
                {
                    items = new List<GPSAdvancedInfo>(),
                    meta = new PaginationMeta(),
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Validation cho request
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Dữ liệu request không hợp lệ";
                    return response;
                }

                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "ID công ty không hợp lệ";
                    return response;
                }
                var type = GetWifiTypeFromRequest(request.type);
                // Pagination parameters
                var perPage = request.per_page > 0 ? request.per_page : 15; // Default page size
                var currentPage = request.page > 0 ? request.page : 1;
                var offset = (currentPage - 1) * perPage;

                // Get total count first
                var totalGPSList = DaoFactory.Wifi.GetWifiListUsingCreatePattern(companyId,type);
                var totalCount = totalGPSList?.Count() ?? 0;

                // Calculate pagination metadata
                var totalPages = (int)Math.Ceiling((double)totalCount / perPage);
                var actualCount = Math.Min(perPage, totalCount - offset);

                // Apply pagination to data
                var paginatedGPSList =
                    totalGPSList?.Skip(offset).Take(perPage).ToList()
                    ?? new List<Ins_Wifi_Get_ByCompanyId_Result>();

                if (paginatedGPSList.Any())
                {
                    response.Data.items = paginatedGPSList
                        .Select(gps => new GPSAdvancedInfo
                        {
                            id = gps.WifiID,
                            name = gps.WifiName ?? "",
                            branch_obj = GetBranchObject(gps.WifiID ?? 0, gps.BranchID ?? 0),
                            created_at = gps.CreateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                            updated_at = gps.UpdateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                            department_obj = GetDepartmentObject(gps.WifiID ?? 0),
                            extra_branch_obj = GetExtraBranchObject(gps.WifiID ?? 0),
                            address = gps.WifiAddress ?? "",
                            radius = gps.Radius ?? 0,
                            speed = gps.Speed ?? 0,
                            accuracy = gps.Accuracy ?? 0,
                            altitude = gps.Altitude ?? 0,
                            longitude = (float)(gps.Longitude ?? 0),
                            latitude = (float)(gps.Latitude ?? 0),
                        })
                        .ToList();

                    // Set pagination metadata
                    response.Data.meta = new PaginationMeta
                    {
                        total = totalCount,
                        count = response.Data.items.Count,
                        per_page = perPage,
                        current_page = currentPage,
                        total_pages = totalPages,
                    };

                    CommonLogger.DefaultLogger.InfoFormat(
                        "WifiBo.GetGPSListAdvanced - Successfully retrieved {0} GPS records (Page {1}/{2}) for Company ID: {3}",
                        response.Data.items.Count,
                        currentPage,
                        totalPages,
                        companyId
                    );
                }
                else
                {
                    // Set empty pagination metadata
                    response.Data.meta = new PaginationMeta
                    {
                        total = 0,
                        count = 0,
                        per_page = perPage,
                        current_page = currentPage,
                        total_pages = 0,
                    };

                    CommonLogger.DefaultLogger.InfoFormat(
                        "WifiBo.GetGPSListAdvanced - No GPS records found for Company ID: {0} (Page {1})",
                        companyId,
                        currentPage
                    );
                }

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy danh sách GPS nâng cao thành công";
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (
                    entityEx.InnerException != null
                    && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx
                )
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error(
                        "WifiBo.GetGPSListAdvanced - Entity Framework Error",
                        entityEx
                    );
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi database trong quá trình lấy danh sách GPS";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "WifiBo.GetGPSListAdvanced - Unexpected Error: {0}",
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }

            return response;
        }

        /// <summary>
        /// Helper method để lấy thông tin branch object
        /// </summary>
        /// <param name="wifiId">WiFi ID</param>
        /// <param name="branchId">Branch ID</param>
        /// <returns>WifiBranchObject</returns>
        private WifiBranchObject GetBranchObject(int wifiId, int branchId)
        {
            try
            {
                var branchList = DaoFactory.Wifi.WifiBranchGetByWifiIdAndBranchId(wifiId, branchId);

                if (branchList != null && branchList.Any())
                {
                    var branch = branchList.First();
                    return new WifiBranchObject
                    {
                        id = branch.BranchId ?? 0,
                        name = branch.BranchName ?? "",
                        color = branch.Color,
                    };
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error(
                    $"WifiBo.GetBranchObject Error - WifiId: {wifiId}, BranchId: {branchId}",
                    ex
                );
            }

            // Fallback nếu không lấy được thông tin từ DB
            return new WifiBranchObject
            {
                id = branchId,
                name = "Chi nhánh mặt định",
                color = null,
            };
        }

        /// <summary>
        /// Cập nhật WiFi theo WiFi ID
        /// </summary>
        public ApiResult<WifiUpdateResponse> UpdateWifi(WifiUpdateRequest request)
        {
            var response = new ApiResult<WifiUpdateResponse>()
            {
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Validation
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Dữ liệu không hợp lệ";
                    return response;
                }

                if (request.id <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "WiFi ID không hợp lệ";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.name))
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Tên WiFi không được để trống";
                    return response;
                }

                if (request?.branch_id == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Branch ID không được để trống";
                    return response;
                }

                if (request.type == "wifi" && request.bssid == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "BSSID không được để trống";
                    return response;
                }

                if (!request.is_both_bssid_ssid)
                {
                    request.is_both_bssid_ssid = false;
                }
                var type = GetWifiTypeFromRequest(request.type);
                // Update WiFi using DAO với default values cho các fields không có trong request mới
                var updateResults = DaoFactory.Wifi.UpdateWifiByWifiId(
                    request.id,
                    request.radius, // radius - default value
                    request.speed, // speed - default value
                    request.accuracy, // accuracy - default value
                    request.altitude, // altitude - default value
                    request.longitude, // longitude - default value
                    request.latitude, // latitude - default value
                    request.name,
                    request.branch_id,
                    request.address, // wifi_address - default value
                    type, // wifi_type - default value
                    request.bssid ?? "",
                    request.is_both_bssid_ssid
                );

                if (updateResults == null || !updateResults.Any())
                {
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Cập nhật WiFi thất bại";
                    return response;
                }

                var updateResult = updateResults.FirstOrDefault();
                UpdateWifiDepartmentAssociations(request.id, request.department_ids);
                UpdateWifiExtraBranchAssociations(request.id, request.extra_branch_ids);

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Cập nhật WiFi thành công";

                CommonLogger.DefaultLogger.InfoFormat(
                    "WifiBo.UpdateWifi - Successfully updated WiFi ID: {0}, Name: {1}",
                    request.id,
                    request.name
                );
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (
                    entityEx.InnerException != null
                    && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx
                )
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error(
                        "WifiBo.UpdateWifi - Entity Framework Error",
                        entityEx
                    );
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi database trong quá trình cập nhật";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "WifiBo.UpdateWifi - Unexpected Error: {0}",
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }

            return response;
        }

        /// <summary>
        /// Cập nhật department associations cho WiFi - So sánh và cập nhật theo yêu cầu
        /// </summary>
        /// <param name="wifiId">WiFi ID</param>
        /// <param name="newDepartmentIds">Danh sách department IDs mới</param>
        private void UpdateWifiDepartmentAssociations(int wifiId, List<string> newDepartmentIds)
        {
            try
            {
                if (newDepartmentIds == null)
                {
                    DaoFactory.Wifi.DeleteWifiDepartments(wifiId);
                    return;
                }

                // BƯỚC 1: Lấy danh sách department IDs hiện tại từ database
                var currentDepartmentIds = DaoFactory.Wifi.GetWifiDepartmentIds(wifiId);

                // BƯỚC 2: Convert newDepartmentIds từ string sang int
                var newDepartmentIdsInt = new List<int>();
                if (newDepartmentIds != null && newDepartmentIds.Any())
                {
                    foreach (var deptId in newDepartmentIds)
                    {
                        if (int.TryParse(deptId, out int departmentId) && departmentId > 0)
                        {
                            newDepartmentIdsInt.Add(departmentId);
                        }
                    }
                }

                // BƯỚC 3: Tìm các department IDs cần xóa (có trong current nhưng không có trong new)
                var departmentIdsToDelete = currentDepartmentIds
                    .Except(newDepartmentIdsInt)
                    .ToList();

                // BƯỚC 4: Xóa từng department ID cụ thể
                foreach (var departmentIdToDelete in departmentIdsToDelete)
                {
                    try
                    {
                        DaoFactory.Wifi.DeleteWifiDepartmentByWifiIdAndDepartmentId(
                            wifiId,
                            departmentIdToDelete
                        );
                    }
                    catch (Exception ex)
                    {
                        CommonLogger.DefaultLogger.ErrorFormat(
                            "WifiBo.UpdateWifiDepartmentAssociations - Error deleting department association for WiFi: {0}, Department: {1}, Error: {2}",
                            wifiId,
                            departmentIdToDelete,
                            ex
                        );
                    }
                }

                // BƯỚC 5: Tìm các department IDs cần thêm (có trong new nhưng không có trong current)
                var departmentIdsToAdd = newDepartmentIdsInt.Except(currentDepartmentIds).ToList();

                // BƯỚC 6: Tạo associations mới cho các department IDs cần thêm
                if (departmentIdsToAdd.Any())
                {
                    var departmentIdsToAddString = departmentIdsToAdd
                        .Select(x => x.ToString())
                        .ToList();
                    CreateDepartmentAssociations(wifiId, departmentIdsToAddString);
                }

                if (departmentIdsToDelete.Any() || departmentIdsToAdd.Any())
                {
                    CommonLogger.DefaultLogger.InfoFormat(
                        "WifiBo.UpdateWifiDepartmentAssociations - Updated department associations for WiFi: {0}, Deleted: {1}, Added: {2}",
                        wifiId,
                        departmentIdsToDelete.Count,
                        departmentIdsToAdd.Count
                    );
                }
                else
                {
                    CommonLogger.DefaultLogger.InfoFormat(
                        "WifiBo.UpdateWifiDepartmentAssociations - No department changes for WiFi: {0}",
                        wifiId
                    );
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "WifiBo.UpdateWifiDepartmentAssociations - Error updating department associations: {0}",
                    ex
                );
            }
        }

        /// <summary>
        /// So sánh hai danh sách department IDs để kiểm tra có thay đổi không
        /// </summary>
        /// <param name="list1">Danh sách 1</param>
        /// <param name="list2">Danh sách 2</param>
        /// <returns>True nếu hai danh sách bằng nhau</returns>
        private bool AreDepartmentListsEqual(List<int> list1, List<int> list2)
        {
            if (list1 == null && list2 == null)
                return true;
            if (list1 == null || list2 == null)
                return false;
            if (list1.Count != list2.Count)
                return false;

            // Sort và so sánh
            var sortedList1 = list1.OrderBy(x => x).ToList();
            var sortedList2 = list2.OrderBy(x => x).ToList();

            for (int i = 0; i < sortedList1.Count; i++)
            {
                if (sortedList1[i] != sortedList2[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Cập nhật extra branch associations cho WiFi - So sánh và cập nhật theo yêu cầu
        /// </summary>
        /// <param name="wifiId">WiFi ID</param>
        /// <param name="newExtraBranchIds">Danh sách extra branch IDs mới</param>
        private void UpdateWifiExtraBranchAssociations(int wifiId, List<string> newExtraBranchIds)
        {
            try
            {
                if (newExtraBranchIds == null)
                {
                    var deleteResult = DaoFactory.Wifi.DeleteWifiExtraBranches(wifiId);
                    if (deleteResult <= 0)
                    {
                        CommonLogger.DefaultLogger.WarnFormat(
                            "WifiBo.UpdateWifiExtraBranchAssociations - Failed to delete existing extra branch associations for WiFi: {0}",
                            wifiId
                        );
                    }
                    return;
                }

                // BƯỚC 1: Lấy danh sách extra branch IDs hiện tại từ database
                var currentExtraBranchIds = DaoFactory.Wifi.GetWifiExtraBranchIds(wifiId);

                // BƯỚC 2: Convert newExtraBranchIds từ string sang int
                var newExtraBranchIdsInt = new List<int>();
                if (newExtraBranchIds != null && newExtraBranchIds.Any())
                {
                    foreach (var branchId in newExtraBranchIds)
                    {
                        if (int.TryParse(branchId, out int extraBranchId) && extraBranchId > 0)
                        {
                            newExtraBranchIdsInt.Add(extraBranchId);
                        }
                    }
                }

                // BƯỚC 3: Tìm các branch IDs cần xóa (có trong current nhưng không có trong new)
                var branchIdsToDelete = currentExtraBranchIds.Except(newExtraBranchIdsInt).ToList();

                // BƯỚC 4: Xóa từng branch ID cụ thể
                foreach (var branchIdToDelete in branchIdsToDelete)
                {
                    try
                    {
                        DaoFactory.Wifi.DeleteWifiExtraBranchByWifiIdAndBranchId(
                            wifiId,
                            branchIdToDelete
                        );
                    }
                    catch (Exception ex)
                    {
                        CommonLogger.DefaultLogger.ErrorFormat(
                            "WifiBo.UpdateWifiExtraBranchAssociations - Error deleting extra branch association for WiFi: {0}, Branch: {1}, Error: {2}",
                            wifiId,
                            branchIdToDelete,
                            ex
                        );
                    }
                }

                // BƯỚC 5: Tìm các branch IDs cần thêm (có trong new nhưng không có trong current)
                var branchIdsToAdd = newExtraBranchIdsInt.Except(currentExtraBranchIds).ToList();

                // BƯỚC 6: Tạo associations mới cho các branch IDs cần thêm
                if (branchIdsToAdd.Any())
                {
                    var branchIdsToAddString = branchIdsToAdd.Select(x => x.ToString()).ToList();
                    CreateExtraBranchAssociations(wifiId, branchIdsToAddString);
                }

                if (branchIdsToDelete.Any() || branchIdsToAdd.Any())
                {
                    CommonLogger.DefaultLogger.InfoFormat(
                        "WifiBo.UpdateWifiExtraBranchAssociations - Updated extra branch associations for WiFi: {0}, Deleted: {1}, Added: {2}",
                        wifiId,
                        branchIdsToDelete.Count,
                        branchIdsToAdd.Count
                    );
                }
                else
                {
                    CommonLogger.DefaultLogger.InfoFormat(
                        "WifiBo.UpdateWifiExtraBranchAssociations - No extra branch changes for WiFi: {0}",
                        wifiId
                    );
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "WifiBo.UpdateWifiExtraBranchAssociations - Error updating extra branch associations: {0}",
                    ex
                );
            }
        }

        /// <summary>
        /// So sánh hai danh sách extra branch IDs để kiểm tra có thay đổi không
        /// </summary>
        /// <param name="list1">Danh sách 1</param>
        /// <param name="list2">Danh sách 2</param>
        /// <returns>True nếu hai danh sách bằng nhau</returns>
        private bool AreExtraBranchListsEqual(List<int> list1, List<int> list2)
        {
            if (list1 == null && list2 == null)
                return true;
            if (list1 == null || list2 == null)
                return false;
            if (list1.Count != list2.Count)
                return false;

            // Sort và so sánh
            var sortedList1 = list1.OrderBy(x => x).ToList();
            var sortedList2 = list2.OrderBy(x => x).ToList();

            for (int i = 0; i < sortedList1.Count; i++)
            {
                if (sortedList1[i] != sortedList2[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Helper method để lấy thông tin extra branch object
        /// </summary>
        /// <param name="wifiId">WiFi ID</param>
        /// <param name="branchId">Branch ID</param>
        /// <returns>List<WifiBranchObject></returns>
        private List<WifiBranchObject> GetExtraBranchObject(int wifiId)
        {
            try
            {
                //  SỬ DỤNG Ins_Wifi_ExtraBranch_Get_ByWifiId_Value để lấy thông tin extra branch đầy đủ
                var extraBranchList = DaoFactory.Wifi.GetWifiExtraBranchesByWifiId(wifiId);

                if (extraBranchList != null && extraBranchList.Any())
                {
                    return extraBranchList
                        .Select(branch => new WifiBranchObject
                        {
                            id = branch.BranchId ?? 0,
                            name = branch.BranchName ?? "",
                            color = branch.Color,
                        })
                        .ToList();
                }

                return new List<WifiBranchObject>();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error(
                    $"WifiBo.GetExtraBranchObject Error - WifiId: {wifiId}",
                    ex
                );
                return new List<WifiBranchObject>();
            }
        }

        /// <summary>
        /// Xóa WiFi theo WiFi ID - Xóa associations trước khi xóa WiFi
        /// </summary>
        /// <param name="wifiId">WiFi ID cần xóa</param>
        /// <returns>ApiResult với thông báo kết quả</returns>
        public ApiResult<string> DeleteWifi(int wifiId)
        {
            var response = new ApiResult<string>()
            {
                Data = string.Empty,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (wifiId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "WiFi ID không hợp lệ";
                    return response;
                }

                // BƯỚC 1: XÓA EXTRA BRANCH ASSOCIATIONS - Xóa tất cả extra branch associations trước
                try
                {
                    var deleteExtraBranchResult = DaoFactory.Wifi.DeleteWifiExtraBranches(wifiId);
                    CommonLogger.DefaultLogger.InfoFormat(
                        "WifiBo.DeleteWifi - Deleted {0} extra branch associations for WiFi ID: {1}",
                        deleteExtraBranchResult,
                        wifiId
                    );
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat(
                        "WifiBo.DeleteWifi - Error deleting extra branch associations for WiFi ID: {0}, Error: {1}",
                        wifiId,
                        ex
                    );
                    // Continue với việc xóa department associations
                }

                // BƯỚC 2: XÓA DEPARTMENT ASSOCIATIONS - Xóa tất cả department associations
                try
                {
                    var deleteDepartmentResult = DaoFactory.Wifi.DeleteWifiDepartments(wifiId);
                    CommonLogger.DefaultLogger.InfoFormat(
                        "WifiBo.DeleteWifi - Deleted {0} department associations for WiFi ID: {1}",
                        deleteDepartmentResult,
                        wifiId
                    );
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat(
                        "WifiBo.DeleteWifi - Error deleting department associations for WiFi ID: {0}, Error: {1}",
                        wifiId,
                        ex
                    );
                    // Continue với việc xóa WiFi
                }

                // BƯỚC 3: XÓA WIFI - Gọi Ins_Wifi_Delete_ByWifiId để xóa WiFi
                var deleteResult = DaoFactory.Wifi.DeleteWifiByWifiId(wifiId);

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Xóa WiFi thành công";
                response.Data = $"Đã xóa WiFi ID: {wifiId} và tất cả associations";

                CommonLogger.DefaultLogger.InfoFormat(
                    "WifiBo.DeleteWifi - Successfully deleted WiFi ID: {0} with all associations",
                    wifiId
                );
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (
                    entityEx.InnerException != null
                    && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx
                )
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error(
                        "WifiBo.DeleteWifi - Entity Framework Error",
                        entityEx
                    );
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi database trong quá trình xóa WiFi";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "WifiBo.DeleteWifi - Unexpected Error: {0}",
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xóa WiFi";
            }

            return response;
        }

        /// <summary>
        /// Helper method để lấy thông tin department object
        /// </summary>
        /// <param name="wifiId">WiFi ID</param>
        /// <returns>List<WifiDepartmentObject></returns>
        private List<WifiDepartmentObject> GetDepartmentObject(int wifiId)
        {
            try
            {
                var departmentList = DaoFactory.Wifi.GetWifiDepartmentsByWifiId(wifiId);
                if (departmentList != null && departmentList.Any())
                {
                    return departmentList
                        .Select(dept => new WifiDepartmentObject
                        {
                            id = dept.Id ?? 0,
                            name = dept.DepartmentName ?? "",
                            parent_id = dept.ParentId ?? 0,
                        })
                        .ToList();
                }

                return new List<WifiDepartmentObject>();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error(
                    $"WifiBo.GetDepartmentObject Error - WifiId: {wifiId}",
                    ex
                );
                return new List<WifiDepartmentObject>();
            }
        }
        /// <summary>
        /// Helper method để map request type sang enum value
        /// </summary>
        /// <param name="requestType">Request type (string hoặc int)</param>
        /// <returns>Enum value string</returns>
        private int GetWifiTypeFromRequest(object requestType)
        {
            if (requestType == null)
                return Wifi_type_Enum.wifi.Value();

            // Handle string type
            if (requestType is string)
            {
                var typeString = (string)requestType;
                switch (typeString.ToLower())
                {
                    case "wifi":
                        return Wifi_type_Enum.wifi.Value();
                    case "location":
                        return Wifi_type_Enum.location.Value();
                    default:
                        return Wifi_type_Enum.wifi.Value();
                }
            }

            // Handle int type
            if (requestType is int)
            {
                var typeInt = (int)requestType;
                switch (typeInt)
                {
                    case 1:
                        return Wifi_type_Enum.wifi.Value();
                    case 2:
                        return Wifi_type_Enum.location.Value();
                    default:
                        return Wifi_type_Enum.wifi.Value();
                }
            }

            // Default fallback
            return Wifi_type_Enum.wifi.Value();
        }
    }

}
