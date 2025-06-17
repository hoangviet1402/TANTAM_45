using System.Collections.Generic;

namespace DataAccessMongo.Model.AchievementCollections
{
    public class CompletedAchievementInfoModel
    {
        public string Image { get; set; }


        public int Type { get; set; }
        public int GameId { get; set; }
        public int Star { get; set; }
        public int SlotType { get; set; }

        public string NumberRequired { get; set; }
        public string Description { get; set; }
        public string Checksum { get; set; }
        public List<CompletedAchievementAwardModel> ListAward { get; set; }
    }


    public class CompletedAchievementAwardModel
    {
        public int AwardType { get; set; }
        public decimal AwardValue { get; set; }
        public string Image { get; set; }
        public string Checksum { get; set; }
    }
}