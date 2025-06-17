using System;

namespace DataAccessRedis.Dao.EventDao
{
    public interface IE1810_FreeSpin
    {
        bool GetUserFreeSpin(int userID, int gameId);

        void PubFreeSpin(int userID, int gameId);
    }

    internal class E1810_FreeSpin : IE1810_FreeSpin
    {
        public bool GetUserFreeSpin(int userID, int gameId)
        {
            var strKeyFreespin = string.Format("{0}-{1}{2}", "freespin", gameId, userID);
            var objRedisDataFreespin = RedisFactory.MyRedis.Get(strKeyFreespin);

            var strKeyWbfreespin = string.Format("{0}-{1}{2}", "wbfreespin", gameId, userID);
            var objRedisDataWbfreespin = RedisFactory.MyRedis.Get(strKeyWbfreespin);

            return string.IsNullOrEmpty(objRedisDataFreespin) && string.IsNullOrEmpty(objRedisDataWbfreespin);
        }

        public void PubFreeSpin(int userID, int gameId)
        {
            var channel = string.Format("_pub_from_{0}_chan_{1}", DateTime.Now.Ticks, "Game.WebTangFree");
            var Message = string.Format("{0}|{1}", gameId, userID);

            RedisFactory.MyRedis.PublishMessage(channel, Message);
        }
    }
}