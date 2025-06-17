using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessMongo.Model.UserShowAward
{
    public class UserShowAwardMongoModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int UserId { get; set; }
        public string DataConfig { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LastModified { get; set; }

        public bool IsShow { get; set; }
        public int Type { get; set; }
    }
}