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

namespace BussinessObject.Bo.TanTamBo
{
    public class BranchesBo : BaseBo<DBNull>
    {
        public BranchesBo()
            : base(DaoFactory.Company)
        {

        }

        public ApiResult<List<CreateBranchesResponse>> SetupCompany_CreateBranches(int companyId, int accountid, List<CreateBranchesRequest> request)
        {
            var response = new ApiResult<List<CreateBranchesResponse>>()
            {
                Data = new List<CreateBranchesResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var branchIds = 0;
                var branchId = 0;
                var now = DateTime.Now;
                var isAddFirstBranch = false;

                var isOnboarding = request.Any(x => x.IsOnboarding.GetValueOrDefault(0) == 1);
                var regionid = 0;

                if (isOnboarding)
                {
                    regionid = DaoFactory.Branches.GetAllRegion(companyId).FirstOrDefault().ID;
                }

                foreach (var branch in request)
                {
                    if (isOnboarding == true && (branch.RegionId == null || branch.RegionId.GetValueOrDefault() <= 0)) //  ko truyền thì xài mặc định chỉ áp dụng cho isOnboarding
                    {
                        branch.RegionId = regionid;
                    }
                    else if(isOnboarding == false && (branch.RegionId == null || branch.RegionId.GetValueOrDefault() <= 0))
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(branch.Name))
                    {
                        branch.Name = "Chi nhánh " + (branchIds + 1);
                    }

                    if (string.IsNullOrEmpty(branch.Address))
                    {
                        branch.Address = "Địa chỉ chi nhánh " + (branchIds + 1);
                    }

                    branchId = DaoFactory.Branches.CreateBranche(
                        branch.Name,
                        branch.Address,
                        branch.RegionId ?? 0,
                        isOnboarding == true ? 1 :0,
                        branch.Latitude ?? 0,
                        branch.Longitude ?? 0,
                        companyId,
                        StringCommon.NormalizeText(branch.Name, "-"),
                        StringCommon.NormalizeText(branch.Name, "_").ToUpper());

                    try
                    {
                        // tạo thêm thông tin wifi cho chi nhánh 
                        var resultWifi = DaoFactory.Wifi.CreateWifi(200, 0, 10, 0, branch.Latitude ?? 0, branch.Longitude ?? 0, branch.Name, branchId, branch.Address, Wifi_type_Enum.wifi.Value());
                    }
                    catch (Exception ex)
                    {
                        CommonLogger.DefaultLogger.ErrorFormat("SetupCompany_CreateBranches CreateWifi {0} ,companyId {1} , {2} EX: {3}", accountid, companyId , branchId , ex.ToString());
                    }
                    response.Data.Add(new CreateBranchesResponse
                    {
                        Id = branchId,
                        Name = branch.Name,
                        AddressLat = branch.Latitude ?? 0,
                        AddressLng = branch.Longitude ?? 0,
                        Country = "",
                        Province = "",
                        District = "",
                        Address = branch.Address,
                        CreatedAt = now.ToString("yyyy-MM-dd HH:mm:ss"),
                        Alias = StringCommon.NormalizeText(branch.Name, "-"),
                        Code = StringCommon.NormalizeText(branch.Name, "_").ToUpper(),
                        SortIndex = 0,
                        PhoneCode = "",
                        Region = new RegionInfo
                        {
                            Id = companyId
                        }
                    });

                    if (isOnboarding && !isAddFirstBranch)
                    {
                        DaoFactory.Company.Employee_AddIntoBranch(accountid, branchId, true);
                        isAddFirstBranch = true;
                    }

                    branchIds++;
                }

                if (branchIds == request.Count())
                {
                    var result = DaoFactory.Company.UpdateCompanyStep(companyId, SetupStepEnum.ONBOARDING_CREATE_BRANCH.Value());

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Tạo chi nhánh thành công";
                    return response;
                }

                response.Code = ResponseResultEnum.Failed.Value();
                response.Message = "Tạo chi nhánh thất bại";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("SetupCompany_CreateBranches accountId {0} ,companyId {1} ,  EX: {3}", accountid, companyId, ex.ToString());
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Tạo chi nhánh thất bại";
            }

            return response;
        }

        public ApiResult<ListBranchesReponse> CompanyGetAllBranches(int companyId, int currentPage = 10000)
        {
            var response = new ApiResult<ListBranchesReponse>()
            {
                Data = new ListBranchesReponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var total = 0;
                var dataSQL = DaoFactory.Branches.GetAllBranchs(companyId, out total);
                total = total <= 0 ? dataSQL.Count() : total;
                if (dataSQL == null || total == 0)
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    return response;
                }

                response.Data.Meta = new BranchesMetaReponse()
                {
                    Count = dataSQL.Count,
                    CurrentPage = currentPage,
                    PerPage = 30,
                    Total = total
                };

                var companys = new RegionInfo()
                {
                    Id = companyId,
                    Name = dataSQL.FirstOrDefault().FullName
                };

                response.Data.Items = dataSQL.Select(d => new CreateBranchesResponse
                {
                    Id = d.BranchId,
                    Name = d.BranchName,
                    CreatedAt = d.CreatedAt.HasValue ? d.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                    Region = companys,
                    AddressLat = d.Latitude ?? 0,
                    AddressLng = d.Longitude ?? 0,
                    Country = d.Country,
                    Province = d.Province,
                    District = d.District,
                    Tel = d.Tel,
                    Address = d.Address,
                    Description = d.Description,
                    IsHeadquarter = d.IsHeadquarter,
                    Alias = d.Alias,
                    Code = d.Code,
                    SortIndex = d.SortIndex ?? 0,
                    PhoneCode = d.PhoneCode ?? ""
                }).ToList();

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy danh sách chi nhánh thành công";
                return response;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyGetAllBranches companyId {0} EX: {1}", companyId, ex.ToString());
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lấy danh sách phòng ban thất bại";
            }

            return response;
        }

        public ApiResult<List<RegionInfo>> CompanyGetAllRegion(int companyId)
        {
            var response = new ApiResult<List<RegionInfo>>()
            {
                Data = new List<RegionInfo>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var total = 0;
                var dataSQL = DaoFactory.Branches.GetAllRegion(companyId);                
                if (dataSQL == null || dataSQL.Count == 0)
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    return response;
                }
                response.Data = dataSQL.Select(d => new RegionInfo
                {
                    Id = d.ID,
                    Name = d.Region, 
                    Code = d.Code,
                    Alias = d.Alias,
                    CreatedAt = d.CreateAt.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                    Description = d.Description,
                    SortIndex = d.SortIndex ?? 0
                }).ToList();

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy danh sách vùng thành công";
                return response;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyGetAllBranches companyId {0} EX: {1}", companyId, ex.ToString());
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lấy danh sách vùng thất bại";
            }

            return response;
        }

        public ApiResult<int> CompanyRegionCreate(string regionName,string description, int companyId)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                try
                {
                    var id = DaoFactory.Branches.CreateCompanyRegion(regionName, companyId, "", StringCommon.NormalizeText(regionName, "_").ToUpper(), 0, description, StringCommon.NormalizeText(regionName, "-").ToLower());
                    response.Data = id;
                    if (id <= 0)
                    {
                        response.Code = ResponseResultEnum.Failed.Value();
                        response.Message = "Tạo vùng thất bại";
                    }
                    else
                    {
                        response.Code = ResponseResultEnum.Success.Value();
                        response.Message = "Tạo vùng thành công";
                    }
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat("SetupCompany_CreateBranches regionName {0} ,companyId {1} ,  EX: {3}", regionName, companyId, ex.ToString());
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Tạo vùng thất bại";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyGetAllBranches companyId {0} EX: {1}", companyId, ex.ToString());
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Tạo vùng thất bại";
            }

            return response;
        }

        public ApiResult<int> CompanyRegionUpdate(string regionName, string description, int companyId, int idRegion)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                try
                {
                    DaoFactory.Branches.UpdateCompanyRegion(regionName, companyId, "", StringCommon.NormalizeText(regionName, "_").ToUpper(), 0, description, StringCommon.NormalizeText(regionName, "-").ToLower(), idRegion);

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.Success.Text();
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat("SetupCompany_CreateBranches regionName {0} ,companyId {1} ,  EX: {3}", regionName, companyId, ex.ToString());
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Cập nhật vùng thất bại";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyGetAllBranches companyId {0} EX: {1}", companyId, ex.ToString());
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Cập nhật vùng thất bại";
            }

            return response;
        }

        public ApiResult<int> CompanyRegionDelete(int regionId, int companyId)
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
                if (regionId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID vùng không hợp lệ.";
                    return response;
                }

                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID công ty không hợp lệ.";
                    return response;
                }

                // Call stored procedure - validation is done in stored procedure
                var result = DaoFactory.Branches.DeleteCompanyRegion(regionId, companyId);

                if (result.HasValue && result.Value > 0)
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Xóa vùng thành công.";
                    response.Data = result.Value;
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Xóa vùng thất bại.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("BranchesBo CompanyRegionDelete regionId {0}, companyId {1} EX: {2}", regionId, companyId, ex.ToString());
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xóa vùng.";
            }

            return response;
        }

        public ApiResult<int> CompanyBranchUpdate(UpdateBranchRequest request, int companyId)
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
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Dữ liệu cập nhật không được để trống.";
                    return response;
                }

                if (request.Id <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID chi nhánh không hợp lệ.";
                    return response;
                }

                if (string.IsNullOrEmpty(request.Name))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Tên chi nhánh không được để trống.";
                    return response;
                }

                // Generate alias and code from name (giống branch-add)
                var alias = StringCommon.NormalizeText(request.Name, "-");
                var code = StringCommon.NormalizeText(request.Name, "_").ToUpper();

                // Set default values if not provided
                var regionId = request.RegionId ?? 0;
                var isOnboarding = request.IsOnboarding ?? 0;
                var latitude = request.Latitude ?? 0;
                var longitude = request.Longitude ?? 0;
                var address = request.Address ?? "";

                // Call DAO to update branch with generated alias and code
                var result = DaoFactory.Branches.UpdateBranch(
                    request.Id,
                    request.Name,
                    address,
                    regionId,
                    isOnboarding,
                    latitude,
                    longitude,
                    companyId,
                    alias,
                    code
                );

                if (result > 0)
                {
                    response.Data = result;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Cập nhật chi nhánh thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể cập nhật chi nhánh. Vui lòng thử lại.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("BranchesBo CompanyBranchUpdate Error", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình cập nhật chi nhánh.";
            }

            return response;
        }

        public ApiResult<BranchListDetailResponse> CompanyBranchGetList(int companyId, int? page = null, bool? isAll = null, int? regionId = null)
        {
            var response = new ApiResult<BranchListDetailResponse>()
            {
                Data = new BranchListDetailResponse() {
                    Meta = null,
                    Items = new List<BranchDetailResponse>()
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            List<Ins_CompanyBranch_GetListByCompanyId_Result> branches;
            int totalCount = 0;
            int perPage = 15;

            // Nếu có page parameter, lấy tất cả branches trong company (không filter theo region)
            if (page.HasValue && page.Value > 0)
            {
                branches = DaoFactory.Branches.GetListByCompanyId(companyId, page.Value, perPage, false);
                totalCount = branches.FirstOrDefault().TotalRecord.GetValueOrDefault();
            }
            else
            {
                // Logic mới: xử lý isAll và regionId
                if (isAll == true || (regionId.HasValue && regionId.Value > 0))
                {
                    // Nếu isAll = true hoặc có regionId, lấy tất cả branches trong company
                    branches = DaoFactory.Branches.GetListByCompanyId(companyId, 1, perPage, true);
                    
                    // Nếu có regionId, lọc theo regionId trong code C#
                    if (regionId.HasValue && regionId.Value > 0)
                    {
                        branches = branches.Where(b => b.RegionId == regionId.Value).ToList();
                        totalCount = branches.Count;
                    }
                }
                else
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp region_id hợp lệ hoặc sử dụng is_all = true.";
                    return response;
                }
            }

            if (branches.Any())
            {
                var currentPage = page ?? 1;
                var totalPages = (int)Math.Ceiling((double)totalCount / perPage);

                // Nếu có page parameter, thực hiện pagination
                if (page.HasValue && page.Value > 0)
                {
                    response.Data.Meta = new BranchesMetaReponse
                    {
                        Total = totalCount,
                        Count = branches.Count,
                        PerPage = perPage,
                        CurrentPage = currentPage,
                        TotalPages = totalPages.ToString()
                    };
                }

                response.Data.Items = branches.Select(b => new BranchDetailResponse()
                {
                    Id = b.BranchId,
                    Name = b.BranchName,
                    Region = new RegionInfo
                    {
                        Id = b.RegionId ?? 0,
                        Name = b.RegionName ?? ""
                    },
                    AddressLat = b.Latitude,
                    AddressLng = b.Longitude,
                    Country = b.Country,
                    Province = b.Province ?? "",
                    District = b.District ?? "",
                    Tel = b.Tel ?? "",
                    Address = b.Address,
                    Description = b.Description,
                    IsHeadquarter = b.IsHeadquarter,
                    CreatedAt = b.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                    Alias = b.Alias ?? "",
                    Color = b.Color,
                    Code = b.Code ?? "",
                    SortIndex = b.SortIndex ?? 0,
                    PhoneCode = b.PhoneCode ?? "84"
                }).ToList();
            }
            else
            {
                // Không có data
                response.Data.Meta = page.HasValue && page.Value > 0 ? new BranchesMetaReponse
                {
                    Total = 0,
                    Count = 0,
                    PerPage = 30,
                    CurrentPage = page.Value,
                    TotalPages = "0"
                } : null;
                response.Data.Items = new List<BranchDetailResponse>();
            }

            response.Code = ResponseResultEnum.Success.Value();
            response.Message = page.HasValue && page.Value > 0 
                ? "Lấy danh sách tất cả chi nhánh thành công" 
                : "Lấy danh sách chi nhánh theo vùng thành công";

            return response;
        }

        public ApiResult<RegionListDetailResponse> CompanyRegionGetList(int companyId, int? page = null, bool? isAll = null)
        {
            var response = new ApiResult<RegionListDetailResponse>()
            {
                Data = new RegionListDetailResponse
                {
                    Items = new List<RegionDetailResponse>(),
                    Meta = null
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                List<Ins_CompanyRegion_GetListByCompanyId_Result> regions;
                int totalCount = 0;
                int perPage = 15;

                // Nếu có page parameter, lấy regions với pagination
                if (page.HasValue && page.Value > 0)
                {
                    regions = DaoFactory.Branches.GetRegionListByCompanyId(companyId, page.Value, perPage, false);
                    totalCount = regions.FirstOrDefault()?.TotalRecord ?? 0;
                }
                else
                {
                    // Logic mới: xử lý isAll
                    if (isAll == true)
                    {
                        // Lấy tất cả regions trong company (không dùng pagination khi page = null)
                        regions = DaoFactory.Branches.GetRegionListByCompanyId(companyId, 1, perPage, true);
                        totalCount = regions.Count; // Đếm thực tế thay vì dùng TotalRecord
                    }
                    else
                    {
                        response.Code = ResponseResultEnum.InvalidInput.Value();
                        response.Message = "Vui lòng cung cấp page parameter hoặc sử dụng is_all = true.";
                        return response;
                    }
                }

                if (regions != null && regions.Any())
                {
                    var currentPage = page ?? 1;
                    var totalPages = (int)Math.Ceiling((double)totalCount / perPage);

                    response.Data.Items = regions.Select(x => new RegionDetailResponse
                    {
                        id = x.Id,
                        name = x.Name,
                        code = x.Code,
                        sort_index = x.SortIndex ?? 0,
                        description = x.Description,
                        created_at = x.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                        alias = x.Alias,
                        color = x.Color
                    }).ToList();

                    // Chỉ tạo meta khi có phân trang
                    if (page.HasValue && page.Value > 0)
                    {
                        response.Data.Meta = new BranchesMetaReponse
                        {
                            Total = totalCount,
                            Count = regions.Count,
                            PerPage = perPage,
                            CurrentPage = currentPage,
                            TotalPages = totalPages.ToString()
                        };
                    }

                    response.Code = ResponseResultEnum.Success.Value();
                    
                    // Tạo message phù hợp với filter
                    if (page.HasValue && page.Value > 0)
                    {
                        response.Message = "Lấy danh sách vùng với phân trang thành công";
                    }
                    else if (isAll == true && !page.HasValue)
                    {
                        response.Message = "Lấy danh sách tất cả vùng thành công";
                    }
                    else
                    {
                        response.Message = "Lấy danh sách vùng thành công";
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

                    response.Data.Items = new List<RegionDetailResponse>();
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không có dữ liệu vùng cho công ty này.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("BranchesBo CompanyRegionGetList Exception EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
            }

            return response;
        }

        /// <summary>
        /// Xóa chi nhánh với validation
        /// - Cho phép xóa ngay cả khi có records trong Department_Branch/Position_Branch
        /// - Tự động xóa các records liên quan trong Department_Branch/Position_Branch
        /// - Chỉ báo lỗi khi branch được sử dụng trong EmployeeBranchMap, CompanyDepartment, CompanyPosition
        /// </summary>
        public ApiResult<int> CompanyBranchDelete(int branchId, int companyId)
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
                if (branchId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID chi nhánh không hợp lệ.";
                    return response;
                }

                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID công ty không hợp lệ.";
                    return response;
                }

                // Call stored procedure - validation is done in stored procedure
                var result = DaoFactory.Branches.DeleteCompanyBranch(branchId, companyId);

                if (result.HasValue && result.Value > 0)
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Xóa chi nhánh thành công.";
                    response.Data = result.Value;
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Xóa chi nhánh thất bại.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("BranchesBo CompanyBranchDelete branchId {0}, companyId {1} EX: {2}", branchId, companyId, ex.ToString());
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xóa chi nhánh.";
            }

            return response;
        }
    }
}

