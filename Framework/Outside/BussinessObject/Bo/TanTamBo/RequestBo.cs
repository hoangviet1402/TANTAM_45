using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.RequestFor;
using DataAccess;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BussinessObject.Bo.TanTamBo
{
    public class RequestForBo : BaseBo<DBNull>
    {
        public RequestForBo()
            : base(DaoFactory.TanTam)
        {
        }

        public ApiResult<RequestTypeResponse> RequestTypes_GetAll(int companyID)
        {
            var response = new ApiResult<RequestTypeResponse>
            {
                Data = new RequestTypeResponse(),
                Code = ResponseResultEnum.NoData.Value(),
                Message = ResponseResultEnum.NoData.Text(),
            };

            try
            {
                var Tutorials = DaoFactory.RequestFor.RequestTypes_GetAll(companyID);
                response.Data.Meta = new List<string>();
                response.Data.Items = Tutorials.Select(x =>new RequestTypeResponse_Items() {
                        Value = x.ValueRequest,
                        Id = x.Id,
                        Label = x.Label,
                        Setting = new RequestTypeResponse_Items_Setting() {
                            Alias = x.Alias,
                            AllowRequestWhenShiftStillWorking = x.AllowRequestWhenShiftStillWorking,
                            DayRequiredBeforeSendRequest = x.DayRequiredBeforeSendRequest,
                            EnableAutoApproval = x.EnableAutoApproval,
                            EnableEmployeeDeleteRequest = x.EnableEmployeeDeleteRequest,
                            IsDisabledForEmployees = x.IsDisabledForEmployees ,
                            IsDisabledForManager = x.IsDisabledForManager,
                            IsShortcut = x.IsShortcut,
                            MinimumDay = x.MinimumDay,
                            SerialPrefix = x.SerialPrefix,
                            SerialSuffix = x.SerialSuffix
                        }
                }).ToList();
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("UserTutorials_Complete - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public ApiResult<RequestForResponse> Request_CreateRequestWithShift(RequestForRequest request, int accountMapID, int companyId)
        {
            var response = new ApiResult<RequestForResponse>
            {
                Data = new RequestForResponse(),
                Code = ResponseResultEnum.NoData.Value(),
                Message = ResponseResultEnum.NoData.Text(),
            };

            try
            {
                if(request == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();                    
                    return response;
                }

                if (request.ShiftIds == null || request.ShiftIds.Any() == false)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "chưa chọn ca";
                    return response;
                }

                if (string.IsNullOrEmpty(request.FromDate) || string.IsNullOrEmpty(request.ToDate))
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "chưa chọn ngày bắt đầu hoặc kết thúc nghỉ";
                    return response;
                }

                var fromDate = new DateTime();
                var toDate = new DateTime();
                List<int> listPayRollID  = new List<int>();
                fromDate = DateTime.ParseExact(
                           request.FromDate,
                           "yyyy-MM-dd",
                           CultureInfo.InvariantCulture
                       );
                toDate = DateTime.ParseExact(
                            request.ToDate,
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture
                        );
                List<Ins_Shift_GetSimple_Result> company_Shift = DaoFactory.Shift.Shift_GetSimple(companyId, -1);
                if(company_Shift == null || company_Shift.Any() == false)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Doanh nghiệp chưa tạo ca";
                    return response;
                }
                company_Shift = company_Shift.Where(x => request.ShiftIds.Any(y => y == x.ShiftId)).ToList();
                if (company_Shift == null || company_Shift.Any() == false)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Ca bạn chọn không tồn tại";
                    return response;
                }

                var user_Payroll = DaoFactory.Payroll.GetListByAccountMapID(accountMapID, fromDate, toDate).
                    Where(x => x.PayrollStatus == 1 && request.ShiftIds.Any(y => y == x.ShiftId) ).ToList();
                
                var totalDay = 1;
                var branchId = DaoFactory.Employee.GetEmployeeObjectData(accountMapID).BranchObjId;
                var employeeDetail = DaoFactory.Employee.GetEmployeeDetail(accountMapID);

                var requestID = DaoFactory.RequestFor.Request_CreateRequestWithShift(request.TypeId,
                    request.Status,
                    accountMapID,
                    request.Reason,
                    fromDate, toDate,
                    fromDate, toDate,
                    //user_Payroll.FirstOrDefault().StartTime.GetValueOrDefault(), user_Payroll.FirstOrDefault().EndTime.GetValueOrDefault(),
                    totalDay,
                    0,
                    branchId ?? 0,
                    true,
                    true,
                    user_Payroll.FirstOrDefault().AssignmentUserID,
                    string.Join(",", user_Payroll.Select(x => x.PayrollUserID).ToList()));

                response.Data = new RequestForResponse()
                {
                    Id = requestID,
                    Name = employeeDetail.FullName,
                    UserId = accountMapID,
                    FromDate = fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    ToDate = toDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    StatusId = 0,
                    Type = 0,
                    TotalDay = 1,
                    Reason = request.Reason,
                    Tel = string.Format("{0}{1}",employeeDetail.PhoneCode, employeeDetail.Phone),
                    LeaveWageByLeaveCoefficient = 0, 
                    ExchangeContent = "12e",
                    CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ShiftIds = request.ShiftIds,
                    StartTime = user_Payroll.FirstOrDefault().StartTime.GetValueOrDefault().ToString("HH:mm"),
                    EndTime = user_Payroll.FirstOrDefault().EndTime.GetValueOrDefault().ToString("HH:mm"),
                    Shifts = company_Shift.Select(x => new RequestForResponse_Shift() {
                        Id = x.ShiftId,
                        Name = x.ShiftName
                    }).ToList(),
                    WorkingdayConfig =  new RequestForResponse_WorkingdayConfig() {
                        Id = 1,
                        Name = "Nghỉ phép tiêu chuẩn",
                        Code = "NPTC",
                        TypeOnleave = 1
                    },
                    TypeObj = new RequestForResponse_TypeObj() {
                        Id = 1,
                        Name = "in_day",
                        Type = "day_off_type",
                        Value = 0,
                        NumberDay = 1
                    },
                    Status = new RequestForResponse_StatusObj()
                    {
                        Id = 1,
                        Name = "Chờ phê duyệt",
                        Value = 0,
                        Type = "allowance_status_type",
                        Title = "Chờ phê duyệt",
                        IsDefault = 1,
                        IndexNum = 1
                    }
                };


                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("Request_CreateRequestWithShift - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }
    }
}