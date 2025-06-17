using DataAccessMongo.Module.AchievementCollectionsDao;
using DataAccessMongo.Module.Defender;
using DataAccessMongo.Module.E2003_LogAllEventStatusCollection;
using DataAccessMongo.Module.LogUserPlayGame;
using DataAccessMongo.Module.RequestLogCollection;
using DataAccessMongo.Module.UserCollections;
using DataAccessMongo.Module.UserInfoCollection;
using DataAccessMongo.Module.UserLoginLogCollection;
using DataAccessMongo.Module.UserShowAward;

namespace DataAccessMongo
{
    public class DaoMongoFactory
    {
        public static ILogUserPlayGameDao LogUserPlayGame => new LogUserPlayGameDao();
        public static IUser UserAchievement => new User();

        public static IUserInfo UserInfo => new UserInfo();
        public static IAchievementCollectionDao Achievement => new AchievementCollectionDao();
        public static IUserLoginLog UserLoginLog => new UserLoginLog();

        public static IUserShowAward UserShowAward => new UserShowAward();
        public static IDefender Defender => new Defender();

        public static IRequestLog RequestLog => new RequestLog();

        public static E2003LogAllEventStatus E2003LogAllEventStatus => new E2003LogAllEventStatus();
    }
}