using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Company;
using DataAccess;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using Newtonsoft.Json;
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
                    var dataBrandID = DaoFactory.Branches.GetAllBranchs(companyId, out total).OrderBy(x => x.BranchId).ToList();

                    foreach (var item in request.Name)
                    {
                        departmentId = DaoFactory.Department.CreateDepartmentInAllBranches_Simple(
                             item,
                             companyId,
                             0,
                             StringCommon.NormalizeText(item, "-"),
                             StringCommon.NormalizeText(item, "_").ToUpper(),
                             request.IsOnboarding
                             );
                        foreach (var item_dataBrandID in dataBrandID)
                        {
                            DaoFactory.Department.CreateRelate(departmentId, item_dataBrandID.BranchId);

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
                                // departmentId = 0;
                            }
                        }
                    }
                    //ban đầu mà tạo nhiều phong ban thi chi co chi nhanh dau tiên là măc định
                    // request.IsOnboarding = 0;
                } else {
                    
                    if (request.BranchIds == null || request.BranchIds.Count <= 0)
                    {
                        request.BranchIds = DaoFactory.Branches.GetAllBranchs(companyId, out total).Select(x => x.BranchId).ToList();
                    }

                    foreach (var item in request.Name)
                    {
                        departmentId = DaoFactory.Department.CreateDepartmentInAllBranches_Simple(
                            item,
                            companyId,
                            0,
                            StringCommon.NormalizeText(item, "-"),
                            StringCommon.NormalizeText(item, "_").ToUpper(),
                            request.IsOnboarding
                        );

                        if (departmentId <= 0)
                        {
                            continue;
                        }

                        foreach (var item_branchId in request.BranchIds)
                        {
                            DaoFactory.Department.CreateRelate(departmentId, item_branchId);
                        }

                        response.Data.Add(
                            new CreateDepartmentResponse()
                            {
                                Id = departmentId,
                                Name = request.Name.FirstOrDefault(),
                                CreatedAt = now.ToString("yyyy-MM-dd HH:mm:ss"),
                                BranchIds = request.BranchIds,
                                SortIndex = 0,
                                Alias = StringCommon.NormalizeText(request.Name.FirstOrDefault(), "-"),
                                Code = StringCommon.NormalizeText(request.Name.FirstOrDefault(), "_").ToUpper(),
                                ShopId = companyId,
                                Key = "1",
                                Value = "1",
                                Title = request.Name.FirstOrDefault()
                            }
                        );
                    }
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
                    var departmentResponses = new List<CreateDepartmentResponse>();

                    foreach (var d in departments)
                    {
                        // Lấy branches cho department này
                        var departmentBranches = DaoFactory.Department.GetListBranchById(d.Id, companyId);

                        var departmentResponse = new CreateDepartmentResponse
                        {
                            Id = d.Id,
                            Name = d.DepartmentName,
                            CreatedAt = d.CreatedAt.HasValue ? d.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                            BranchIds = departmentBranches?.Select(b => b.BranchId).ToList() ?? new List<int>(),
                            Branchs = departmentBranches?.Select(b => new DepartmentsBranchsResponseList
                            {
                                Id = b.BranchId,
                                Name = b.BranchName ?? "",
                                Color = b.Color ?? ""
                            }).ToList() ?? new List<DepartmentsBranchsResponseList>(),
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
                        };

                        departmentResponses.Add(departmentResponse);
                    }

                    response.Data = departmentResponses;

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy danh sách phòng ban thành công";
                    return response;
                }

                response.Code = ResponseResultEnum.NoData.Value();
                response.Message = "Không có dữ liệu";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyGetAllDepartment companyId {0} EX: {1}", companyId, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lấy danh sách phòng ban thất bại";
            }

            return response;
        }

        public ApiResult<DepartmentListDetailResponse> CompanyDepartmentGetList(int companyId, int? page = null, bool? isAll = null, List<int> branchIds = null)
        {
            var response = new ApiResult<DepartmentListDetailResponse>()
            {
                Data = new DepartmentListDetailResponse
                {
                    Items = new List<CreateDepartmentResponse>(),
                    Meta = null
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                List<Ins_CompanyDepartment_GetListByCompanyId_Result> departments;
                int totalCount = 0;
                int perPage = 15;

                // Nếu có page parameter, lấy departments với pagination
                if (page.HasValue && page.Value > 0)
                {
                    departments = DaoFactory.Department.GetListByCompanyId(companyId, page.Value, perPage, false);
                    totalCount = departments.FirstOrDefault()?.TotalRecord ?? 0;
                }
                else
                {
                    // Logic mới: xử lý isAll và filter
                    if (isAll == true || (branchIds != null && branchIds.Count > 0))
                    {
                        // Lấy tất cả departments trong company (không dùng pagination khi page = null)
                        departments = DaoFactory.Department.GetListByCompanyId(companyId, 1, perPage, true);
                        totalCount = departments.Count; // Đếm thực tế thay vì dùng TotalRecord
                    }
                    else
                    {
                        response.Code = ResponseResultEnum.InvalidInput.Value();
                        response.Message = "Vui lòng cung cấp branch_id hoặc sử dụng is_all = true.";
                        return response;
                    }
                }

                if (departments != null && departments.Any())
                {
                    var currentPage = page ?? 1;
                    var totalPages = (int)Math.Ceiling((double)totalCount / perPage);

                    // Lấy danh sách branch cho từng department
                    var departmentResponses = new List<CreateDepartmentResponse>();

                    foreach (var dept in departments)
                    {
                        // Lấy branches cho department này
                        var departmentBranches = DaoFactory.Department.GetListBranchById(dept.Id, companyId);

                        // Logic filter mới:
                        // - Nếu isAll = true: thêm tất cả departments
                        // - Nếu có branchId filter: thêm departments có mapping với branchId HOẶC departments chung (không có mapping)
                        bool shouldAddDepartment = true;

                        if (branchIds != null && branchIds.Count > 0)
                        {
                            // Kiểm tra xem department có mapping với branchId không
                            bool hasBranchMapping = departmentBranches?.Any(b => branchIds.Contains(b.BranchId)) == true;

                            // Kiểm tra xem department có phải là department chung không (không có mapping nào)
                            bool isCommonDepartment = departmentBranches == null || !departmentBranches.Any();

                            // Thêm department nếu có mapping với branchId HOẶC là department chung
                            shouldAddDepartment = hasBranchMapping || isCommonDepartment;
                        }

                        if (shouldAddDepartment)
                        {
                            var departmentResponse = new CreateDepartmentResponse
                            {
                                Id = dept.Id,
                                Name = dept.DepartmentName,
                                CreatedAt = dept.CreatedAt.HasValue ? dept.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                                BranchIds = departmentBranches?.Select(b => b.BranchId).ToList() ?? new List<int>(),
                                Branchs = departmentBranches?.Select(b => new DepartmentsBranchsResponseList
                                {
                                    Id = b.BranchId,
                                    Name = b.BranchName ?? "",
                                    Color = b.Color ?? ""
                                }).ToList() ?? new List<DepartmentsBranchsResponseList>(),
                                Description = dept.Description,
                                ParentId = dept.ParentId,
                                SortIndex = dept.SortIndex ?? 0,
                                Code = dept.Code,
                                Alias = dept.Alias,
                                Key = dept.Id.ToString(),
                                Value = dept.DepartmentName,
                                Title = dept.DepartmentName,
                                Parent = dept.ParentId,
                                IsHead = dept.IsHead ?? false,
                            };

                            departmentResponses.Add(departmentResponse);
                        }
                    }

                    response.Data.Items = departmentResponses;

                    // Chỉ tạo meta khi có phân trang
                    if (page.HasValue && page.Value > 0)
                    {
                        response.Data.Meta = new BranchesMetaReponse
                        {
                            Total = totalCount,
                            Count = departments.Count,
                            PerPage = perPage,
                            CurrentPage = currentPage,
                            TotalPages = totalPages.ToString()
                        };
                    }

                    response.Code = ResponseResultEnum.Success.Value();

                    // Tạo message phù hợp với filter
                    if (page.HasValue && page.Value > 0)
                    {
                        response.Message = "Lấy danh sách phòng ban với phân trang thành công";
                    }
                    else if (isAll == true && !page.HasValue)
                    {
                        response.Message = "Lấy danh sách tất cả phòng ban thành công";
                    }
                    else if (branchIds != null)
                    {
                        response.Message = "Lấy danh sách phòng ban theo chi nhánh thành công";
                    }
                    else
                    {
                        response.Message = "Lấy danh sách phòng ban thành công";
                    }

                    return response;
                }
                else
                {
                    // Không có dữ liệu
                    if (page.HasValue && page.Value > 0)
                    {
                        response.Data.Meta = new BranchesMetaReponse
                        {
                            Total = 0,
                            Count = 0,
                            PerPage = perPage,
                            CurrentPage = page.Value,
                            TotalPages = "0"
                        };
                    }

                    response.Data.Items = new List<CreateDepartmentResponse>();
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không có dữ liệu phòng ban cho công ty này.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("DepartmentBo CompanyDepartmentGetList Exception EX: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
            }

            return response;
        }

        public ApiResult<UpdateDepartmentResponse> UpdateDepartment(int companyId, UpdateDepartmentRequest request)
        {
            var response = new ApiResult<UpdateDepartmentResponse>()
            {
                Data = new UpdateDepartmentResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input
                if (request == null || request.Id <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID phòng ban không hợp lệ.";
                    return response;
                }

                if (string.IsNullOrEmpty(request.Name))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Tên phòng ban không được để trống.";
                    return response;
                }

                // Auto-generate alias and code if not provided (giống như create)
                var alias = request.Alias ?? StringCommon.NormalizeText(request.Name, "-");
                var code = request.Code ?? StringCommon.NormalizeText(request.Name, "_").ToUpper();

                // Call DAO to update department
                var result = DaoFactory.Department.UpdateDepartment(
                    request.Id,
                    request.Name,
                    request.IsOnboarding,
                    alias,
                    code,
                    companyId
                );

                if (result > 0)
                {
                    // Handle branch relationships if provided
                    var validBranchIds = new List<int>();

                    if (request.BranchIds?.Any() == true)
                    {
                        // Get current branch relationships and all company branches once
                        var currentBranches = DaoFactory.Department.GetListBranchById(request.Id, companyId);
                        var currentBranchIds = new HashSet<int>(currentBranches?.Select(b => b.BranchId) ?? new List<int>());

                        var allBranches = DaoFactory.Branches.GetAllBranchs(companyId, out int total);
                        var companyBranchIds = new HashSet<int>(allBranches.Select(b => b.BranchId));

                        // Validate new branch IDs belong to company
                        validBranchIds = request.BranchIds.Where(companyBranchIds.Contains).ToList();
                        var validBranchIdsSet = new HashSet<int>(validBranchIds);

                        // Remove relationships that are not in new list
                        var branchesToRemove = currentBranchIds.Except(validBranchIdsSet);
                        foreach (var branchId in branchesToRemove)
                        {
                            DaoFactory.Department.DeleteRelate(request.Id, branchId);
                        }

                        // Add new relationships
                        var branchesToAdd = validBranchIdsSet.Except(currentBranchIds);
                        foreach (var branchId in branchesToAdd)
                        {
                            DaoFactory.Department.CreateRelate(request.Id, branchId);
                        }
                    }

                    // Set response data
                    response.Data.BranchIds = validBranchIds;
                    response.Data.Id = result;
                    response.Data.Name = request.Name;
                    response.Data.Alias = alias;
                    response.Data.Code = code;
                    response.Data.IsOnboarding = request.IsOnboarding ?? 0;
                    response.Data.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Cập nhật phòng ban thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Cập nhật phòng ban thất bại.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("UpdateDepartment Error: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình cập nhật phòng ban.";
            }

            return response;
        }

        public ApiResult<int> DeleteDepartment(int companyId, DeleteDepartmentRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input
                if (request == null || request.Id <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID phòng ban không hợp lệ.";
                    return response;
                }

                // Call DAO to delete department
                var result = DaoFactory.Department.DeleteDepartment(request.Id, companyId);

                if (result > 0)
                {
                    response.Data = result;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Xóa phòng ban thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Xóa phòng ban thất bại.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("DeleteDepartment Error: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xóa phòng ban.";
            }

            return response;
        }
    }
}
