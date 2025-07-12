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
                var total = 0;
                int positionID = 0;
                var dataBrandID = DaoFactory.Branches.GetAllBranchs(companyId, out total).OrderBy(x => x.BranchId).ToList();

                foreach (var item in request.Posisions)
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
                            Branchs = dataBrandID.Select(x => new PosisionsBranchsResponseList
                            {
                                Id = x.BranchId,
                                Name = x.BranchName ?? string.Empty,
                                Color = x.Color ?? string.Empty,
                            }).Distinct().ToList()
                        }
                    );
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
                CommonLogger.DefaultLogger.ErrorFormat("CreatePositionInAllBranchesAsync Exception EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Tạo phòng ban thất bại";
            }

            return response;
        }
    }
}
