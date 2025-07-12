using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Company;
using DataAccess;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Bo.TanTamBo
{
    public class DepartmentBo : BaseBo<DBNull>
    {
        public DepartmentBo()
            : base(DaoFactory.Department)
        {
        }

        public ApiResult<List<CreateDepartmentResponse>> SetupCompany_CreateDepartmentAllBranchAsync(int companyId, CreateDepartmentRequest request)
        {
            var response = new ApiResult<List<CreateDepartmentResponse>>()
            {
                Data = new List<CreateDepartmentResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };
            try
            {
                var departmentId = 0;
                var now = DateTime.Now;
                var total = 0;

                 if (request.IsOnboarding == 1)
                {
                    var dataBrandID = DaoFactory.Branches.GetAllBranchs(companyId, out total).OrderBy(x=>x.BranchId).ToList();
                    foreach (var item_dataBrandID in dataBrandID)
                    {
                        foreach (var item in request.Name)
                        {
                            departmentId = DaoFactory.Department.CreateDepartmentInAllBranches_Simple(
                                 item,
                                 companyId,
                                 item_dataBrandID.BranchId,
                                 StringCommon.NormalizeText(item, "-"),
                                 StringCommon.NormalizeText(item, "_").ToUpper(),
                                 request.IsOnboarding
                                 );
                            if (departmentId > 0 && request.IsOnboarding == 1)
                            {
                                response.Data.Add(
                                    new CreateDepartmentResponse()
                                    {
                                        Id = departmentId,
                                        Name = item,
                                        CreatedAt = now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        BranchIds = dataBrandID.Select(x => x.BranchId).ToList(),
                                        SortIndex = 0,
                                        Alias = StringCommon.NormalizeText(item, "-"),
                                        Code = StringCommon.NormalizeText(item, "_").ToUpper(),
                                        ShopId = companyId,
                                        Key = "1",
                                        Value = "1",
                                        Title = item
                                    }
                                );
                                departmentId = 0;
                            }
                        }
                        //ban đầu mà tạo nhiều phong ban thi chi co chi nhanh dau tiên là măc định
                        request.IsOnboarding = 0;
                    }
                    
                    if (response.Data != null && response.Data.Any())
                    {
                        var result = DaoFactory.Company.UpdateCompanyStep(companyId, SetupStepEnum.ONBOARDING_CREATE_DEPARTMENT.Value());
                        response.Code = ResponseResultEnum.Success.Value();
                        response.Message = "Tạo phòng ban thành công";
                    }
                    else
                    {
                        response.Code = ResponseResultEnum.NoData.Value();
                        response.Message = "Tạo phòng ban thất bại";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("SetupCompany_CreateBranches companyId {0} EX:", companyId, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Tạo phòng ban thất bại";
            }

            return response;
        }

        public ApiResult<List<CreateDepartmentResponse>> CompanyGetAllDepartment(int companyId)
        {
            var response = new ApiResult<List<CreateDepartmentResponse>>()
            {
                Data = new List<CreateDepartmentResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var dataSQL = DaoFactory.Department.GetAllDepartments(companyId);
                var departments = dataSQL.Select(x => new
                {
                    x.Id,
                    x.DepartmentName,
                    x.Description,
                    x.CreatedAt,
                    x.ParentId,
                    x.SortIndex,
                    x.Alias,
                    x.Code,
                    x.IsHead
                }).Distinct().ToList();

                if (departments.Any())
                {
                    response.Data = departments.Select(d => new CreateDepartmentResponse
                    {
                        Id = d.Id,
                        Name = d.DepartmentName,
                        CreatedAt = d.CreatedAt.HasValue ? d.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                        BranchIds = dataSQL.Where(x => x.Id == d.Id).Select(x => x.BranchId ?? 0).Distinct().ToList(),
                        Branchs = dataSQL.Where(x => x.Id == d.Id).Select(x => new DepartmentsBranchsResponseList
                        {
                            Id = x.BranchId ??  0,
                            Name = x.BranchName,
                            Color = x.Color
                        }).ToList(),
                        Description = d.Description,
                        ParentId = d.ParentId,
                        SortIndex = d.SortIndex ?? 0,
                        Code = d.Code,
                        Alias = d.Alias,
                        Key = d.Id.ToString(),
                        Value = d.DepartmentName,
                        Title = d.DepartmentName,
                        Parent = d.ParentId,
                        IsHead = d.IsHead ?? false,
                    }).ToList();

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy danh sách phòng ban thành công";
                    return response;
                }

                response.Code = ResponseResultEnum.NoData.Value();
                response.Message = "Không có dữ liệu";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyGetAllDepartment companyId {0} EX:", companyId, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lấy danh sách phòng ban thất bại";
            }

            return response;
        }
    }
}
