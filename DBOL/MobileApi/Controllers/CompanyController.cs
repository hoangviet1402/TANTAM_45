using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Auth;
using BussinessObject.Models.Company;
using DataAccess;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using Newtonsoft.Json;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TanTamApi.Helper;
using TanTamApi.JWT.Helper;
using TanTamApi.JWT.Middleware;
using WebUtility;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/Company")]
    public class CompanyController : ApiController
    {
        [ApiAuthorize]
        [HttpPost, Route("region-add")]
        public HttpResponseMessage CreateRegion([FromBody] CreateRegionRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountMapId = JwtHelper.GetAccountMapIDFromToken(Request);
                if (companyId <= 0 || accountMapId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin tài khoản hoặc công ty không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (request == null || string.IsNullOrEmpty(request.Name))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập tên vùng.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                response = BoFactory.Branches.CompanyRegionCreate(request.Name, request.Description, companyId);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController CreateBranches EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình tạo vùng.";
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("region-list")]
        public HttpResponseMessage RegionList([FromBody] RegionListRequest request)
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);

                // Trả lỗi nếu token không có companyId
                if (companyId <= 0)
                {
                    var errorRes = new ApiResult<List<RegionDetailResponse>>()
                    {
                        Data = new List<RegionDetailResponse>(),
                        Code = ResponseResultEnum.InvalidInput.Value(),
                        Message = "Thông tin tài khoản hoặc công ty không hợp lệ."
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorRes);
                }

                // Không truyền tham số filter ⇒ trả danh sách thuần (không meta)
                if (request == null || (request.IsAll == null && request.Page == null))
                {
                    var internalRes = BoFactory.Branches.CompanyRegionGetList(companyId, null, true);

                    var plainRes = new ApiResult<List<RegionDetailResponse>>()
                    {
                        Data = internalRes.Data?.Items ?? new List<RegionDetailResponse>(),
                        Code = internalRes.Code,
                        Message = internalRes.Message
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, plainRes);
                }
                else
                {
                    // Có filter ⇒ dùng cấu trúc meta/items như cũ
                    var detailRes = BoFactory.Branches.CompanyRegionGetList(companyId, request.Page, request.IsAll);
                    return Request.CreateResponse(HttpStatusCode.OK, detailRes);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyController RegionList EX: {0}", ex);
                var errorRes = new ApiResult<List<RegionDetailResponse>>()
                {
                    Data = new List<RegionDetailResponse>(),
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách vùng."
                };
                return Request.CreateResponse(HttpStatusCode.OK, errorRes);
            }
        }

        [ApiAuthorize]
        [HttpPost, Route("region-update")]
        public HttpResponseMessage RegionUpdate([FromBody] CreateRegionRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                response = BoFactory.Branches.CompanyRegionUpdate(request.Name, request.Description, companyId, request.Id);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController RegionList EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình lấy danh sách vùng của công ty.";
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("region-delete")]
        public HttpResponseMessage RegionDelete([FromBody] DeleteRegionRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountMapId = JwtHelper.GetAccountMapIDFromToken(Request);
                
                if (companyId <= 0 || accountMapId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin tài khoản hoặc công ty không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (request == null || string.IsNullOrEmpty(request.Id))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID vùng cần xóa.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                // Convert string ID to int
                if (!int.TryParse(request.Id, out int regionId))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID vùng không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                response = BoFactory.Branches.CompanyRegionDelete(regionId, companyId);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController RegionDelete EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xóa vùng.";
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("branch-add")]
        public HttpResponseMessage CreateBranches([FromBody] List<CreateBranchesRequest> request)
        {
            var response = new ApiResult<List<CreateBranchesResponse>> ()
            {
                Data = new List<CreateBranchesResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountMapId = JwtHelper.GetAccountMapIDFromToken(Request);
                if (companyId <= 0 || accountMapId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin tài khoản hoặc công ty không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (request == null || request.Count == 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Danh sách chi nhánh không được để trống.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);               
                }

                response = BoFactory.Branches.SetupCompany_CreateBranches(companyId, accountMapId, request);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController CreateBranches EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình tạo chi nhánh.";               
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("branch-list")]
        public HttpResponseMessage ListBranch([FromBody] BranchListRequest request)
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);

                // Trả lỗi nếu token không có companyId
                if (companyId <= 0)
                {
                    var errorRes = new ApiResult<List<BranchDetailResponse>>()
                    {
                        Data = new List<BranchDetailResponse>(),
                        Code = ResponseResultEnum.InvalidInput.Value(),
                        Message = "Thông tin tài khoản hoặc công ty không hợp lệ."
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorRes);
                }

                // Không truyền tham số filter ⇒ trả danh sách thuần (không meta)
                if (request == null || (request.IsAll == null && request.RegionId == null && request.Page == null))
                {
                    var internalRes = BoFactory.Branches.CompanyBranchGetList(companyId, null, true, null);

                    return Request.CreateResponse(HttpStatusCode.OK, internalRes);
                }
                else
                {
                    // Có filter ⇒ dùng cấu trúc meta/items như cũ
                    var detailRes = BoFactory.Branches.CompanyBranchGetList(companyId, request.Page, request.IsAll, request.RegionId);
                    return Request.CreateResponse(HttpStatusCode.OK, detailRes);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyController ListBranchByRegion EX: {0}", ex);
                var errorRes = new ApiResult<List<BranchDetailResponse>>()
                {
                    Data = new List<BranchDetailResponse>(),
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách chi nhánh."
                };
                return Request.CreateResponse(HttpStatusCode.OK, errorRes);
            }
        }

        [ApiAuthorize]
        [HttpPost, Route("branch-update")]
        public HttpResponseMessage UpdateBranch([FromBody] UpdateBranchRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountMapId = JwtHelper.GetAccountMapIDFromToken(Request);
                if (companyId <= 0 || accountMapId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin tài khoản hoặc công ty không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Dữ liệu cập nhật không được để trống.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                response = BoFactory.Branches.CompanyBranchUpdate(request, companyId);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController UpdateBranch EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình cập nhật chi nhánh.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("branch-delete")]
        public HttpResponseMessage DeleteBranch([FromBody] DeleteBranchRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountMapId = JwtHelper.GetAccountMapIDFromToken(Request);
                
                if (companyId <= 0 || accountMapId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin tài khoản hoặc công ty không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (request == null || string.IsNullOrEmpty(request.Id))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID chi nhánh cần xóa.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                // Convert string ID to int
                if (!int.TryParse(request.Id, out int branchId))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID chi nhánh không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                response = BoFactory.Branches.CompanyBranchDelete(branchId, companyId);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController DeleteBranch EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xóa chi nhánh.";
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("department-add")]
        public HttpResponseMessage CreateDepartment([FromBody] CreateDepartmentRequest request)
        {
            var response = new ApiResult<List<CreateDepartmentResponse>>()
            {
                Data = new List<CreateDepartmentResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountId = JwtHelper.GetAccountMapIDFromToken(Request);
                if (companyId <= 0 || accountId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin tài khoản hoặc công ty không hợp lệ.";                   
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (request == null || request.Name == null || request.Name.Count == 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Danh sách phòng ban không được để trống.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                
                response = BoFactory.Department.SetupCompany_CreateDepartmentAllBranchAsync(companyId, request);               
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController CreateDepartment EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình tạo phòng ban.";               
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("department-list")]
        public HttpResponseMessage ListDepartment([FromBody] ListDepartmentsByBranchRequest request)
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountId = JwtHelper.GetAccountIdFromToken(Request);

                // Trả lỗi nếu token không có companyId hoặc accountId
                if (companyId <= 0 || accountId <= 0)
                {
                    var errorRes = new ApiResult<List<CreateDepartmentResponse>>()
                    {
                        Data = new List<CreateDepartmentResponse>(),
                        Code = ResponseResultEnum.InvalidInput.Value(),
                        Message = "Thông tin tài khoản hoặc công ty không hợp lệ."
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorRes);
                }

                // Không truyền tham số filter ⇒ trả danh sách thuần (không meta)
                if (request == null || (request.IsAll == null && request.BranchIds == null))
                {
                    var internalRes = BoFactory.Department.CompanyDepartmentGetList(companyId, null, true, null);

                    var plainRes = new ApiResult<List<CreateDepartmentResponse>>()
                    {
                        Data = internalRes.Data?.Items ?? new List<CreateDepartmentResponse>(),
                        Code = internalRes.Code,
                        Message = internalRes.Message
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, plainRes);
                }
                else
                {
                    // Có filter ⇒ dùng cấu trúc meta/items như cũ
                    var detailRes = BoFactory.Department.CompanyDepartmentGetList(companyId, null, request.IsAll, request.BranchIds);
                    return Request.CreateResponse(HttpStatusCode.OK, detailRes);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController ListDepartmentsByBranch EX:", ex);
                var errorRes = new ApiResult<List<CreateDepartmentResponse>>()
                {
                    Data = new List<CreateDepartmentResponse>(),
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách phòng ban theo chi nhánh."
                };
                return Request.CreateResponse(HttpStatusCode.OK, errorRes);
            }
        }

        [ApiAuthorize]
        [HttpPost, Route("department-update")]
        public HttpResponseMessage UpdateDepartment([FromBody] UpdateDepartmentRequest request)
        {
            var response = new ApiResult<UpdateDepartmentResponse>()
            {
                Data = new UpdateDepartmentResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountId = JwtHelper.GetAccountIdFromToken(Request);
                
                if (companyId <= 0 || accountId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin tài khoản hoặc công ty không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Dữ liệu cập nhật không được để trống.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                response = BoFactory.Department.UpdateDepartment(companyId, request);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController UpdateDepartment EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình cập nhật phòng ban.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("department-delete")]
        public HttpResponseMessage DeleteDepartment([FromBody] DeleteDepartmentRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountId = JwtHelper.GetAccountIdFromToken(Request);
                
                if (companyId <= 0 || accountId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin tài khoản hoặc công ty không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (request == null || request.Id <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID phòng ban cần xóa.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                response = BoFactory.Department.DeleteDepartment(companyId, request);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController DeleteDepartment EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xóa phòng ban.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        
        [ApiAuthorize]
        [HttpPost, Route("api/auth/refreshtoken")]
        public HttpResponseMessage CompanyGetALLdepartments([FromBody] string refreshToken)
        {
            var response = new ApiResult<RefeshTokenResponse>()
            {
                Data = new RefeshTokenResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };
           
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("position-add")]
        public HttpResponseMessage CreatePosition([FromBody] CreatePosisionInAllBranchesRequest request)
        {
            var response = new ApiResult<List<CreatePosisionResponse>>()
            {
                Data = new List<CreatePosisionResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                if (request == null || request.Positions == null || request.Positions.Count == 0)
                {
                    response.Message = "Vui  lòng nhập các vị trí cần thiết cho công ty.";
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                request.CompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountId = JwtHelper.GetAccountMapIDFromToken(Request);
                if (request.CompanyId <= 0)
                {
                    response.Message = "Thông tin công ty không hợp lệ.";
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                response = BoFactory.Position.SetupCompany_CreatePositionInAllBranchesAsync(request.CompanyId, request);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController CreatePosition EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("position-list")]
        public HttpResponseMessage ListPosition([FromBody] ListPositionByBranchRequest request)
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                if (companyId <= 0)
                {
                    var errorResponse = new ApiResult<List<PositionByBranchResponse>>()
                    {
                        Data = new List<PositionByBranchResponse>(),
                        Code = ResponseResultEnum.InvalidInput.Value(),
                        Message = "Thông tin công ty không hợp lệ."
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
                }

                // Nếu không truyền request (hoặc rỗng), trả về data mặc định với is_all = true
                if (request == null || (request.IsAll == null && request.BranchIds == null && request.DepartmentIds == null && request.Page == null))
                {
                    var internalRes = BoFactory.Position.CompanyPositionGetList(companyId, null, true, null, null);

                    var plainResponse = new ApiResult<List<PositionByBranchResponse>>()
                    {
                        Data = internalRes.Data?.Items ?? new List<PositionByBranchResponse>(),
                        Code = internalRes.Code,
                        Message = internalRes.Message
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, plainResponse);
                }
                else
                {
                    var response = BoFactory.Position.CompanyPositionGetList(companyId, request.Page, request.IsAll, request.BranchIds, request.DepartmentIds);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController ListPositionByBranch EX:", ex);
                var errorResponse = new ApiResult<List<PositionByBranchResponse>>()
                {
                    Data = new List<PositionByBranchResponse>(),
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách vị trí."
                };
                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }
        }

        [ApiAuthorize]
        [HttpPost, Route("position-update")]
        public HttpResponseMessage UpdatePosition([FromBody] UpdatePositionRequest request)
        {
            var response = new ApiResult<UpdatePositionResponse>()
            {
                Data = new UpdatePositionResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Dữ liệu cập nhật không được để trống.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                var companyId = JwtHelper.GetCompanyIdFromToken(Request);

                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin công ty không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                response = BoFactory.Position.UpdatePositionAsync(request, companyId);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController UpdatePosition EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình cập nhật vị trí.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("position-delete")]
        public HttpResponseMessage DeletePosition([FromBody] DeletePositionRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountId = JwtHelper.GetAccountIdFromToken(Request);
                
                if (companyId <= 0 || accountId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin tài khoản hoặc công ty không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (request == null || string.IsNullOrEmpty(request.Id))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID vị trí cần xóa.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                // Convert string ID to int
                if (!int.TryParse(request.Id, out int positionId))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID vị trí không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                response = BoFactory.Position.DeletePosition(positionId, companyId);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController DeletePosition EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xóa vị trí.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost, Route("element/list-business-field")]
        public HttpResponseMessage listBusinessField()
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                CommonLogger.PerformanceLogger.DebugFormat("element/list-business-field request {0}", JsonConvert.SerializeObject(response));
                var result = BoFactory.Company.ListBusinessResponseAsync();
                CommonLogger.PerformanceLogger.DebugFormat("element/list-business-field result {0}", JsonConvert.SerializeObject(response));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyController list-business-field EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình lấy thông tin.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("update-user-and-shop-name")]
        public HttpResponseMessage UpdateUserAndShopName([FromBody] UpdateInfoWhenSinupRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = ResponseResultEnum.InvalidData.Text();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                request.CompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                request.AccountId = JwtHelper.GetAccountIdFromToken(Request);
                var accountMapId = JwtHelper.GetAccountMapIDFromToken(Request);

                if (request.AccountId <= 0 || request.CompanyId <= 0 || string.IsNullOrWhiteSpace(request.CompanyName))
                {
                    response.Message = "Thông tin tài khoản hoặc công ty không hợp lệ.";
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                if (request.CompanyLatitude < -90 || request.CompanyLatitude > 90 || request.CompanyLongitude < -180 || request.CompanyLongitude > 180)
                {
                    response.Message = "Vĩ độ hoặc kinh độ không hợp lệ.";
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                if (string.IsNullOrWhiteSpace(request.Email) || ValidationHelper.IsValidEmail(request.Email) == false)
                {
                    request.Email = string.Format("{0}@mail.com", StringCommon.NormalizeText(request.CompanyName, "_"));
                }

                if (request.HearAbout == null || request.UsePurpose == null)
                {
                    response.Message = "Thông tin về nguồn gốc và mục đích sử dụng không được để trống.";
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                //CommonLogger.PerformanceLogger.DebugFormat("update-user-and-shop-name request {0}", JsonConvert.SerializeObject(request));
                response = BoFactory.Company.UpdateUserAndShopNameAsync(request);

                if (response.Code == ResponseResultEnum.Success.Value())
                {
                    response.Data = BoFactory.Branches.CompanyRegionCreate(request.CompanyName, request.CompanyName, request.CompanyId).Data;
                    var addEmployeeIntoRegion = DaoFactory.Company.Employee_AddIntoRegion(accountMapId, response.Data, true);
                    if (addEmployeeIntoRegion <= 0)
                    {
                        response.Code = ResponseResultEnum.SystemError.Value();
                        response.Message = "Đã xảy ra lỗi trong quá trình thêm nhân viên vào vùng.";
                    }
                    // add Tutorials
                    BoFactory.Tutorials.UserTutorials_Initialize(accountMapId);
                }
                
                // CommonLogger.PerformanceLogger.DebugFormat("update-user-and-shop-name request {0}", JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController UpdateUserAndShopName EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("detail")]
        public HttpResponseMessage CompanyDetail([FromBody] string refreshToken)
        {
            var response = new ApiResult<CompanyDetailResponse>()
            {
                Data = new CompanyDetailResponse(),
                Code = ResponseResultEnum.InvalidInput.Value(),
                Message = ResponseResultEnum.InvalidInput.Text()
            };

            try
            {
                CompanyDetailRequest request = new CompanyDetailRequest();
                request.CompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                request.AccountId = JwtHelper.GetAccountIdFromToken(Request);
                var accountMapId = JwtHelper.GetAccountMapIDFromToken(Request);
                CommonLogger.PerformanceLogger.DebugFormat("detail request {0}", JsonConvert.SerializeObject(request));
                response = BoFactory.Company.CompanyDetail(request, accountMapId);
                CommonLogger.PerformanceLogger.DebugFormat("detail response {0}", JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController CompanyDetail EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("step-skip")]
        public HttpResponseMessage CompanyCreateStepSkip([FromBody] StepSkipWhenSinupRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.InvalidInput.Value(),
                Message = ResponseResultEnum.InvalidInput.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountMapId = JwtHelper.GetAccountMapIDFromToken(Request);
                CommonLogger.PerformanceLogger.DebugFormat("detail request {0}, step {1}", companyId,  request.step);
                response = BoFactory.Company.CompanyCreateStepSkip(companyId, accountMapId, request.step);
                CommonLogger.PerformanceLogger.DebugFormat("detail response {0}", JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController CompanyDetail EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("tutorials-complete")]
        public HttpResponseMessage TutorialsComplete([FromBody] TutorialsCompleteRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.InvalidInput.Value(),
                Message = ResponseResultEnum.InvalidInput.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountMapId = JwtHelper.GetAccountMapIDFromToken(Request);
               
                response = BoFactory.Tutorials.UserTutorials_Complete(accountMapId, request.code);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CompanyController CompanyDetail EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
