using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessMongo.Model.AchievementCollections
{
    public class AchievementConfig
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int ID { get; set; }
        public int? Type { get; set; }
        public int? GameId { get; set; }
        public int? SlotType { get; set; }
        public int? DestinationType { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int? Position { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public List<AchievementConfigStar> ConfigStar { get; set; }
    }

    public class AchievementConfigStar
    {
        public int? StarID { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? NumberRequired { get; set; }

        public string Description { get; set; }
        public bool? Status { get; set; }
        public List<AchievementConfigStarAward> ConfigStarAward { get; set; }
    }

    public class AchievementConfigStarAward
    {
        public int? AwardType { get; set; }
        public int? AwardItemID { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? AwardValue { get; set; }

        public string Description { get; set; }
        public bool? Status { get; set; }
    }

    public class LogUserStar
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int UserID { get; set; }
        public int ConfigID { get; set; }
        public int StarID { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal NumberCurrent { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal NumberRequired { get; set; }

        public bool IsComplete { get; set; }
        public bool IsReward { get; set; }
        public bool IsShow { get; set; }
        public DateTime RewardDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class Achievement_Summary
    {
        public string Name { get; set; }
        public int Type { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Value { get; set; }

        public int ShowNode { get; set; }
        public List<object> ListGame { get; set; }
    }

    public class AchievementPercent
    {
        public int? ID { get; set; }
        public int? Type { get; set; }
        public int? StarRequired { get; set; }
        public int? StarCurrent { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Percent { get; set; }

        public int IsShowNode { get; set; }
    }

    public class Achievement_Summary_ListGame
    {
        public int? GameId { get; set; }
        public int IsShowNode { get; set; }
    }

    public class Achievement_Image
    {
        public string Url { get; set; }
        public string CheckSum { get; set; }
    }
}