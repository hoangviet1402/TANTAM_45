using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using DataAccess;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BussinessObject.Bo.TanTamBo
{
    public class TutorialsBo : BaseBo<DBNull>
    {
        public TutorialsBo()
            : base(DaoFactory.TanTam)
        {
        }

        public ApiResult<int> UserTutorials_Initialize(int accountMapID)
        {
            var response = new ApiResult<int>
            {
                Data = 0,
                Code = ResponseResultEnum.NoData.Value(),
                Message = ResponseResultEnum.NoData.Text(),
            };

            try
            {
                if (accountMapID > 0)
                {
                    DaoFactory.Tutorials.UserTutorials_Initialize(accountMapID);
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.Success.Text();
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("UserBo.GetUserListAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public ApiResult<int> UserTutorials_Complete(int accountMapID, string tutorialType)
        {
            var response = new ApiResult<int>
            {
                Data = 0,
                Code = ResponseResultEnum.NoData.Value(),
                Message = ResponseResultEnum.NoData.Text(),
            };

            try
            {
                if (string.IsNullOrEmpty(tutorialType) == false && accountMapID > 0)
                {
                    var Tutorials = DaoFactory.Tutorials.UserTutorials_GetPendingTutorials(accountMapID);
                    if (Tutorials != null && Tutorials.Any())
                    {
                        if (tutorialType == "all")
                        {
                            DaoFactory.Tutorials.UserTutorials_Complete(accountMapID, -1);
                        }
                        else
                        {
                            if (Tutorials.FirstOrDefault(x => x.Type == tutorialType) != null)
                            {
                                DaoFactory.Tutorials.UserTutorials_Complete(accountMapID, Tutorials.FirstOrDefault(x => x.Type == tutorialType).Id);
                            }
                        }
                    }
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.Success.Text();
                }
               
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("UserBo.GetUserListAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public List<Ins_UserTutorials_GetPendingTutorials_Result> UserTutorials_GetPendingTutorials(int accountMapID)
        {
            return  DaoFactory.Tutorials.UserTutorials_GetPendingTutorials(accountMapID);
        }
    }
}