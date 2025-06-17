using System.Collections.Generic;
using DataAccessMongo.Model.LogUserPlayGame;

namespace DataAccessMongo.Module.LogUserPlayGame
{
    public interface ILogUserPlayGameDao
    {
        LogUserPlayGameModel GetRequestLog(int userid);
        void UpdateRequestLogGames(int userId, List<LogUserPlayGameGamesModel> listGames);

        void InsertLog(LogUserPlayGameModel data);
    }
}