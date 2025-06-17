using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessMongo.Model.UserLoginLogCollection
{
    public class UserLoginLogModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int UserId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LoginTime { get; set; }

        public string Device { get; set; }
        public string Platform { get; set; }
        public string ClientIP { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Current_Coin { get; set; }
    }
}