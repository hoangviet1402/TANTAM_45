using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessMongo.Base;
using DataAccessMongo.Enum;
using DataAccessMongo.Model.UserShowAward;
using MongoDB.Driver;

namespace DataAccessMongo.Module.UserShowAward
{
    public class UserShowAward : IUserShowAward
    {
        public bool UserShowAwardInsert(UserShowAwardMongoModel data)
        {
            if (!MongoFactory.MongoEnable)
                return true;

            var collection = MongoFactory.MongoBb.GetCollection<UserShowAwardMongoModel>(CollectionName.UserShowAward);
            collection.InsertOne(data);
            return true;
        }

        public List<UserShowAwardMongoModel> UserShowAwardShow(int userId, ref long outResult)
        {
            outResult = 0;

            if (!MongoFactory.MongoEnable)
                return new List<UserShowAwardMongoModel>();

            var collection = MongoFactory.MongoBb.GetCollection<UserShowAwardMongoModel>(CollectionName.UserShowAward);

            var builder = Builders<UserShowAwardMongoModel>.Filter;
            var filter = builder.And(
                builder.Eq("UserId", userId),
                builder.Eq("IsShow", false)
            );
            var data = collection.Find(filter).ToList();

            if (data != null && data.Any())
            {
                var update = Builders<UserShowAwardMongoModel>.Update.Set("IsShow", true).CurrentDate("LastModified");
                try
                {
                    var result = collection.UpdateMany(
                        filter,
                        update
                    );
                    outResult = result.IsAcknowledged ? result.ModifiedCount : 0;
                }
                catch (Exception ex)
                {
                    outResult = -1;
                }
            }

            return data;
        }

        public List<UserShowAwardMongoModel> UserShowAwardShow(int userId, int type, ref long outResult)
        {
            if (!MongoFactory.MongoEnable)
                return new List<UserShowAwardMongoModel>();

            var collection = MongoFactory.MongoBb.GetCollection<UserShowAwardMongoModel>(CollectionName.UserShowAward);

            var builder = Builders<UserShowAwardMongoModel>.Filter;
            var filter = builder.And(
                builder.Eq("UserId", userId),
                builder.Eq("IsShow", false),
                builder.Eq("Type", type)
            );
            var data = collection.Find(filter).ToList();

            if (data != null && data.Any())
            {
                var update = Builders<UserShowAwardMongoModel>.Update.Set("IsShow", true).CurrentDate("LastModified");
                try
                {
                    var result = collection.UpdateMany(
                        filter,
                        update
                    );
                    outResult = result.IsAcknowledged ? result.ModifiedCount : 0;
                }
                catch (Exception ex)
                {
                    outResult = -1;
                }
            }

            return data;
        }

        public List<UserShowAwardMongoModel> UserShowAwardShowNotDefault(int userId, ref long outResult)
        {
            outResult = 0;

            if (!MongoFactory.MongoEnable)
                return new List<UserShowAwardMongoModel>();

            var collection = MongoFactory.MongoBb.GetCollection<UserShowAwardMongoModel>(CollectionName.UserShowAward);

            var builder = Builders<UserShowAwardMongoModel>.Filter;
            var filter = builder.And(
                builder.Eq("UserId", userId),
                builder.Eq("IsShow", false),
                builder.Ne("Type", 0)
            );
            var data = collection.Find(filter).ToList();

            if (data != null && data.Any())
            {
                var update = Builders<UserShowAwardMongoModel>.Update.Set("IsShow", true).CurrentDate("LastModified");
                try
                {
                    var result = collection.UpdateMany(
                        filter,
                        update
                    );
                    outResult = result.IsAcknowledged ? result.ModifiedCount : 0;
                }
                catch (Exception ex)
                {
                    outResult = -1;
                }
            }

            return data;
        }
    }
}