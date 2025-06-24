using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Company;
using DataAccess;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Bo.TanTamBo
{
    public class CompanyBo : BaseBo<DBNull>
    {
        public CompanyBo()
            : base(DaoFactory.Company)
        {
        }

        public ApiResult<CompanyDetailResponse> CompanyDetail(CompanyDetailRequest request)
        {
            var response = new ApiResult<CompanyDetailResponse>()
            {
                Data = new CompanyDetailResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var getCompanyInfo = DaoFactory.Company.GetCompanyInfo(request.CompanyId);
                if (getCompanyInfo == null)
                {
                    response.Data = null;
                    response.Code = ResponseResultEnum.CompanyNoData.Value();
                    response.Message = "Thông tin cty không tồn tại";
                    return response;
                }

                var getAccountInfo = DaoFactory.Company.GetAccountInfo(request.AccountId, request.CompanyId);
                if (getAccountInfo == null)
                {
                    response.Data = null;
                    response.Code = ResponseResultEnum.AccountNotExist.Value();
                    response.Message = "Thông tin tài khoản không tồn tại";
                    return response;
                }

                response.Data.Id = getCompanyInfo.Id;
                response.Data.Name = getCompanyInfo.FullName;
                response.Data.Username = getAccountInfo.EmployeesFullName;
                response.Data.Alias = getCompanyInfo.Alias;
                response.Data.AddressLat = getCompanyInfo.Latitude;
                response.Data.AddressLng = getCompanyInfo.Longitude;
                response.Data.Address = getCompanyInfo.Address;
                response.Data.CreatedAt = getCompanyInfo.CreateDate;
                response.Data.Email = getAccountInfo.Email;
                response.Data.TypeOfBusiness = getCompanyInfo.BusinesFieldIds;
                response.Data.Phone = getAccountInfo.Phone;

                var getSetupStep = DaoFactory.Company.GetCompanyGetSetupStep(request.CompanyId, request.AccountId);

                if (getSetupStep != null && getSetupStep.Any())
                {
                    response.Data.SetupSteps = new List<SetupStep>();
                    response.Data.SetupSteps.AddRange(getSetupStep.Select(x => new SetupStep()
                    {
                        Code = x.Code,
                        IsDone = x.IsDone == true ? 1 : 0,
                        Weight = x.Weight ?? 0
                    }));
                    response.Data.GetStartedStep = getSetupStep.Any(x => x.IsDone.GetValueOrDefault(false) == true) ? getSetupStep.Where(x => x.IsDone == true).Max(x => x.Code.GetValueOrDefault(0)) : 0;
                }
                response.Data.FirstStepModalOff = getCompanyInfo.FirstStepModalOff ?? 0;
                response.Data.TimeFormat = "24hour";
                response.Data.DateFormat = "dd/MM/yyyy";
                response.Data.IsUsingOnleaveV2 = true;
                response.Data.IsUsingCameraAi = false;
                response.Data.TalentManagement = false;
                response.Data.ElearningManagement = false;
                response.Data.Integration = new Integration()
                {
                    DigitalSignature = false
                };
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "lấy danh thông tin thành công";
                return response;
            }
            catch (Exception ex)
            {
                response.Data = null;
                CommonLogger.DefaultLogger.ErrorFormat("CompanyDetail FullName {0}, Address {1} Exception EX:", request.CompanyId, request.AccountId, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống";
            }

            return response;
        }
       
        public ApiResult<List<ListBusinessResponse>> ListBusinessResponseAsync()
        {
            var response = new ApiResult<List<ListBusinessResponse>>()
            {
                Data = new List<ListBusinessResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var departments = DaoFactory.Company.BusinessGetList();

                if (departments.Any())
                {
                    response.Data = departments.Select(d => new ListBusinessResponse
                    {
                        Id = d.Id,
                        Value = "",
                        Name = d.Business,
                        Alias = d.Alias,
                        IndexNum = d.IndexNum ?? 0
                    }).ToList();

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.Success.Text();
                    return response;
                }

                response.Code = ResponseResultEnum.NoData.Value();
            }
            catch (Exception ex)
            {
                //LoggerHelper.Error($"ListBusinessResponseAsync Exception", ex);
                CommonLogger.DefaultLogger.ErrorFormat("CompanyBo ListBusinessResponseAsync Exception EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
            }

            return response;
        }

        public ApiResult<int> UpdateUserAndShopNameAsync(UpdateInfoWhenSinupRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                DaoFactory.Company.UpdateInfoWhenSinup(
                    request.AccountId,
                    request.CompanyId,
                    request.CompanyName,
                    StringCommon.NormalizeText(request.CompanyName, "_"),
                    request.CompanyLatitude ?? 0,
                    request.CompanyLongitude ?? 0,
                    request.CompanyNumberEmploye,
                    request.CompanyAddress,
                    request.Email,
                    string.Join(",", request.HearAbout),
                    string.Join(",", request.UsePurpose),
                    string.Join(",", request.BusinesFieldIds));
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyBo UpdateUserAndShopNameAsync request {0} Exception EX:",Common.TrySerializeObject(request), ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
            }

            return response;
        }
    }
}
