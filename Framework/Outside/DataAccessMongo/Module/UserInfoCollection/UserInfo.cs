using System;
using System.Collections.Generic;
using DataAccessMongo.Base;
using DataAccessMongo.Enum;
using DataAccessMongo.Model.UserInfoCollection;
using MongoDB.Driver;

namespace DataAccessMongo.Module.UserInfoCollection
{
    internal class UserInfo : IUserInfo
    {
        public List<UserInfoModel> GetTopCurrentCoin(int top)
        {
            if (!MongoFactory.MongoEnable)
                return new List<UserInfoModel>();

            var collection = MongoFactory.MongoBb.GetCollection<UserInfoModel>(CollectionName.UserInfo);

            var builder = Builders<UserInfoModel>.Filter;
            var filter = builder.Exists("FishingInfo");
            return collection.Find(filter).Sort(Builders<UserInfoModel>.Sort.Descending("FishingInfo.CurrentCoin"))
                .Limit(top).ToList(); // -1 decs, 1 asc
        }

        public List<UserInfoModel> GetTopUser(int top)
        {
            if (!MongoFactory.MongoEnable)
                return new List<UserInfoModel>();

            var collection = MongoFactory.MongoBb.GetCollection<UserInfoModel>(CollectionName.UserInfo);

            var builder = Builders<UserInfoModel>.Filter;
            var filter = Builders<UserInfoModel>.Filter.Empty;
            return collection.Find(filter).Sort(Builders<UserInfoModel>.Sort.Descending("Achi.Summary.TotalPoint"))
                .Limit(top).ToList(); // -1 decs, 1 asc
        }

        public List<UserInfoModel> GetByUserId(int userId)
        {
            if (!MongoFactory.MongoEnable)
                return new List<UserInfoModel>();

            var collection = MongoFactory.MongoBb.GetCollection<UserInfoModel>(CollectionName.UserInfo);

            var builder = Builders<UserInfoModel>.Filter;
            var filter = builder.Eq("UserID", userId);
            return collection.Find(filter).ToList();
        }

        public List<UserInfoModel> GetByUserIdListId(List<int> listUserId)
        {
            if (!MongoFactory.MongoEnable)
                return new List<UserInfoModel>();

            var collection = MongoFactory.MongoBb.GetCollection<UserInfoModel>(CollectionName.UserInfo);

            var builder = Builders<UserInfoModel>.Filter;
            var filter = builder.In("UserID", listUserId);
            return collection.Find(filter).ToList();
        }

        public long UpdateUserAchievementComplete(int userId, decimal totalPoint, decimal totalQuantity,
            decimal totalGold, decimal totalReceivedQuantity, decimal totalReceivedGold)
        {
            if (!MongoFactory.MongoEnable)
                return 0;

            var collection = MongoFactory.MongoBb.GetCollection<UserInfoModel>(CollectionName.UserInfo);
            var builder = Builders<UserInfoModel>.Filter;
            var filter = builder.And(
                builder.Eq("UserID", userId)
            );
            var update = Builders<UserInfoModel>.Update.Set("Achi.Summary.TotalPoint", totalPoint)
                .Set("Achi.Summary.TotalQuantity", totalQuantity).Set("Achi.Summary.TotalGold", totalGold)
                .Set("Achi.Summary.TotalReceivedQuantity", totalReceivedQuantity)
                .Set("Achi.Summary.TotalReceivedGold", totalReceivedGold).CurrentDate("LastModified");
            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = true }
                );
                return result.IsAcknowledged ? result.ModifiedCount : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}