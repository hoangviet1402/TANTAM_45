using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.User;
using DataAccess;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BussinessObject.Bo.TanTamBo
{
    public class UserBo : BaseBo<DBNull>
    {
        public UserBo() : base(DaoFactory.User)
        {
        }

        /// <summary>
        /// Get user list with pagination and filtering
        /// </summary>
        public ApiResult<UserListResponse> GetUserListAsync(UserListRequest request)
        {
            var response = new ApiResult<UserListResponse>
            {
                Data = new UserListResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Call stored procedure to get user list
                var usersFromDb = DaoFactory.User.GetUserList(
                    request.Page,
                    request.Limit,
                    request.Search,
                    request.Status.HasValue ? request.Status.Value == 1 : (bool?)null,
                    request.DepartmentId,
                    request.RoleId
                );

                if (usersFromDb != null && usersFromDb.Any())
                {
                    // Map from DB result to DTO
                    response.Data.Items = usersFromDb.Select(u => new UserDto
                    {
                        UserId = u.UserId,
                        Email = u.Email,
                        Phone = u.Phone,
                        FullName = u.FullName,
                        IsActive = u.IsActive,
                        Role = u.Role,
                        CompanyId = u.CompanyId,
                        CompanyFullName = string.Empty, // Not available in list result
                        EmployeeCode = u.EmployeeCode,
                        BirthDate = u.BirthDate,
                        Gender = u.Gender
                    }).ToList();

                    int page = request.Page ?? 1;
                    int limit = request.Limit ?? 10;

                    // Note: The stored procedure should handle pagination and return total count
                    // For now, we calculate based on returned data
                    response.Data.Meta = new MetaResponse
                    {
                        Total = usersFromDb.Count, // This should come from DB
                        Count = response.Data.Items.Count,
                        PerPage = limit,
                        CurrentPage = page,
                        TotalPages = (int)Math.Ceiling((double)usersFromDb.Count / limit),
                    };

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy danh sách người dùng thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không có dữ liệu";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("UserBo.GetUserListAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get user detail by userId and companyId
        /// </summary>
        public ApiResult<UserDetailResponse> GetUserDetailAsync(UserDetailRequest request)
        {
            var response = new ApiResult<UserDetailResponse>
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Call stored procedure to get user detail
                var userFromDb = DaoFactory.User.GetUserDetail(request.UserId, request.CompanyId);

                if (userFromDb != null)
                {
                    // Map from DB result to DTO
                    response.Data = new UserDetailResponse
                    {
                        UserId = userFromDb.UserId,
                        Email = userFromDb.Email,
                        Phone = userFromDb.Phone,
                        PhoneCode = userFromDb.PhoneCode,
                        PhoneFull = userFromDb.PhoneFull,
                        AccountIsActive = userFromDb.AccountIsActive,
                        AccountCreatedAt = userFromDb.AccountCreatedAt,
                        EmployeeAccountMapId = userFromDb.EmployeeAccountMapId,
                        CompanyId = userFromDb.CompanyId,
                        CompanyFullName = userFromDb.CompanyFullName,
                        FullName = userFromDb.FullName,
                        UserIsActive = userFromDb.UserIsActive,
                        IsNewUser = userFromDb.IsNewUser,
                        NeedSetPassword = userFromDb.NeedSetPassword,
                        UserCreatedAt = userFromDb.UserCreatedAt,
                        Role = userFromDb.Role,
                        EmployeeInfoId = userFromDb.EmployeeInfoId,
                        EmployeeCode = userFromDb.EmployeeCode,
                        BirthDate = userFromDb.BirthDate,
                        Gender = userFromDb.Gender,
                        ContactAddress = userFromDb.ContactAddress
                    };

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy thông tin người dùng thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không tìm thấy thông tin người dùng";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("UserBo.GetUserDetailAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Simple method to get user list - legacy support
        /// </summary>
        public List<Ins_User_GetList_Result> GetUserList(int? page, int? limit, string search, bool? status, int? departmentId, int? roleId)
        {
            try
            {
                return DaoFactory.User.GetUserList(page, limit, search, status, departmentId, roleId);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("UserBo.GetUserList - Error occurred", ex);
                return new List<Ins_User_GetList_Result>();
            }
        }

        /// <summary>
        /// Simple method to get user detail - legacy support
        /// </summary>
        public Ins_User_GetDetail_Result GetUserDetail(int userId, int companyId)
        {
            try
            {
                return DaoFactory.User.GetUserDetail(userId, companyId);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("UserBo.GetUserDetail - Error occurred", ex);
                return new Ins_User_GetDetail_Result();
            }
        }
    }
} 