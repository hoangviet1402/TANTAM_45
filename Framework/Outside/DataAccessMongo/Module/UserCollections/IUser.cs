using System.Collections.Generic;
using DataAccessMongo.Model.UserCollections;

namespace DataAccessMongo.Module.UserCollections
{
    public interface IUser
    {
        List<E1805_User_Achievement> LogUserStarByUserId(int userId);

        List<E1805_User_Achievement> LogUserAchievementSearch(int userId, int achievementId, int type, int gameId,
            int slotType, int star);

        void InsertUserAchievement(E1805_User_Achievement user);

        long UpdateUserAchievement(int userId, int achievementId, double star, decimal number, bool isShow);

        long ReceiveUserAchievementStatus(int userId, int achievementId, double star, bool isComplete,
            decimal numberCurrent);
    }
}