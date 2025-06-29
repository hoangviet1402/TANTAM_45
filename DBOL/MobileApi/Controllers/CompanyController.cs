﻿using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Auth;
using BussinessObject.Models.Company;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TanTamApi.Helper;
using TanTamApi.JWT.Helper;
using WebUtility;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/Company")]
    public class CompanyController : ApiController
    {
        [JWT.Middleware.Authorize]
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
                CommonLogger.DefaultLogger.ErrorFormat("CompanyController CreateBranches EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình tạo chi nhánh.";               
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [JWT.Middleware.Authorize]
        [HttpPost, Route("list-branch")]
        public HttpResponseMessage ListBranch()
        {
            var response = new ApiResult<ListBranchesReponse>()
            {
                Data = new ListBranchesReponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin tài khoản hoặc công ty không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                response = BoFactory.Branches.CompanyGetAllBranches(companyId, 10000);                
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyController ListBranch EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình lấy danh sách chi nhánh.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [JWT.Middleware.Authorize]
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
                var accountId = JwtHelper.GetAccountIdFromToken(Request);
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
                CommonLogger.DefaultLogger.ErrorFormat("CompanyController CreateDepartment EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình tạo phòng ban.";               
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [JWT.Middleware.Authorize]
        [HttpPost, Route("list-departments")]
        public HttpResponseMessage ListDepartments([FromBody] string refreshToken)
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
                var accountId = JwtHelper.GetAccountIdFromToken(Request);
                if (companyId <= 0 || accountId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin tài khoản hoặc công ty không hợp lệ.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                response = BoFactory.Department.CompanyGetAllDepartment(companyId);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyController ListDepartments EX:", ex);
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình lấy danh sách phòng ban.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [JWT.Middleware.Authorize]
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

        [JWT.Middleware.Authorize]
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
                if (request == null || request.Posisions == null || request.Posisions.Count == 0)
                {
                    response.Message = "Vui  lòng nhập các vị trí cần thiết cho công ty.";
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                request.CompanyId = JwtHelper.GetCompanyIdFromToken(Request);

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
                CommonLogger.DefaultLogger.ErrorFormat("CompanyController CreatePosition EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
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
                var result = BoFactory.Company.ListBusinessResponseAsync();
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

        [JWT.Middleware.Authorize]
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
                if (string.IsNullOrWhiteSpace(request.CompanyAddress))
                {
                    response.Message = "Địa chỉ công ty không được để trống.";
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (string.IsNullOrWhiteSpace(request.Email) || ValidationHelper.IsValidEmail(request.Email) == false)
                {
                    response.Message = "Email không hợp lệ.";
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                if (request.HearAbout == null || request.UsePurpose == null)
                {
                    response.Message = "Thông tin về nguồn gốc và mục đích sử dụng không được để trống.";
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                response = BoFactory.Company.UpdateUserAndShopNameAsync(request);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyController UpdateUserAndShopName EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [JWT.Middleware.Authorize]
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
                response = BoFactory.Company.CompanyDetail(request);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CompanyController CompanyDetail EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
