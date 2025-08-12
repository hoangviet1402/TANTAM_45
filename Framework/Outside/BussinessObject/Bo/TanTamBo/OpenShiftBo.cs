using BussinessObject.Enum;
using BussinessObject.Helper;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.OpenShift;
using BussinessObject.Models.Shift;
using DataAccess;
using DataAccess.Model.OpenShift;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

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

                // Create or reactivate open shift with smart detection
                int openShiftId;
                bool isReactivated;
                DaoFactory.OpenShift.CreateOpenShift(
                    request.shift_id,
                    companyId,
                    request.total_employees,
                    workingDay,
                    request.is_draft == 1,
                    createdBy,
                    out openShiftId,
                    out isReactivated
                );

                // Call separate stored procedure for each branch
                if (openShiftId > 0 && request.branch_ids != null && request.branch_ids.Any())
                {
                    foreach (var branchId in request.branch_ids)
                    {
                        int branchIdInt;
                        if (int.TryParse(branchId, out branchIdInt))
                        {
                            DaoFactory.OpenShift.AddBranchToOpenShift(openShiftId, branchIdInt, companyId);
                        }
                    }
                }

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
                        is_reactivated = isReactivated // ✅ Include reactivation flag in response
                    };
                    response.Code = ResponseResultEnum.Success.Value();
                    
                    // Provide contextual messages based on action
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
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("OpenShiftBo.Create - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get list of open shifts - OPTIMIZED VERSION
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

                if (!DateTime.TryParse(request.start_date, out DateTime startDate))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Ngày bắt đầu không hợp lệ.";
                    return response;
                }

                if (!DateTime.TryParse(request.end_date, out DateTime endDate))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Ngày kết thúc không hợp lệ.";
                    return response;
                }

                if (startDate > endDate)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Ngày bắt đầu không được lớn hơn ngày kết thúc.";
                    return response;
                }

                var openShifts = DaoFactory.OpenShift.GetList(companyId, startDate, endDate);
                
                if (openShifts == null || !openShifts.Any())
                {
                    response.Data = new List<List<OpenShiftListItemDto>>();
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.Success.Text();
                    return response;
                }

                var groupedData = ProcessOpenShiftsOptimized(openShifts, startDate, endDate);

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
        /// </summary>
        private List<List<OpenShiftListItemDto>> ProcessOpenShiftsOptimized(
            List<Ins_OpenShift_List_Result> openShifts, 
            DateTime startDate, 
            DateTime endDate)
        {
            var groupedData = new List<List<OpenShiftListItemDto>>();
            var totalDays = (endDate - startDate).Days + 1;
            groupedData.Capacity = totalDays;

            var shiftsByDate = openShifts
                .GroupBy(os => 
                {
                    if (DateTime.TryParse(os.working_day, out DateTime workingDay))
                        return workingDay.Date;
                    return DateTime.MinValue;
                })
                .ToDictionary(g => g.Key, g => g.ToList());

            var currentDate = startDate;
            while (currentDate <= endDate)
            {
                var dayData = new List<OpenShiftListItemDto>();
                
                if (shiftsByDate.TryGetValue(currentDate.Date, out var dayShifts))
                {
                    var usedIds = new HashSet<string>();
                    var usedShiftNames = new HashSet<string>();
                    
                    foreach (var os in dayShifts)
                    {
                        var openShiftItem = CreateOptimizedOpenShiftItem(
                            os, currentDate, usedIds, usedShiftNames);
                        dayData.Add(openShiftItem);
                    }
                }

                groupedData.Add(dayData);
                currentDate = currentDate.AddDays(1);
            }

            return groupedData;
        }

        /// <summary>
        /// </summary>
        private OpenShiftListItemDto CreateOptimizedOpenShiftItem(
            Ins_OpenShift_List_Result os, 
            DateTime currentDate, 
            HashSet<string> usedIds, 
            HashSet<string> usedShiftNames)
        {
            var originalId = os.id.ToString();
            var uniqueId = GenerateUniqueValue(originalId, usedIds);

            var originalShiftName = os.shift_name;
            var uniqueShiftName = GenerateUniqueValue(originalShiftName, usedShiftNames);

            int shiftIdInt = 0;
            int.TryParse(os.shift_id, out shiftIdInt);
            var timeConfig = ShiftTimeConfigHelper.GetShiftTimeConfiguration(shiftIdInt);

            var isNotAvailable = currentDate.Date > DateTime.Now.Date ? 1 : os.not_available;

            return new OpenShiftListItemDto
            {
                id = uniqueId,
                shift_name = uniqueShiftName,
                total_employees = os.total_employees,
                shift_id = os.shift_id,
                start_time = $"{os.start_time} {timeConfig.StartTime}",
                end_time = $"{os.end_time} {timeConfig.EndTime}",
                working_day = os.working_day,
                timezone = os.timezone,
                is_draft = os.is_draft,
                status = new OpenShiftStatusDto
                {
                    not_available = isNotAvailable,
                    status_color = new List<string> { "#838BA3", "#EBEBEB" }
                },
                registered_employees = os.registered_employees
            };
        }

        /// <summary>
        /// </summary>
        private string GenerateUniqueValue(string originalValue, HashSet<string> usedValues)
        {
            if (!usedValues.Contains(originalValue))
            {
                usedValues.Add(originalValue);
                return originalValue;
            }

            var counter = 1;
            var uniqueValue = $"{originalValue}_{counter}";
            
            while (usedValues.Contains(uniqueValue))
            {
                counter++;
                uniqueValue = $"{originalValue}_{counter}";
            }
            
            usedValues.Add(uniqueValue);
            return uniqueValue;
        }

        /// <summary>
        /// Get shift list by working day
        /// </summary>
        public ApiResult<ListShiftResponse> GetShiftListByWorkingDay(int userId, ShiftListByWorkingDayRequest request)
        {
            if (request == null)
            {
                return new ApiResult<ListShiftResponse>()
                {
                    Data = new ListShiftResponse(),
                    Code = ResponseResultEnum.InvalidInput.Value(),
                    Message = "Vui lòng cung cấp thông tin đầy đủ."
                };
            }

            if (request.status != 0 && request.status != 1)
            {
                return new ApiResult<ListShiftResponse>()
                {
                    Data = new ListShiftResponse(),
                    Code = ResponseResultEnum.InvalidInput.Value(),
                    Message = "Trạng thái không hợp lệ. Chỉ chấp nhận 0 (inactive) hoặc 1 (active)."
                };
            }

            DateTime workingDay;
            var validationResult = ShiftListHelper.ValidateWorkingDayInput(request.working_day, out workingDay);
            if (validationResult != null)
            {
                return validationResult;
            }

            return ShiftListHelper.GetShiftListByWorkingDayCommon(userId, workingDay, true);
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

                var mainDetail = DaoFactory.OpenShift.GetDetail(openShiftId, companyId);
                if (mainDetail == null)
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = "Không tìm thấy thông tin ca làm mở.";
                    return response;
                }

                var result = new OpenShiftCompleteDetailResult();
                result.id = mainDetail.id.ToString();
                result.shift_name = mainDetail.shift_name;
                result.total_employees = mainDetail.total_employees;
                result.shift_id = mainDetail.shift_id;
                result.start_time = mainDetail.start_time;
                result.end_time = mainDetail.end_time;
                result.working_day = mainDetail.working_day;
                result.timezone = mainDetail.timezone;
                result.is_draft = mainDetail.is_draft;
                
                result.status = new OpenShiftStatusModel
                {
                    not_available = mainDetail.not_available,
                    status_color = new List<string> { "#838BA3", "#EBEBEB" }
                };

                var branches = DaoFactory.OpenShift.GetBranches(openShiftId, companyId);
                foreach (var branch in branches)
                {
                    result.branches.Add(new OpenShiftBranchResult
                    {
                        id = branch.id.ToString(),
                        name = branch.name
                    });
                }

                var positions = DaoFactory.OpenShift.GetPositions(openShiftId, companyId);
                foreach (var position in positions)
                {
                    result.positions.Add(new OpenShiftPositionResult
                    {
                        id = position.id.ToString(),
                        name = position.name
                    });
                }

                var users = DaoFactory.OpenShift.GetUsers(openShiftId, companyId);
                foreach (var user in users)
                {
                    result.users.Add(new OpenShiftUserResult
                    {
                        id = user.id,
                        name = user.name,
                        employee_code = user.employee_code,
                        position = user.position,
                        avatar = user.avatar,
                        status = user.status,
                        registered_at = user.registered_at
                    });
                }

                response.Data = result;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy thông tin ca làm mở thành công.";
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

                // Call DAO multiple times - one for each OpenShift ID
                int totalUpdatedCount = 0;
                var validIds = request.ids.Where(id => !string.IsNullOrWhiteSpace(id)).ToList();

                if (!validIds.Any())
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Danh sách ID ca làm mở không hợp lệ.";
                    return response;
                }

                // Publish each OpenShift individually
                foreach (var id in validIds)
                {
                    int openShiftId;
                    if (int.TryParse(id, out openShiftId))
                    {
                        int result = DaoFactory.OpenShift.PublishOpenShiftSingle(openShiftId, companyId, publishedBy);
                        if (result > 0)
                        {
                            totalUpdatedCount++;
                        }
                    }
                }

                response.Data = new
                {
                    updated_count = totalUpdatedCount,
                    open_shift_ids = request.ids,
                    published_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    published_by = publishedBy
                };
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = $"Xuất bản thành công {totalUpdatedCount} ca làm mở.";
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