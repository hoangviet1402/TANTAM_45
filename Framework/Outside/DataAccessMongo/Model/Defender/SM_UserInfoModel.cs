using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessMongo.Model.Defender
{
    [BsonIgnoreExtraElements]
    public class SM_UserInfoModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int UserID { get; set; }
        public DateTime LastLogin { get; set; }
        public int Status { get; set; }
        public List<UserGunInfo> ListGun { get; set; }

        [BsonIgnoreIfNull] public List<UserTowerInfo> ListTower { get; set; }

        [BsonIgnoreIfNull] public List<UserMineralInfo> ListMineral { get; set; }

        [BsonIgnoreIfNull] public List<UserItemInfo> ListItem { get; set; }

        [BsonIgnoreIfNull] public List<UserSkillInfo> ListSkill { get; set; }

        [BsonIgnoreIfNull] public List<UserMaterialInfo> ListMaterial { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserItemInfo
    {
        [BsonIgnoreIfNull] public int? ItemID { get; set; }

        [BsonDefaultValue(0)] public int? Quantity { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserGunInfo
    {
        public int GunLevel { get; set; }

        [BsonDefaultValue(0)] public int Exp { get; set; }

        public int GunKind { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

        //[BsonDefaultValue(1)]
        //public int Generation { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserTowerInfo
    {
        public int TowerLevel { get; set; }

        [BsonDefaultValue(0)] public int Exp { get; set; }

        public int TowerKind { get; set; }
        public bool IsActive { get; set; }

        //[BsonDefaultValue(1)]
        //public int Generation { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserMineralInfo
    {
        [BsonIgnoreIfNull] public int MineralID { get; set; }

        [BsonIgnoreIfNull] public int Quantity { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserMaterialInfo
    {
        [BsonIgnoreIfNull] public int Element { get; set; }

        [BsonIgnoreIfNull] public int Quantity { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserSkillInfo
    {
        public int SkillKind { get; set; }
        public int SkillLevel { get; set; }
        public bool IsActive { get; set; }
    }
}