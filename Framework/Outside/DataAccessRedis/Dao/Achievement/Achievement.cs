using System;

namespace DataAccessRedis.Dao.Achievement
{
    public interface IAchievement
    {
        void UserCompleteAchievement(int userID, string title, string content, string image, decimal number);
    }

    internal class Achievement : IAchievement
    {
        public void UserCompleteAchievement(int userID, string title, string content, string image, decimal number)
        {
            var channel = string.Format("_pub_from_{0}_chan_{1}", DateTime.Now.Ticks, "_User.Request.Web");
            var Message = string.Format("{0}", userID);
            RedisFactory.MyRedis.PublishMessage(channel, Message);
        }


        //public void Level_LevelUp(int userID)
        //{
        //    string channel = string.Format("_pub_from_{0}_chan_{1}", DateTime.Now.Ticks, "_User.Request.Web");
        //    string Message = string.Format("{0}", userID);

        //    //Logger.CommonLogger.DefaultLogger.DebugFormat("channel: {0} -- Message: {1}", channel, Message);

        //    RedisFactory.MyRedis.PublishMessage(channel, Message);
        //}
    }
}