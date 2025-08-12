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
    public class PositionBo : BaseBo<DBNull>
    {
        public PositionBo()
            : base(DaoFactory.Position)
        {

        }
        public ApiResult<int> CreatePositionAsync(CreatePositionRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            if (string.IsNullOrEmpty(request.Name))
            {
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "Vui lòng nhập tên vị trí.";
                return response;
            }

            if (request.BrandId <= 0)
            {
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "ID phòng ban không hợp lệ.";
                return response;
            }

            if (request.CompanyId <= 0)
            {
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "ID công ty không hợp lệ.";
                return response;
            }

            try
            {
                var positionId = DaoFactory.Position.CreatePosition(request.Name, request.BrandId, request.CompanyId);

                if (positionId > 0)
                {
                    response.Data = positionId;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Tạo vị trí thành công";
                    return response;
                }

                response.Data = positionId;
                response.Code = ResponseResultEnum.Failed.Value();
                response.Message = "Tạo vị trí thất bại";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CreatePositionAsync Exception Name {0}, DepartmentId {1}, CompanyId {2} EX:",
                    request.Name,
                    request.BrandId,
                    request.CompanyId,
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Tạo vị trí thất bại";
            }

            return response;
        }

        public ApiResult<List<CreatePosisionResponse>> SetupCompany_CreatePositionInAllBranchesAsync(int companyId, CreatePosisionInAllBranchesRequest request)
        {
            var response = new ApiResult<List<CreatePosisionResponse>>()
            {
                Data = new List<CreatePosisionResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };
            try
            {
                int positionID = 0;
                if (request.IsOnboarding == 1)
                {
                    var total = 0;
                    var dataBrandID = DaoFactory.Branches.GetAllBranchs(companyId, out total).OrderBy(x => x.BranchId).ToList();

                    foreach (var item in request.Positions)
                    {
                        positionID = DaoFactory.Position.CreatePosition_Simple(
                            item.Names,
                            StringCommon.NormalizeText(item.Names, "-"),
                            StringCommon.NormalizeText(item.Names, "_").ToUpper(),
                            companyId,
                            item.ExpYear
                        );

                        if (positionID <= 0)
                        {
                            continue;
                        }

                        foreach (var item_dataBrandID in dataBrandID)
                        {
                            DaoFactory.Position.CreatePosition_CreateRelate(
                                item_dataBrandID.BranchId,
                                positionID,
                                0,
                                0
                            );
                        }

                        response.Data.Add(
                            new CreatePosisionResponse()
                            {
                                Id = positionID,
                                Name = item.Names,
                                Code = StringCommon.NormalizeText(item.Names, "_").ToUpper(),
                                SortIndex = 0,
                                AcademicLevel = new List<int>(),
                                ExpYear = item.ExpYear,
                                Description = null,
                                Alias = StringCommon.NormalizeText(item.Names, "-"),
                                BranchIds = dataBrandID.Select(x => x.BranchId).Distinct().ToList(),
                                Branchs = dataBrandID.Select(x => new PositionsBranchsResponseList
                                {
                                    Id = x.BranchId,
                                    Name = x.BranchName ?? string.Empty,
                                    Color = x.Color ?? string.Empty,
                                }).Distinct().ToList()
                            }
                        );
                    }
                } else {
                    foreach (var item in request.Positions)
                    {
                        positionID = DaoFactory.Position.CreatePosition_Simple(
                            item.Names,
                            StringCommon.NormalizeText(item.Names, "-"),
                            StringCommon.NormalizeText(item.Names, "_").ToUpper(),
                            companyId,
                            item.ExpYear
                        );

                        if(positionID <= 0) {
                            continue;
                        }

                        if(request.BranchIds != null && request.BranchIds.Count > 0) {
                            foreach (var item_branchId in request.BranchIds)
                            {
                                DaoFactory.Position.CreatePositionBranchRelate(positionID, item_branchId, companyId);
                            }
                        }
                        
                        if(request.DepartmentIds != null && request.DepartmentIds.Count > 0) {
                            foreach (var item_departmentId in request.DepartmentIds)
                            {
                                DaoFactory.Position.CreatePositionDepartmentRelate(positionID, item_departmentId, companyId);
                            }
                        }

                        response.Data.Add(
                            new CreatePosisionResponse()
                            {
                                Id = positionID,
                                Name = item.Names,
                                Code = StringCommon.NormalizeText(item.Names, "_").ToUpper(),
                                SortIndex = 0,
                                AcademicLevel = new List<int>(),
                                ExpYear = item.ExpYear,
                                Description = null,
                                Alias = StringCommon.NormalizeText(item.Names, "-"),
                                BranchIds = request.BranchIds,
                                DepartmentIds = request.DepartmentIds
                            }
                        );
                    }
                }
                if (response.Data != null && response.Data.Any())
                {
                    var result = DaoFactory.Company.UpdateCompanyStep(companyId, SetupStepEnum.ONBOARDING_CREATE_POSITION.Value());
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
                CommonLogger.DefaultLogger.ErrorFormat("CreatePositionInAllBranchesAsync Exception EX: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Tạo phòng ban thất bại";
            }

            return response;
        }

        public ApiResult<PositionListDetailResponse> CompanyPositionGetList(int companyId, int? page = null, bool? isAll = null, List<int> branchIds = null, List<int> departmentIds = null)
        {
            var response = new ApiResult<PositionListDetailResponse>()
            {
                Data = new PositionListDetailResponse
                {
                    Items = new List<PositionByBranchResponse>(),
                    Meta = null
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                List<Ins_CompanyPosition_GetListByCompanyId_Result> positions;
                int totalCount = 0;
                int perPage = 15;

                // Nếu có page parameter, lấy positions với pagination
                if (page.HasValue && page.Value > 0)
                {
                    positions = DaoFactory.Position.GetListByCompanyId(companyId, page.Value, perPage, false);
                    totalCount = positions.FirstOrDefault()?.TotalRecord ?? 0;
                }
                else
                {
                    // Logic mới: xử lý isAll và filter
                    if (isAll == true || (branchIds != null && branchIds.Count > 0) || (departmentIds != null && departmentIds.Count > 0))
                    {
                        // Lấy tất cả positions trong company (không dùng pagination khi page = null)
                        positions = DaoFactory.Position.GetListByCompanyId(companyId, 1, perPage, true);
                        totalCount = positions.Count; // Đếm thực tế thay vì dùng TotalRecord

                        // Không cần filter ở code C# vì stored procedure đã không trả về BranchId và DepartmentID
                    }
                    else
                    {
                        response.Code = ResponseResultEnum.InvalidInput.Value();
                        response.Message = "Vui lòng cung cấp branch_id hoặc department_id hợp lệ hoặc sử dụng is_all = true.";
                        return response;
                    }
                }

                if (positions != null && positions.Any())
                {
                    var currentPage = page ?? 1;
                    var totalPages = (int)Math.Ceiling((double)totalCount / perPage);

                    // Lấy danh sách branches và departments cho từng position
                    var positionResponses = new List<PositionByBranchResponse>();

                    foreach (var pos in positions)
                    {
                        // Lấy branches cho position này
                        var positionBranches = DaoFactory.Position.GetBranchesById(pos.Id, companyId);

                        // Lấy departments cho position này
                        var positionDepartments = DaoFactory.Position.GetDepartmentsById(pos.Id, companyId);

                        // Filter branches và departments theo filter parameters
                        if (branchIds != null && branchIds.Count > 0)
                        {
                            positionBranches = positionBranches?.Where(b => branchIds.Contains(b.BranchId)).ToList() ?? new List<Ins_CompanyPosition_GetBranchesById_Result>();
                        }

                        if (departmentIds != null && departmentIds.Count > 0)
                        {
                            positionDepartments = positionDepartments?.Where(d => departmentIds.Contains(d.Id)).ToList() ?? new List<Ins_CompanyPosition_GetDepartmentsById_Result>();
                        }

                        // Logic filter mới:
                        // - Nếu isAll = true: thêm tất cả positions
                        // - Nếu có branchId filter: thêm positions có mapping với branchId HOẶC positions chung (không có mapping)
                        // - Nếu có departmentId filter: thêm positions có mapping với departmentId HOẶC positions chung (không có mapping)
                        bool shouldAddPosition = true;

                        if (branchIds != null && branchIds.Count > 0)
                        {
                            // Kiểm tra xem position có mapping với branchId không
                            bool hasBranchMapping = positionBranches?.Any(b => branchIds.Contains(b.BranchId)) == true;

                            // Kiểm tra xem position có phải là position chung không (không có mapping nào)
                            bool isCommonPosition = positionBranches == null || !positionBranches.Any();

                            // Thêm position nếu có mapping với branchId HOẶC là position chung
                            shouldAddPosition = shouldAddPosition && (hasBranchMapping || isCommonPosition);
                        }

                        if (departmentIds != null && departmentIds.Count > 0)
                        {
                            // Kiểm tra xem position có mapping với departmentId không
                            bool hasDepartmentMapping = positionDepartments?.Any(d => departmentIds.Contains(d.Id)) == true;

                            // Kiểm tra xem position có phải là position chung không (không có mapping nào)
                            bool isCommonPosition = positionDepartments == null || !positionDepartments.Any();

                            // Thêm position nếu có mapping với departmentId HOẶC là position chung
                            shouldAddPosition = shouldAddPosition && (hasDepartmentMapping || isCommonPosition);
                        }

                        if (shouldAddPosition)
                        {
                            var positionResponse = new PositionByBranchResponse
                            {
                                id = pos.Id,
                                name = pos.PositionName,
                                code = pos.Code,
                                sort_index = pos.SortIndex,
                                exp_year = pos.ExpYear,
                                academic_level = new List<string>(),
                                description = pos.Description,
                                alias = pos.Alias,
                                branch_ids = positionBranches?.Select(b => b.BranchId).ToList() ?? new List<int>(),
                                branchs = positionBranches?.Select(b => new PositionsBranchsResponseList
                                {
                                    Id = b.BranchId,
                                    Name = b.BranchName ?? "",
                                    Color = b.Color ?? ""
                                }).ToList() ?? new List<PositionsBranchsResponseList>(),
                                department_ids = positionDepartments?.Select(d => d.Id).ToList() ?? new List<int>(),
                                departments = positionDepartments?.Select(d => new PositionsDepartmentsResponseList
                                {
                                    Id = d.Id,
                                    Name = d.DepartmentName ?? "",
                                    Color = d.Color ?? ""
                                }).ToList() ?? new List<PositionsDepartmentsResponseList>()
                            };

                            positionResponses.Add(positionResponse);
                        }
                    }

                    response.Data.Items = positionResponses;

                    // Chỉ tạo meta khi có phân trang
                    if (page.HasValue && page.Value > 0)
                    {
                        response.Data.Meta = new BranchesMetaReponse
                        {
                            Total = totalCount,
                            Count = positions.Count,
                            PerPage = perPage,
                            CurrentPage = currentPage,
                            TotalPages = totalPages.ToString()
                        };
                    }

                    response.Code = ResponseResultEnum.Success.Value();

                    // Tạo message phù hợp với filter
                    if (page.HasValue && page.Value > 0)
                    {
                        response.Message = "Lấy danh sách vị trí với phân trang thành công";
                    }
                    else if (isAll == true && !page.HasValue)
                    {
                        response.Message = "Lấy danh sách tất cả vị trí thành công";
                    }
                    else if (branchIds != null && departmentIds != null)
                    {
                        response.Message = "Lấy danh sách vị trí theo chi nhánh và phòng ban thành công";
                    }
                    else if (branchIds != null)
                    {
                        response.Message = "Lấy danh sách vị trí theo chi nhánh thành công";
                    }
                    else if (departmentIds != null)
                    {
                        response.Message = "Lấy danh sách vị trí theo phòng ban thành công";
                    }
                    else
                    {
                        response.Message = "Lấy danh sách vị trí thành công";
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

                    response.Data.Items = new List<PositionByBranchResponse>();
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không có dữ liệu vị trí cho công ty này.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("PositionBo GetPositionsByBranchWithFilter Exception EX: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
            }

            return response;
        }

        // =============================================
        // UPDATE POSITION METHODS
        // =============================================

        public ApiResult<UpdatePositionResponse> UpdatePositionAsync(UpdatePositionRequest request, int companyId)
        {
            var response = new ApiResult<UpdatePositionResponse>()
            {
                Data = new UpdatePositionResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            // Validate input
            if (request.PositionId <= 0)
            {
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "ID vị trí không hợp lệ.";
                return response;
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "Vui lòng nhập tên vị trí.";
                return response;
            }

            try
            {
                // Prepare update data
                var name = request.Name;
                var alias = !string.IsNullOrEmpty(request.Alias) ? request.Alias : StringCommon.NormalizeText(request.Name, "-");
                var code = !string.IsNullOrEmpty(request.Code) ? request.Code : StringCommon.NormalizeText(request.Name, "_").ToUpper();
                var expYear = request.ExpYear ?? 0;
                var description = request.Description;
                var sortIndex = request.SortIndex ?? 0;

                // Update position
                var updateResult = DaoFactory.Position.UpdatePosition(
                    request.PositionId,
                    name,
                    alias,
                    code,
                    companyId,
                    expYear,
                    description,
                    sortIndex
                );

                if (updateResult > 0)
                {
                    // Handle branch relationships if provided
                    var validBranchIds = new List<int>();
                    if (request.BranchIds?.Any() == true)
                    {
                        // Get current branch relationships
                        var currentBranches = DaoFactory.Position.GetBranchesById(request.PositionId, companyId);
                        var currentBranchIds = new HashSet<int>(currentBranches?.Select(b => b.BranchId) ?? new List<int>());

                        // Get all company branches for validation
                        var allBranches = DaoFactory.Branches.GetAllBranchs(companyId, out int total);
                        var companyBranchIds = new HashSet<int>(allBranches.Select(b => b.BranchId));

                        // Validate new branch IDs belong to company
                        validBranchIds = request.BranchIds.Where(companyBranchIds.Contains).ToList();
                        var validBranchIdsSet = new HashSet<int>(validBranchIds);

                        // Remove relationships that are not in new list
                        var branchesToRemove = currentBranchIds.Except(validBranchIdsSet);
                        foreach (var branchId in branchesToRemove)
                        {
                            DaoFactory.Position.DeletePositionBranchRelate(request.PositionId, branchId, companyId);
                        }

                        // Add new relationships
                        var branchesToAdd = validBranchIdsSet.Except(currentBranchIds);
                        foreach (var branchId in branchesToAdd)
                        {
                            DaoFactory.Position.CreatePositionBranchRelate(request.PositionId, branchId, companyId);
                        }
                    }

                    // Handle department relationships if provided
                    var validDepartmentIds = new List<int>();
                    if (request.DepartmentIds?.Any() == true)
                    {
                        // Get current department relationships
                        var currentDepartments = DaoFactory.Position.GetDepartmentsById(request.PositionId, companyId);
                        var currentDepartmentIds = new HashSet<int>(currentDepartments?.Select(d => d.Id) ?? new List<int>());

                        // Get all company departments for validation
                        var allDepartments = DaoFactory.Department.GetAllDepartments(companyId);
                        var companyDepartmentIds = new HashSet<int>(allDepartments.Select(d => d.Id));

                        // Validate new department IDs belong to company
                        validDepartmentIds = request.DepartmentIds.Where(companyDepartmentIds.Contains).ToList();
                        var validDepartmentIdsSet = new HashSet<int>(validDepartmentIds);

                        // Remove relationships that are not in new list
                        var departmentsToRemove = currentDepartmentIds.Except(validDepartmentIdsSet);
                        foreach (var departmentId in departmentsToRemove)
                        {
                            DaoFactory.Position.DeletePositionDepartmentRelate(request.PositionId, departmentId, companyId);
                        }

                        // Add new relationships
                        var departmentsToAdd = validDepartmentIdsSet.Except(currentDepartmentIds);
                        foreach (var departmentId in departmentsToAdd)
                        {
                            DaoFactory.Position.CreatePositionDepartmentRelate(request.PositionId, departmentId, companyId);
                        }
                    }

                    // Get updated relationships for response
                    var updatedBranches = DaoFactory.Position.GetBranchesById(request.PositionId, companyId);
                    var updatedDepartments = DaoFactory.Position.GetDepartmentsById(request.PositionId, companyId);

                    response.Data = new UpdatePositionResponse
                    {
                        Id = request.PositionId,
                        Name = name,
                        Alias = alias,
                        Code = code,
                        ExpYear = expYear,
                        Description = description,
                        SortIndex = sortIndex,
                        BranchIds = validBranchIds,
                        Branchs = updatedBranches?.Select(b => new PositionsBranchsResponseList
                        {
                            Id = b.BranchId,
                            Name = b.BranchName ?? "",
                            Color = b.Color ?? ""
                        }).ToList() ?? new List<PositionsBranchsResponseList>(),
                        DepartmentIds = validDepartmentIds,
                        Departments = updatedDepartments?.Select(d => new PositionsDepartmentsResponseList
                        {
                            Id = d.Id,
                            Name = d.DepartmentName ?? "",
                            Color = d.Color ?? ""
                        }).ToList() ?? new List<PositionsDepartmentsResponseList>()
                    };

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Cập nhật vị trí thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Cập nhật vị trí thất bại";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("PositionBo UpdatePositionAsync Exception PositionId {0}, EX: {1}",
                    request.PositionId,
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Cập nhật vị trí thất bại";
            }

            return response;
        }

        // =============================================
        // DELETE POSITION METHOD
        // =============================================

        public ApiResult<int> DeletePosition(int positionId, int companyId)
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
                if (positionId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID vị trí không hợp lệ.";
                    return response;
                }

                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID công ty không hợp lệ.";
                    return response;
                }

                // Delete position
                var deleteResult = DaoFactory.Position.DeletePosition(positionId, companyId);

                if (deleteResult > 0)
                {
                    response.Data = deleteResult;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Xóa vị trí thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Xóa vị trí thất bại";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("PositionBo DeletePosition Exception PositionId {0}, CompanyId {1}, EX: {2}",
                    positionId,
                    companyId,
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Xóa vị trí thất bại";
            }

            return response;
        }
    }
}
