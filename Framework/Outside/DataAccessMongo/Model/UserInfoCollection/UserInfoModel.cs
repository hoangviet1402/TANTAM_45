using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessMongo.Model.UserInfoCollection
{
    public class UserInfoModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int UserID { get; set; }
        public UserInfo_AchiModel Achi { get; set; }
        public UserInfo_FishingInfoModel FishingInfo { get; set; }
        public DateTime? LastModified { get; set; }
    }

    public class UserInfo_FishingInfoModel
    {
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal CurrentCoin { get; set; }

        [BsonRepresentation(BsonType.Int32, AllowTruncation = true)]
        public int RoomID { get; set; }

        public DateTime UpdateTime { get; set; }

        [BsonRepresentation(BsonType.Int32, AllowTruncation = true)]
        public int Status { get; set; }
    }

    public class UserInfo_AchiModel
    {
        public UserInfo_Achi_SummaryModel Summary { get; set; }
    }

    public class UserInfo_Achi_SummaryModel
    {
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalPoint { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalQuantity { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalGold { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalReceivedQuantity { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalReceivedGold { get; set; }
    }
}