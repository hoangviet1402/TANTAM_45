using System;
using System.Collections.Generic;
using DataAccessMongo.Base;
using DataAccessMongo.Enum;
using DataAccessMongo.Model.LogUserPlayGame;
using MongoDB.Driver;

namespace DataAccessMongo.Module.LogUserPlayGame
{
    public class LogUserPlayGameDao : ILogUserPlayGameDao
    {
        public LogUserPlayGameModel GetRequestLog(int userId)
        {
            if (!MongoFactory.MongoEnable)
                return new LogUserPlayGameModel();

            var collection =
                MongoFactory.MongoBb.GetCollection<LogUserPlayGameModel>(CollectionName.LogUserPlayGameModel);

            var builder = Builders<LogUserPlayGameModel>.Filter;
            var filter = builder.Eq("UserID", userId);
            return collection.Find(filter).FirstOrDefault();
        }

        public void InsertLog(LogUserPlayGameModel data)
        {
            if (!MongoFactory.MongoEnable)
                return;

            var collection =
                MongoFactory.MongoBb.GetCollection<LogUserPlayGameModel>(CollectionName.LogUserPlayGameModel);
            collection.InsertOne(data);
        }

        public void UpdateRequestLogGames(int userId, List<LogUserPlayGameGamesModel> listGames)
        {
            if (!MongoFactory.MongoEnable)
                return;

            var collection =
                MongoFactory.MongoBb.GetCollection<LogUserPlayGameModel>(CollectionName.LogUserPlayGameModel);

            var builder = Builders<LogUserPlayGameModel>.Filter;
            var filter = builder.Eq("UserID", userId);
            var update = Builders<LogUserPlayGameModel>.Update.Set(c => c.ListGames, listGames);
            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = false }
                );
            }
            catch (Exception ex)
            {
            }
        }
    }
}