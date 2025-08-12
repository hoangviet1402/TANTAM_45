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

        public ApiResult<CompanyDetailResponse> CompanyDetail(CompanyDetailRequest request, int accountMapId)
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

                try
                {
                    var Tutorials = DaoFactory.Tutorials.UserTutorials_GetAll(accountMapId);
                    if (Tutorials != null && Tutorials.Any())
                    {
                        response.Data.Tutorials = new List<TutorialsInfo>();
                        response.Data.Tutorials.AddRange(Tutorials.Select(x => new TutorialsInfo()
                        {
                            Code = x.Type,
                            IsDone = x.IsCompleted == true ? 1 : 0,
                            Weight = x.StepOrder
                        }));
                    }
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat("CompanyDetail companyId {0} EX:", request.CompanyId, ex);
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
                CommonLogger.DefaultLogger.ErrorFormat("CompanyDetail FullName {0}, Address {1} Exception EX: {2}", request.CompanyId, request.AccountId, ex.ToString());
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

        public ApiResult<int> CompanyCreateStepSkip(int companyId,int accountid, int step)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                int total = 0;
                int branchId = 0;
                int departmentId = 0;
                int positionID = 0;
                var regionid = DaoFactory.Branches.GetAllRegion(companyId).FirstOrDefault().ID;
                List<Ins_CompanyBranch_GetAllByCompany_Result> all_branchs = new List<Ins_CompanyBranch_GetAllByCompany_Result>();
                // skip từ chi nhánh đổ xuống
                if (step <= SetupStepEnum.ONBOARDING_CREATE_BRANCH.Value()) 
                {
                    all_branchs = DaoFactory.Branches.GetAllBranchs(companyId, out total);
                    if (all_branchs == null || all_branchs.Any() == false) // chưa có chi nhánh
                    {
                        branchId = DaoFactory.Branches.CreateBranche(
                                "Chi Nhánh Trung Tâm",
                                "",
                                regionid,
                                1,
                                0,
                                0,
                                companyId,
                                StringCommon.NormalizeText("Chi Nhánh Trung Tâm", "-"),
                                StringCommon.NormalizeText("Chi Nhánh Trung Tâm", "_").ToUpper());
                        DaoFactory.Company.UpdateCompanyStep(companyId, SetupStepEnum.ONBOARDING_CREATE_BRANCH.Value());
                        all_branchs.Add(new Ins_CompanyBranch_GetAllByCompany_Result()
                        {
                            BranchId = branchId
                        });

                        try
                        {
                            // tạo thêm thông tin wifi cho chi nhánh 
                            var resultWifi = DaoFactory.Wifi.CreateWifi(200, 0, 10, 0, 0, 0, "Chi Nhánh Trung Tâm", branchId, "", Wifi_type_Enum.location.Value());
                        }
                        catch (Exception ex)
                        {
                           
                        }

                        DaoFactory.Company.Employee_AddIntoBranch(accountid, branchId, true);
                    }
                    else
                    {
                        DaoFactory.Company.UpdateCompanyStep(companyId, SetupStepEnum.ONBOARDING_CREATE_BRANCH.Value());
                    }
                }
                // skip từ phòng ban đổ xuống
                if (step <= SetupStepEnum.ONBOARDING_CREATE_DEPARTMENT.Value()) 
                {
                    if (all_branchs == null || all_branchs.Any() == false)
                    {
                        all_branchs = DaoFactory.Branches.GetAllBranchs(companyId, out total);
                    }
                    if (all_branchs != null && all_branchs.Any() == true)
                    {
                        var all_Department = DaoFactory.Department.GetAllDepartments(companyId);
                        if (all_Department != null && all_Department.Any() == false)
                        {
                            departmentId = DaoFactory.Department.CreateDepartmentInAllBranches_Simple(
                                 "Giám Đốc",
                                 companyId,
                                 0,
                                 StringCommon.NormalizeText("Giám Đốc", "-"),
                                 StringCommon.NormalizeText("Giám Đốc", "_").ToUpper(),
                                 1
                            );
                            foreach (var item_dataBrandID in all_branchs)
                            {
                                DaoFactory.Department.CreateRelate(departmentId, item_dataBrandID.BranchId);
                            }
                            DaoFactory.Company.UpdateCompanyStep(companyId, SetupStepEnum.ONBOARDING_CREATE_DEPARTMENT.Value());
                        }
                        else
                        {
                            DaoFactory.Company.UpdateCompanyStep(companyId, SetupStepEnum.ONBOARDING_CREATE_DEPARTMENT.Value());
                        }
                    }
                }
                // skip từ chức vụ đổ xuống
                if (step <= SetupStepEnum.ONBOARDING_CREATE_POSITION.Value()) 
                {
                    if (all_branchs == null || all_branchs.Any() == false)
                    {
                        all_branchs = DaoFactory.Branches.GetAllBranchs(companyId, out total);
                    }
                    if (all_branchs != null && all_branchs.Any() == true)
                    {
                        var all_positionID = DaoFactory.Position.GetListByCompanyId(companyId,1,1,true);
                        if (all_positionID == null || all_positionID.Any() == false)
                        {
                            positionID = DaoFactory.Position.CreatePosition_Simple(
                                "Giám Đốc",
                                StringCommon.NormalizeText("Giám Đốc", " - "),
                                StringCommon.NormalizeText("Giám Đốc", "_").ToUpper(),
                                companyId,
                                0
                            );

                            foreach (var item_dataBrandID in all_branchs)
                            {
                                DaoFactory.Position.CreatePosition_CreateRelate(
                                    item_dataBrandID.BranchId,
                                    positionID,
                                    0,
                                    0
                                );
                            }
                            DaoFactory.Company.UpdateCompanyStep(companyId, SetupStepEnum.ONBOARDING_CREATE_POSITION.Value());
                        }
                        else
                        {
                            DaoFactory.Company.UpdateCompanyStep(companyId, SetupStepEnum.ONBOARDING_CREATE_POSITION.Value());
                        }
                    }
                }
                // skip từ tạo nhân viên
                if (step <= SetupStepEnum.ONBOARDING_CREATE_EMPLOYEE.Value())
                {
                    var ONBOARDING_CREATE_EMPLOYEE = DaoFactory.Company.UpdateCompanyStep(companyId, SetupStepEnum.ONBOARDING_CREATE_EMPLOYEE.Value());
                }
                // skip từ tạo ca
                if (step <= SetupStepEnum.ONBOARDING_CREATE_SHIFT.Value())
                {
                    var ONBOARDING_CREATE_SHIFT = DaoFactory.Company.UpdateCompanyStep(companyId, SetupStepEnum.ONBOARDING_CREATE_SHIFT.Value());
                }
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
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
    }
}
