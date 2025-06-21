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
                foreach (var branch in request)
                {
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
                        branch.RegionId,
                        branch.IsOnboarding ?? 0,
                        branch.Latitude ?? 0,
                        branch.Longitude ?? 0,
                        companyId,
                        StringCommon.NormalizeText(branch.Name, "-"),
                        StringCommon.NormalizeText(branch.Name, "_").ToUpper());

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
                    branchIds++;
                }

                if (branchIds == request.Count())
                {

                    DaoFactory.Company.Employee_AddIntoBranch(accountid, branchId, true);
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
                CommonLogger.DefaultLogger.ErrorFormat("SetupCompany_CreateBranches accountId {0} ,companyId {1} EX:", accountid, companyId, ex);
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
                CommonLogger.DefaultLogger.ErrorFormat("CompanyGetAllBranches companyId {0} EX:", companyId, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lấy danh sách phòng ban thất bại";
            }

            return response;
        }
    }
}
