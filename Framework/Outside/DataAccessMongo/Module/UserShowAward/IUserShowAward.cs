using System.Collections.Generic;
using DataAccessMongo.Model.UserShowAward;

namespace DataAccessMongo.Module.UserShowAward
{
    public interface IUserShowAward
    {
        bool UserShowAwardInsert(UserShowAwardMongoModel data);
        List<UserShowAwardMongoModel> UserShowAwardShow(int userId, ref long outResult);
        List<UserShowAwardMongoModel> UserShowAwardShow(int userId, int type, ref long outResult);
        List<UserShowAwardMongoModel> UserShowAwardShowNotDefault(int userId, ref long outResult);
    }
}