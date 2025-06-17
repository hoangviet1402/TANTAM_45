using System;
using System.Collections.Generic;
using DataAccessMongo.Base;
using DataAccessMongo.Enum;
using DataAccessMongo.Model.UserCollections;
using MongoDB.Driver;

namespace DataAccessMongo.Module.UserCollections
{
    internal class User : IUser
    {
        public List<E1805_User_Achievement> LogUserAchievementSearch(int userId, int achievementId, int type,
            int gameId, int slotType, int star)
        {
            if (!MongoFactory.MongoEnable)
                return new List<E1805_User_Achievement>();

            var collection = MongoFactory.MongoBb.GetCollection<E1805_User_Achievement>(CollectionName.LogUserStar);

            var builder = Builders<E1805_User_Achievement>.Filter;
            var filter = builder.And(
                userId != -1 ? builder.Eq("UserId", userId) : Builders<E1805_User_Achievement>.Filter.Empty,
                achievementId != -1
                    ? builder.Eq("AchievementID", achievementId)
                    : Builders<E1805_User_Achievement>.Filter.Empty,
                type != -1 ? builder.Eq("Type", type) : Builders<E1805_User_Achievement>.Filter.Empty,
                gameId != -1 ? builder.Eq("GameId", gameId) : Builders<E1805_User_Achievement>.Filter.Empty,
                slotType != -1 ? builder.Eq("SlotType", slotType) : Builders<E1805_User_Achievement>.Filter.Empty,
                star != -1 ? builder.Eq("Star", star) : Builders<E1805_User_Achievement>.Filter.Empty
            );
            return collection.Find(filter).ToList();
        }

        public List<E1805_User_Achievement> LogUserStarByUserId(int userId)
        {
            if (!MongoFactory.MongoEnable)
                return new List<E1805_User_Achievement>();

            var collection = MongoFactory.MongoBb.GetCollection<E1805_User_Achievement>(CollectionName.LogUserStar);

            var builder = Builders<E1805_User_Achievement>.Filter;
            var filter = builder.Eq("UserId", userId);
            return collection.Find(filter).ToList();
        }

        public void InsertUserAchievement(E1805_User_Achievement user)
        {
            if (!MongoFactory.MongoEnable)
                return;

            var collection = MongoFactory.MongoBb.GetCollection<E1805_User_Achievement>(CollectionName.LogUserStar);

            collection.InsertOne(user);
        }

        public long UpdateUserAchievement(int userId, int achievementId, double star, decimal number, bool isShow)
        {
            if (!MongoFactory.MongoEnable)
                return 0;

            var collection = MongoFactory.MongoBb.GetCollection<E1805_User_Achievement>(CollectionName.LogUserStar);
            var builder = Builders<E1805_User_Achievement>.Filter;
            var filter = builder.And(
                builder.Eq("UserId", userId),
                builder.Eq("AchievementID", achievementId),
                builder.Eq("Star", star)
            );
            var update = Builders<E1805_User_Achievement>.Update.Set("NumberCurrent", number).Set("IsShow", isShow)
                .CurrentDate("LastModified");
            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update
                );
                return result.IsAcknowledged ? result.ModifiedCount : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public long ReceiveUserAchievementStatus(int userId, int achievementId, double star, bool isComplete,
            decimal numberCurrent)
        {
            if (!MongoFactory.MongoEnable)
                return 0;

            var collection = MongoFactory.MongoBb.GetCollection<E1805_User_Achievement>(CollectionName.LogUserStar);
            var builder = Builders<E1805_User_Achievement>.Filter;
            var filter = builder.And(
                builder.Eq("UserId", userId),
                builder.Eq("AchievementID", achievementId),
                builder.Eq("Star", star)
            );
            var update = Builders<E1805_User_Achievement>.Update.Set("IsComplete", isComplete).Set("IsReward", true)
                .Set("NumberCurrent", numberCurrent).CurrentDate("LastModified");
            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update
                );
                return result.IsAcknowledged ? result.ModifiedCount : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<E1805_User_Achievement> FindAchievementByTypeAndGameId(int userId, int type, int gameId)
        {
            if (!MongoFactory.MongoEnable)
                return new List<E1805_User_Achievement>();

            var collection = MongoFactory.MongoBb.GetCollection<E1805_User_Achievement>(CollectionName.LogUserStar);

            var builder = Builders<E1805_User_Achievement>.Filter;
            var filter = builder.And(
                userId != -1 ? builder.Eq("UserId", userId) : Builders<E1805_User_Achievement>.Filter.Empty,
                type != -1 ? builder.Eq("Type", type) : Builders<E1805_User_Achievement>.Filter.Empty,
                gameId != -1 ? builder.Eq("GameId", gameId) : Builders<E1805_User_Achievement>.Filter.Empty
            );
            return collection.Find(filter).ToList();
        }

        public List<E1805_User_Achievement> FindAchievementByIDAndStar(int userId, int ID, int star)
        {
            if (!MongoFactory.MongoEnable)
                return new List<E1805_User_Achievement>();

            var collection = MongoFactory.MongoBb.GetCollection<E1805_User_Achievement>(CollectionName.LogUserStar);

            var builder = Builders<E1805_User_Achievement>.Filter;
            var filter = builder.And(
                builder.Eq("UserId", userId),
                builder.Eq("AchievementID", ID),
                builder.Eq("Star", star)
            );
            return collection.Find(filter).ToList();
        }

        public List<E1805_User_Achievement> FindAchievementByGame(int userId, int gameId, int slotType)
        {
            if (!MongoFactory.MongoEnable)
                return new List<E1805_User_Achievement>();

            var collection = MongoFactory.MongoBb.GetCollection<E1805_User_Achievement>(CollectionName.LogUserStar);

            var builder = Builders<E1805_User_Achievement>.Filter;
            var filter = builder.And(
                builder.Eq("UserId", userId),
                builder.Eq("GameId", gameId),
                builder.Eq("SlotType", slotType)
            );
            return collection.Find(filter).ToList();
        }
    }
}