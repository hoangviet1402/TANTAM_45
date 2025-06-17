using System;
using DataAccessMongo.Base;
using DataAccessMongo.Enum;
using DataAccessMongo.Model.E2003_LogAllEventStatusCollection;

namespace DataAccessMongo.Module.E2003_LogAllEventStatusCollection
{
    public class E2003LogAllEventStatus : IE2003LogAllEventStatus
    {
        public void LogAllEventStatus_Insert(E2003_LogAllEventStatusMongoModel e2003_LogAllEventStatusModel)
        {
            if (!MongoFactory.MongoEnable)
                return;

            var collection =
                MongoFactory.MongoBb.GetCollection<E2003_LogAllEventStatusMongoModel>(CollectionName
                    .E2003_LogAllEventStatus);

            //var createDate = e2003_LogAllEventStatusModel.CreateDate.ToUniversalTime();

            //var builder = Builders<E2003_LogAllEventStatusMongoModel>.Filter;
            //var filter = builder.And(
            //    builder.Eq(x => x.UserId, e2003_LogAllEventStatusModel.UserId),
            //    builder.Eq(x => x.Status, e2003_LogAllEventStatusModel.Status),
            //    builder.Eq(x => x.ClientIp, e2003_LogAllEventStatusModel.ClientIp),
            //    builder.Eq(x => x.DeviceId, e2003_LogAllEventStatusModel.DeviceId),
            //    builder.Eq(x => x.PlatFormId, e2003_LogAllEventStatusModel.PlatFormId),
            //    builder.Eq(x => x.EventPromotionID, e2003_LogAllEventStatusModel.EventPromotionID),
            //    builder.Eq(x => x.Date, e2003_LogAllEventStatusModel.Date)
            //    );

            //var data = collection.Find(filter).FirstOrDefault();
            //if (data == null)            
            collection.InsertOneAsync(e2003_LogAllEventStatusModel);
        }

        private DateTime ConvertDateTimeToDate(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }
    }
}