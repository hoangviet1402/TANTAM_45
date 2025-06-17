using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessMongo.Model.LogUserPlayGame
{
    public class LogUserPlayGameModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int UserID { get; set; }

        public DateTime LastUpdate { get; set; }

        public List<LogUserPlayGameGamesModel> ListGames { get; set; }
    }

    public class LogUserPlayGameGamesModel
    {
        public int GameId { get; set; }

        public int TotalPlay { get; set; }

        public decimal? TotalWin { get; set; }

        public decimal? TotalLose { get; set; }
    }
}