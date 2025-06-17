using System;
using Logger;
using Newtonsoft.Json;

namespace DataAccessRedis.Dao.ServerGame
{
    public interface IServerGame
    {
        int GetRoomId(int userID, int game);
        void KickAllGame(int userID);
        void KickDino(int userID);
        void KickBanCa(int userID);
        void KickDaoVang(int userID);
        void ForceKick(int userID, int gameID);
    }

    internal class ServerGame : IServerGame
    {
        public int GetRoomId(int userID, int game)
        {
            var roomid = 0;

            try
            {
                var strKey = string.Format("user-play-server-{0}-{1}", game, userID);
                var value = RedisFactory.MyRedis.Get(strKey);

                if (!string.IsNullOrEmpty(value))
                    int.TryParse(value, out roomid);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("DataAccessRedis ServerGame | GetRoomId: {0}", ex);
                roomid = 0;
            }

            return roomid;
        }

        public void KickAllGame(int userID)
        {
            var channel = string.Format("_pub_from_{0}_chan_{1}", DateTime.Now.Ticks, "User.Login");
            var message = string.Format("{0}", userID);
            var message2 = string.Format("{0}|{1}|{2}", userID, "web", "80");
            var messageBC = JsonConvert.SerializeObject(new { UserID = userID });

            RedisFactory.MyRedis.PublishMessage(channel, message);
            RedisFactory.MyRedis.PublishMessage(channel, message2);
            RedisFactory.MyRedis.PublishMessage("BC_K", messageBC);
        }

        public void KickDino(int userID)
        {
            RedisFactory.MyRedis.PublishMessage("Admin.KickUser2", string.Format("{0}|{1}|{2}|{3}", 508, 0, 0, userID));
        }

        public void KickBanCa(int userID)
        {
            var message = JsonConvert.SerializeObject(new { UserID = userID });
            RedisFactory.MyRedis.PublishMessage("BC_K", message);
        }

        public void KickDaoVang(int userID)
        {
            //RedisFactory.MyRedis.PublishMessage("Admin.DaoVangKickLobby", string.Format("{0}|{1}|{2}|{3}", 457, 0, 0, userID));
            //RedisFactory.MyRedis.PublishMessage("_pub_from_636698597016562186_chan_User.Login", userID.ToString());
            //RedisFactory.MyRedis.PublishMessage("_pub_from_636698597016562186_chan_User.Login", string.Format("{0}|{1}|{2}", userID, "web", "80"));
        }

        public void ForceKick(int userID, int gameID)
        {
            RedisFactory.MyRedis.PublishMessage(
                string.Format("_pub_from_{0}_chan_Force.Kick.Master", DateTime.Now.Ticks),
                string.Format("{0}|{1}", userID, gameID));
        }
    }
}