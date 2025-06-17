using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessMongo.Model.E2003_LogAllEventStatusCollection
{
    public class E2003_LogAllEventStatusMongoModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public int EventPromotionID { get; set; }
        public decimal CurrentGold { get; set; }
        public int GoldBonus { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string ClientIp { get; set; }
        public string DeviceId { get; set; }
        public string ClientAgent { get; set; }
        public int ClientTarget { get; set; }
        public int PlatFormId { get; set; }
        public string ConfigData { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDate { get; set; }

        public string Date { get; set; }
    }
}