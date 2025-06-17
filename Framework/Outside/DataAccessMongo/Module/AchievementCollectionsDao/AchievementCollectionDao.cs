using System.Collections.Generic;
using System.Linq;
using DataAccessMongo.Base;
using DataAccessMongo.Enum;
using DataAccessMongo.Model.AchievementCollections;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DataAccessMongo.Module.AchievementCollectionsDao
{
    public interface IAchievementCollectionDao
    {
        AchievementConfig AchievementConfigByID(int Id);
        List<AchievementConfig> AchievementConfigSearch(int id, int gameId, int type, int slotType);
        List<AchievementConfig> AchievementConfigGetAll();
    }

    internal class AchievementCollectionDao : IAchievementCollectionDao
    {
        public List<AchievementConfig> AchievementConfigSearch(int id, int gameId, int type, int slotType)
        {
            if (!MongoFactory.MongoEnable)
                return new List<AchievementConfig>();

            var collection = MongoFactory.MongoBb.GetCollection<AchievementConfig>(CollectionName.AchievementConfig);
            var builder = Builders<AchievementConfig>.Filter;
            var filter = builder.And(
                id != -1 ? builder.Eq("ID", id) : Builders<AchievementConfig>.Filter.Empty,
                gameId != -1 ? builder.Eq("GameId", gameId) : Builders<AchievementConfig>.Filter.Empty,
                type != -1 ? builder.Eq("Type", type) : Builders<AchievementConfig>.Filter.Empty,
                slotType != -1 ? builder.Eq("SlotType", slotType) : Builders<AchievementConfig>.Filter.Empty,
                builder.Eq("IsActive", true)
            );
            return collection.Find(filter).ToListAsync().Result;
        }

        public AchievementConfig AchievementConfigByID(int Id)
        {
            if (!MongoFactory.MongoEnable)
                return new AchievementConfig();

            var collection = MongoFactory.MongoBb.GetCollection<BsonDocument>(CollectionName.AchievementConfig);
            var filter = Builders<BsonDocument>.Filter.Eq("ID", Id); //Builders<BsonDocument>.Filter.Empty; 
            var data = collection.Find(filter).FirstOrDefault();
            var result = BsonSerializer.Deserialize<AchievementConfig>(data);
            return result;
        }

        public List<AchievementConfig> AchievementConfigGetAll()
        {
            if (!MongoFactory.MongoEnable)
                return new List<AchievementConfig>();

            var collection = MongoFactory.MongoBb.GetCollection<AchievementConfig>(CollectionName.AchievementConfig);
            var filter = Builders<AchievementConfig>.Filter.Eq("IsActive", true);
            var data = collection.Find(filter).ToList();
            return data.Where(x => x.IsActive == true).ToList();
        }
    }
}