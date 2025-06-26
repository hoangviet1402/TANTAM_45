using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.OpenShift;
using BussinessObject.Models.Shift;
using DataAccess;
using DataAccess.Model.OpenShift;
using Logger;
using MyUtility.Extensions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BussinessObject.Bo.TanTamBo
{
    public class OpenShiftBo : BaseBo<DBNull>
    {
        public OpenShiftBo() : base(DaoFactory.OpenShift)
        {
        }

        /// <summary>
        /// Create open shift
        /// </summary>
        public ApiResult<object> Create(int companyId, int createdBy, CreateOpenShiftRequest request)
        {
            var response = new ApiResult<object>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp thông tin đầy đủ.";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.shift_id))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng chọn ca làm việc.";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.working_day))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng chọn ngày làm việc.";
                    return response;
                }

                // Parse working day
                DateTime workingDay;
                if (!DateTime.TryParse(request.working_day, out workingDay))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Ngày làm việc không hợp lệ.";
                    return response;
                }

                // Prepare branch IDs (convert array to comma-separated string)
                string branchIds = "";
                if (request.branch_ids != null && request.branch_ids.Any())
                {
                    branchIds = string.Join(",", request.branch_ids);
                }

                // Prepare position IDs
                string positionIds = "";
                if (request.position_ids != null && request.position_ids.Any())
                {
                    positionIds = string.Join(",", request.position_ids);
                }

                // ✅ OPTIMIZED: Create or reactivate open shift with smart detection
                int openShiftId;
                bool isReactivated;
                DaoFactory.OpenShift.CreateOpenShift(
                    request.shift_id,
                    companyId,
                    request.total_employees,
                    workingDay,
                    request.is_draft == 1,
                    createdBy,
                    branchIds,
                    positionIds,
                    out openShiftId,
                    out isReactivated
                );

                if (openShiftId > 0)
                {
                    response.Data = new
                    {
                        open_shift_id = openShiftId,
                        shift_id = request.shift_id,
                        working_day = workingDay.ToString("yyyy-MM-dd"),
                        total_employees = request.total_employees,
                        is_draft = request.is_draft == 1,
                        branch_count = request.branch_ids?.Count ?? 0,
                        position_count = request.position_ids?.Count ?? 0,
                        is_reactivated = isReactivated // ✅ Include reactivation flag in response
                    };
                    response.Code = ResponseResultEnum.Success.Value();
                    
                    // ✅ SMART UX: Provide contextual messages based on action
                    if (isReactivated)
                    {
                        response.Message = "Khôi phục ca làm mở thành công. Ca làm này đã được cập nhật với thông tin mới.";
                    }
                    else
                    {
                        response.Message = "Tạo ca làm mở thành công.";
                    }
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể tạo ca làm mở. Vui lòng thử lại.";
                }
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                // Handle Entity Framework exceptions that wrap SQL exceptions
                // CommonLogger.DefaultLogger.Error($"OpenShiftBo.Create - Entity Exception: {entityEx.Message}", entityEx);
                
                // Check if inner exception is SqlException
                var innerEx = entityEx.InnerException;
                while (innerEx != null)
                {
                    // CommonLogger.DefaultLogger.Error($"Inner Exception Type: {innerEx.GetType().Name}, Message: {innerEx.Message}");
                    
                    if (innerEx is System.Data.SqlClient.SqlException sqlEx)
                    {
                        response.Code = ResponseResultEnum.InvalidInput.Value();
                        response.Message = sqlEx.Message;
                        return response;
                    }
                    
                    innerEx = innerEx.InnerException;
                }
                
                // If no SqlException found, treat as system error
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + entityEx.Message;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("OpenShiftBo.Create - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get list of open shifts
        /// </summary>
        public ApiResult<object> GetList(int companyId, int employeeId, ListOpenShiftRequest request)
        {
            var response = new ApiResult<object>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp thông tin đầy đủ.";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.start_date) || string.IsNullOrWhiteSpace(request.end_date))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ngày bắt đầu và ngày kết thúc.";
                    return response;
                }

                // Parse dates
                DateTime startDate, endDate;
                if (!DateTime.TryParse(request.start_date, out startDate))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Ngày bắt đầu không hợp lệ.";
                    return response;
                }

                if (!DateTime.TryParse(request.end_date, out endDate))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Ngày kết thúc không hợp lệ.";
                    return response;
                }

                // Get data from database
                var openShifts = DaoFactory.OpenShift.GetList(companyId, startDate, endDate);

                // Group by working day and create the response format
                var groupedData = new System.Collections.Generic.List<System.Collections.Generic.List<OpenShiftListItemDto>>();
                
                // Create a list for each day in the range
                var currentDate = startDate;
                while (currentDate <= endDate)
                {
                    var dayData = openShifts
                        .Where(os => DateTime.TryParse(os.working_day, out var workingDay) && workingDay.Date == currentDate.Date)
                        .Select(os => new OpenShiftListItemDto
                        {
                            id = os.id.ToString(),
                            shift_name = os.shift_name,
                            total_employees = os.total_employees,
                            shift_id = os.shift_id,
                            start_time = $"{os.start_time:yyyy-MM-dd} 08:30",
                            end_time = $"{os.end_time:yyyy-MM-dd} 17:30",
                            working_day = os.working_day,
                            timezone = os.timezone,
                            is_draft = os.is_draft,
                            status = new OpenShiftStatusDto
                            {
                                not_available = DateTime.TryParse(os.working_day, out var workingDay) && DateTime.Now.Date < workingDay.Date ? 1 : os.not_available,
                                status_color = new System.Collections.Generic.List<string> { "#838BA3", "#EBEBEB" }
                            },
                            registered_employees = os.registered_employees
                        })
                        .ToList();

                    groupedData.Add(dayData);
                    currentDate = currentDate.AddDays(1);
                }

                response.Data = groupedData;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("OpenShiftBo.GetList - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get shift list by working day
        /// </summary>
        public ApiResult<object> GetShiftListByWorkingDay(int companyId, ShiftListByWorkingDayRequest request)
        {
            var response = new ApiResult<object>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp thông tin đầy đủ.";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.working_day))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ngày làm việc.";
                    return response;
                }

                // Parse working day
                DateTime workingDay;
                if (!DateTime.TryParse(request.working_day, out workingDay))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Ngày làm việc không hợp lệ.";
                    return response;
                }

                // Validate status value (should be 0 or 1)
                if (request.status != 0 && request.status != 1)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Trạng thái không hợp lệ. Chỉ chấp nhận 0 (inactive) hoặc 1 (active).";
                    return response;
                }

                // Get data from database using stored procedure
                var shifts = DaoFactory.OpenShift.GetShiftListByWorkingDay(companyId, request.page, request.status, workingDay, request.is_all);

                // Transform data to match expected response format
                var responseData = shifts.Select(shift => new ShiftListByWorkingDayItemDto
                {
                    id = shift.Id.ToString(),
                    name = shift.ShiftName,
                    start_time = $"{workingDay:yyyy-MM-dd} 08:00", // Default start time since stored procedure doesn't return time
                    end_time = $"{workingDay:yyyy-MM-dd} 17:30",   // Default end time since stored procedure doesn't return time
                    timezone = shift.TimeZone ?? "Asia/Saigon"
                }).ToList();

                response.Data = responseData;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                // Handle Entity Framework exceptions that wrap SQL exceptions
                // CommonLogger.DefaultLogger.Error($"OpenShiftBo.Create - Entity Exception: {entityEx.Message}", entityEx);
                
                // Check if inner exception is SqlException
                var innerEx = entityEx.InnerException;
                while (innerEx != null)
                {
                    // CommonLogger.DefaultLogger.Error($"Inner Exception Type: {innerEx.GetType().Name}, Message: {innerEx.Message}");
                    
                    if (innerEx is System.Data.SqlClient.SqlException sqlEx)
                    {
                        response.Code = ResponseResultEnum.InvalidInput.Value();
                        response.Message = sqlEx.Message;
                        return response;
                    }
                    
                    innerEx = innerEx.InnerException;
                }
                
                // If no SqlException found, treat as system error
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + entityEx.Message;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("OpenShiftBo.GetShiftListByWorkingDay - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Delete open shift
        /// </summary>
        public ApiResult<object> Delete(int companyId, int deletedBy, int openShiftId)
        {
            var response = new ApiResult<object>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input
                if (openShiftId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID ca làm mở không hợp lệ.";
                    return response;
                }

                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Công ty không hợp lệ.";
                    return response;
                }

                if (deletedBy <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Người thực hiện không hợp lệ.";
                    return response;
                }

                // Call DAO method to delete
                bool deleteResult = DaoFactory.OpenShift.DeleteOpenShift(openShiftId, companyId, deletedBy);

                if (deleteResult)
                {
                    response.Data = new
                    {
                        open_shift_id = openShiftId,
                        deleted_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        deleted_by = deletedBy
                    };
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Xóa ca làm mở thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể xóa ca làm mở. Vui lòng thử lại.";
                }
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                // ✅ CORRECT SQL ERROR MESSAGE EXTRACTION
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error($"OpenShiftBo.Delete - Entity Exception", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi khi xóa ca làm mở.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("OpenShiftBo.Delete - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get open shift detail - OPTIMIZED VERSION (Single DB call)
        /// Returns DAO models directly
        /// </summary>
        public ApiResult<OpenShiftCompleteDetailResult> GetDetail(int companyId, int openShiftId)
        {
            var response = new ApiResult<OpenShiftCompleteDetailResult>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // ✅ CLIENT VALIDATION
                if (openShiftId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID ca làm mở không hợp lệ.";
                    return response;
                }

                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Công ty không hợp lệ.";
                    return response;
                }

                // ✅ SINGLE DAO CALL - Lấy tất cả dữ liệu trong 1 lần gọi
                var completeResult = DaoFactory.OpenShift.GetCompleteDetail(openShiftId, companyId);

                if (completeResult == null || string.IsNullOrEmpty(completeResult.id))
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = "Không tìm thấy thông tin ca làm mở.";
                    return response;
                }

                // ✅ RETURN DAO MODELS DIRECTLY - No DTO transformation needed
                response.Data = completeResult;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy thông tin ca làm mở thành công.";
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                // ✅ EXTRACT SQL ERROR MESSAGE
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error($"GetDetail EntityCommandExecutionException. OpenShiftId: {openShiftId}", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi khi lấy thông tin ca làm mở.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"GetDetail Exception. OpenShiftId: {openShiftId}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
            }

            return response;
        }

        /// <summary>
        /// Publish multiple open shifts
        /// </summary>
        public ApiResult<object> Publish(int companyId, int publishedBy, PublishOpenShiftRequest request)
        {
            var response = new ApiResult<object>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // ✅ CLIENT VALIDATION
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp thông tin đầy đủ.";
                    return response;
                }

                if (request.ids == null || !request.ids.Any())
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp danh sách ID ca làm mở.";
                    return response;
                }

                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Công ty không hợp lệ.";
                    return response;
                }

                if (publishedBy <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Người thực hiện không hợp lệ.";
                    return response;
                }

                // Convert IDs list to comma-separated string
                string openShiftIds = string.Join(",", request.ids.Where(id => !string.IsNullOrWhiteSpace(id)));

                if (string.IsNullOrWhiteSpace(openShiftIds))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Danh sách ID ca làm mở không hợp lệ.";
                    return response;
                }

                // ✅ CALL DAO - Use Entity Framework context pattern
                int updatedCount = DaoFactory.OpenShift.PublishOpenShift(openShiftIds, companyId, publishedBy);

                if (updatedCount > 0)
                {
                    response.Data = new
                    {
                        updated_count = updatedCount,
                        open_shift_ids = request.ids,
                        published_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        published_by = publishedBy
                    };
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = $"Xuất bản thành công {updatedCount} ca làm mở.";
                }
                else
                {
                    // ✅ Handle case where no records were updated 
                    // This happens when all OpenShifts are already published (IsDraft = 0)
                    response.Data = new
                    {
                        updated_count = 0,
                        open_shift_ids = request.ids,
                        already_published = true,
                        checked_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    response.Code = ResponseResultEnum.Success.Value(); // Changed to Success since it's not an error
                    response.Message = "Tất cả ca làm mở đã được xuất bản trước đó.";
                }
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                // ✅ CORRECT SQL ERROR MESSAGE EXTRACTION
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error($"OpenShiftBo.Publish - Entity Exception", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi khi xuất bản ca làm mở.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("OpenShiftBo.Publish - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
            }

            return response;
        }
    }
} 