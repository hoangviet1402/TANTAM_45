using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace DataAccessRedis.Model
{
    public class LogShootBossRedisModel
    {
        public int UserId { get; set; }

        public long Money { get; set; }
    }


    public class LogShootFishRedisModel
    {
        [Description("")]
        [JsonProperty("key")]
        public string RedisKey { get; set; }


        [Description("")]
        [JsonProperty("value")]
        public string RedisValue { get; set; }
    }

    public class B_TBossFish_Player
    {
        public int BossDetailId { get; set; }
        public int BossId { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public int UserScore { get; set; }
        public bool IsKillFish { get; set; }
        public int Award { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class B_TBossFish_PlayerAward
    {
        public int BossDetailId { get; set; }
        public int BossId { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public int UserScore { get; set; }
        public bool IsGetAward { get; set; }
        public int Award { get; set; }
        public int Rank { get; set; }
    }

    public class B_TBossFish_Redis
    {
        public int Id { get; set; }
        public int BossId { get; set; }
        public decimal Reward { get; set; }
        public int Ratio1 { get; set; }
        public int Ratio2 { get; set; }
        public int Ratio3 { get; set; }
        public int Ratio4 { get; set; }
        public int KillRatio { get; set; }
        public int KillingUserId { get; set; }
        public decimal CurrentBlood { get; set; }
        public bool IsEnable { get; set; }
        public bool IsDead { get; set; }
        public bool IsSentAward { get; set; }

        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
    }
}