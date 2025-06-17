using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessMongo.Model.UserCollections
{
    public class E1805_User_Achievement
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int AchievementID { get; set; }
        public string AchiName { get; set; }
        public string AchiDescription { get; set; }

        public int Type { get; set; }
        public int GameId { get; set; }
        public int Star { get; set; }
        public int SlotType { get; set; }
        public int DestinationType { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal NumberCurrent { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal NumberRequired { get; set; }

        public bool IsComplete { get; set; }
        public bool IsReward { get; set; }
        public bool IsShow { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModified { get; set; }

        public int UserId { get; set; }
        public List<E1805_User_Achievement_Award> Awards { get; set; }
    }

    public class E1805_User_Achievement_Award
    {
        public int AwardItemID { get; set; }
        public int AwardType { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal AwardValue { get; set; }

        public string AwardDescription { get; set; }
    }
}