using System;
using System.Collections.Generic;
using DataAccessMongo.Base;
using DataAccessMongo.Enum;
using DataAccessMongo.Model.UserLoginLogCollection;
using MongoDB.Driver;

namespace DataAccessMongo.Module.UserLoginLogCollection
{
    public class UserLoginLog : IUserLoginLog
    {
        /// <summary>
        ///     Author: CuongPK
        ///     CreateDate: 12/09/2018
        ///     Description: Insert Log Đăng nhập của user
        /// </summary>
        /// <param name="logModel"></param>
        public void InsertLoginLog(UserLoginLogModel logModel)
        {
            if (!MongoFactory.MongoEnable)
                return;

            var collection = GetCollection();
            collection.InsertOne(logModel);
        }

        /// <summary>
        ///     Author: CuongPK
        ///     CreateDate: 12/09/2018
        ///     Description: Lấy Log Đăng nhập của user
        /// </summary>
        /// <returns></returns>
        public List<UserLoginLogModel> GetDataUserLog(DateTime fromDate, DateTime toDate, bool isPaging, int pageIndex,
            int pageSize)
        {
            if (!MongoFactory.MongoEnable)
                return null;

            if (isPaging && (pageIndex < 0 || pageSize < 1))
                return null;

            pageIndex--;
            var collection = GetCollection();
            var builder = Builders<UserLoginLogModel>.Filter;

            var filter = builder.And(
                builder.Gte("LoginTime", fromDate),
                builder.Lte("LoginTime", toDate)
            );
            if (isPaging)
                return collection.Find(filter).Sort("{LoginTime: 1}").Skip(pageIndex * pageSize).Limit(pageSize)
                    .ToList();
            return collection.Find(filter).Sort("{LoginTime: 1}").ToList();
        }

        private IMongoCollection<UserLoginLogModel> GetCollection()
        {
            if (!MongoFactory.MongoEnable)
                return null;

            return MongoFactory.MongoBb.GetCollection<UserLoginLogModel>(CollectionName.UserLoginLog);
        }
    }
}