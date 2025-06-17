using System.Collections.Generic;

namespace DataAccessMongo.Model.UserCollections
{
    public class E1805_User_AchievementReceive
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Star { get; set; }
        public string Image { get; set; }
        public decimal NumberCurrent { get; set; }
        public decimal NumberRequired { get; set; }
        public string CheckSum { get; set; }
        public List<E1805_User_Achievement_AwardReceive> Awards { get; set; }
    }

    public class E1805_User_Achievement_AwardReceive
    {
        public int ItemID { get; set; }
        public int Type { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
        public string CheckSum { get; set; }
    }
}