using System;
using Logger;

namespace DataAccessRedis.Dao.EventDao
{
    public interface IE1806_CardSpin
    {
        bool PubTotalCoinReward(int userID, decimal coin);
    }

    internal class E1806_CardSpin : IE1806_CardSpin
    {
        public bool PubTotalCoinReward(int userID, decimal coin)
        {
            try
            {
                var channel = string.Format("_pub_from_{0}_chan_{1}", DateTime.Now.Ticks, "Game.VQXS");
                var Message = string.Format("{0}|{1}", userID, coin);

                RedisFactory.MyRedis.PublishMessage(channel, Message);

                return true;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("DataAccessRedis.Dao.EventDao.E1806_CardSpin ", ex);
                return false;
            }
        }
    }
}