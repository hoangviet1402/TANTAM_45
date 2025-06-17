using System.Collections.Generic;
using DataAccessMongo.Model.UserInfoCollection;

namespace DataAccessMongo.Module.UserInfoCollection
{
    public interface IUserInfo
    {
        List<UserInfoModel> GetTopCurrentCoin(int top);
        List<UserInfoModel> GetTopUser(int top);
        List<UserInfoModel> GetByUserId(int userId);

        List<UserInfoModel> GetByUserIdListId(List<int> listUserId);

        long UpdateUserAchievementComplete(int userId, decimal totalPoint, decimal totalQuantity, decimal totalGold,
            decimal totalReceivedQuantity, decimal totalReceivedGold);
    }
}