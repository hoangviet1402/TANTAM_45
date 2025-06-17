using System;

namespace DataAccessRedis.Dao.Minigame
{
    public interface IJackPot
    {
        void BroadCastMessageWithCoinAllServer(string nickName, string jackpotType, string winCoin, int repeatTime);
    }

    internal class JackPot : IJackPot
    {
        public void BroadCastMessageWithCoinAllServer(string nickName, string jackpotType, string winCoin,
            int repeatTime)
        {
            var message = string.Format("Chúc mừng {0} trúng {1} Jackpot giá trị {2} Xu", nickName, jackpotType,
                winCoin);
            var info = string.Format("{0}|{1}|{2}|{3}", message, repeatTime, message, winCoin);

            var chanel = string.Format("_pub_from_{0}_chan_{1}", DateTime.Now.Ticks, "Server.BroadcastAllServer");
            RedisFactory.MyRedis.PublishMessage(chanel, info);
        }
    }
}