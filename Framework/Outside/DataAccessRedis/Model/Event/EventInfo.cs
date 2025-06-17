using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace DataAccessRedis.Model.Event
{
    public class UserEventNoHu
    {
        [Description("")]
        [JsonProperty("gid")]
        public int GameId { get; set; }

        [Description("")]
        [JsonProperty("uid")]
        public int UserId { get; set; }

        [Description("")] [JsonProperty("pc")] public DateTime PointCard { get; set; }

        [Description("")] [JsonProperty("pb")] public int PointBetting { get; set; }

        public string NickName { get; set; }
        public double Gold { get; set; }

        public int PubUserId { get; set; }
    }

    public class EventNoHuUserRankModel
    {
        /// <summary>
        ///     UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     Gold user tich duoc trong event
        /// </summary>
        public double Gold { get; set; }
    }
}