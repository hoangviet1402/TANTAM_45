using System;
using DataAccessMongo.Base;
using DataAccessMongo.Enum;
using DataAccessMongo.Model.RequestLogCollection;

namespace DataAccessMongo.Module.RequestLogCollection
{
    public class RequestLog : IRequestLog
    {
        public void InsertRequestLog(string ip, string domain, string referer, dynamic param)
        {
            if (!MongoFactory.MongoEnable)
                return;

            var model = new RequestLogModel
            {
                Ip = ip,
                Domain = domain,
                Referer = referer,
                Param = param,
                CreateDate = DateTime.Now
            };
            var collection = MongoFactory.MongoBb.GetCollection<RequestLogModel>(CollectionName.RequestLog);

            collection.InsertOne(model);
        }
    }
}