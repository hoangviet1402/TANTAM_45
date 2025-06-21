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
                List<Ins_CompanyDepartment_CreateInAllBranchId_Result> departmentId;
                var now = DateTime.Now;
                foreach (var item in request.Name)
                {
                    departmentId = DaoFactory.Department.CreateDepartmentInAllBranches(
                        item,
                        companyId,
                        StringCommon.NormalizeText(item, "-"),
                        StringCommon.NormalizeText(item, "_").ToUpper());

                    response.Data.Add(
                        new CreateDepartmentResponse()
                        {
                            Id = 1,
                            Name = item,
                            CreatedAt = now.ToString("yyyy-MM-dd HH:mm:ss"),
                            BranchIds = departmentId.Select(x => x.BranchId ?? 0).Distinct().ToList(),
                            SortIndex = 0,
                            Alias = StringCommon.NormalizeText(item, "-"),
                            Code = StringCommon.NormalizeText(item, "_").ToUpper(),
                            ShopId = companyId,
                            Key = "1",
                            Value = "1",
                            Title = item
                        }
                    );
                }
                if (response.Data != null && response.Data.Any())
                {
                    var result = DaoFactory.Company.UpdateCompanyStep(companyId, SetupStepEnum.ONBOARDING_CREATE_DEPARTMENT.Value());
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Tạo phòng ban thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Tạo phòng ban thất bại";
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
                        BranchIds = dataSQL.Where(x => x.Id == d.Id).Select(x => x.BranchId).Distinct().ToList(),
                        Branchs = dataSQL.Where(x => x.Id == d.Id).Select(x => new DepartmentsBranchsResponseList
                        {
                            Id = x.BranchId,
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
