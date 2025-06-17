using System;
using System.Collections.Generic;
using DataAccessRedis.Model;
using Newtonsoft.Json;
//using DataAccess;
//using MyConfig;

namespace DataAccessRedis.Dao.SystemManagementDao
{
    public interface IBanCaRedis
    {
        List<LogShootFishRedisModel> LoadLogShootFishRedis(DateTime dtmSelectedDate, DateTime dtmTimeBegin,
            DateTime dtmTimeEnd, string strUserId);

        List<LogShootBossRedisModel> LoadLogShootBossFishRedis(string id);
        void Kick(string key, string message);

        void KickUser(int userId, int controllerId);

        void KickUserMaster(int userId, int controllerId);

        List<B_TBossFish_Player> LogShootBoss_Redis(string strKey, out string[] keys);
        List<B_TBossFish_PlayerAward> GetTopPlayer(string strKey, out string[] keys);

        List<B_TBossFish_Redis> ListBoss_Redis(string strKey);
    }

    internal class BanCaRedis : IBanCaRedis
    {
        public List<B_TBossFish_Redis> ListBoss_Redis(string strKey)
        {
            var keys = RedisFactory.MyRedis.Scan(strKey);
            var lstBoss = new List<B_TBossFish_Redis>();
            foreach (var data in keys) // danh sách user (key)
            {
                var bossDetail_Redis = RedisFactory.MyRedis.HashGetAll(data); // nội dung
                var bossDetail = new B_TBossFish_Redis();
                foreach (var detail in bossDetail_Redis)
                {
                    var col = detail.Key.ToString();
                    var id = 0;
                    decimal money = 0;
                    var date = DateTime.Now;
                    switch (col)
                    {
                        case "Id":
                            int.TryParse(detail.Value, out id);
                            bossDetail.Id = id;
                            break;
                        case "BossId":
                            int.TryParse(detail.Value, out id);
                            bossDetail.BossId = id;
                            break;
                        case "Reward":
                            decimal.TryParse(detail.Value, out money);
                            bossDetail.Reward = money;
                            break;
                        case "Ratio1":
                            int.TryParse(detail.Value, out id);
                            bossDetail.Ratio1 = id;
                            break;
                        case "Ratio2":
                            int.TryParse(detail.Value, out id);
                            bossDetail.Ratio2 = id;
                            break;
                        case "Ratio3":
                            int.TryParse(detail.Value, out id);
                            bossDetail.Ratio3 = id;
                            break;
                        case "Ratio4":
                            int.TryParse(detail.Value, out id);
                            bossDetail.Ratio4 = id;
                            break;
                        case "CurrentBlood":
                            decimal.TryParse(detail.Value, out money);
                            bossDetail.CurrentBlood = money;
                            break;
                        case "IsEnable":
                            bossDetail.IsEnable = detail.Value.Equals("true");
                            break;
                        case "IsDead":
                            bossDetail.IsDead = detail.Value.Equals("true");
                            break;
                        case "IsSentAward":
                            bossDetail.IsSentAward = detail.Value.Equals("true");
                            break;
                        case "KillingUserId":
                            int.TryParse(detail.Value, out id);
                            bossDetail.KillingUserId = id;
                            break;
                        case "KillRatio":
                            int.TryParse(detail.Value, out id);
                            bossDetail.KillRatio = id;
                            break;
                        case "TimeEnd":
                            if (DateTime.TryParse(detail.Value, out date)) bossDetail.TimeEnd = date;
                            break;
                        case "TimeStart":
                            if (DateTime.TryParse(detail.Value, out date)) bossDetail.TimeStart = date;
                            break;
                    }
                }

                lstBoss.Add(bossDetail);
            }

            return lstBoss;
        }

        public List<B_TBossFish_Player> LogShootBoss_Redis(string strKey, out string[] keys)
        {
            keys = RedisFactory.MyRedis.Scan(strKey);
            var lstData = new List<B_TBossFish_Player>();

            foreach (var data in keys) // danh sách user (key)
            {
                // danh sách key - value trong key
                var userShootBossDetail = RedisFactory.MyRedis.HashGetAll(data); // nội dung
                var tBossFishPlayer = new B_TBossFish_Player();
                // tách key
                char[] delimiterChars = { ':' };
                var key = data.Split(delimiterChars);
                // dữ liệu mặc định 
                var bossDetailId = 0;
                int.TryParse(key[1], out bossDetailId);
                tBossFishPlayer.BossDetailId = bossDetailId;
                tBossFishPlayer.BossId = 0;
                tBossFishPlayer.Award = 0;
                tBossFishPlayer.CreateDate = DateTime.Now;
                tBossFishPlayer.UpdateDate = DateTime.Now;
                tBossFishPlayer.IsKillFish = false;
                // dữ liệu redis
                foreach (var detail in userShootBossDetail)
                {
                    var col = detail.Key.ToString();
                    var id = 0;
                    var date = DateTime.Now;
                    switch (col)
                    {
                        case "UserId":
                            int.TryParse(detail.Value, out id);
                            tBossFishPlayer.UserId = id;
                            break;
                        case "DisplayName":
                            tBossFishPlayer.DisplayName = detail.Value;
                            break;
                        case "Score":
                            int.TryParse(detail.Value, out id);
                            tBossFishPlayer.UserScore = id;
                            break;
                        case "UpdateDate":
                            // Thu Jan 19 2017 22:37:20 GMT+0700 (SE Asia Standard Time)
                            string[] delimiterCharsDate = { "GMT" };
                            var stringDate = detail.Value.ToString()
                                .Split(delimiterCharsDate, StringSplitOptions.RemoveEmptyEntries);
                            if (DateTime.TryParse(stringDate[0], out date))
                            {
                                tBossFishPlayer.UpdateDate = date;
                                tBossFishPlayer.CreateDate = date;
                            }

                            break;
                    }
                }

                lstData.Add(tBossFishPlayer);
            }

            return lstData;
        }

        public List<B_TBossFish_PlayerAward> GetTopPlayer(string strKey, out string[] keys)
        {
            keys = RedisFactory.MyRedis.Scan(strKey);
            var lstData = new List<B_TBossFish_PlayerAward>();

            foreach (var data in keys) // danh sách user (key)
            {
                // danh sách key - value trong key
                var userShootBossDetail = RedisFactory.MyRedis.HashGetAll(data); // nội dung
                var tBossFishPlayer = new B_TBossFish_PlayerAward();
                // tách key
                char[] delimiterChars = { ':' };
                var key = data.Split(delimiterChars);
                // dữ liệu mặc định 
                var bossDetailId = 0;
                int.TryParse(key[1], out bossDetailId);
                tBossFishPlayer.BossDetailId = bossDetailId;
                tBossFishPlayer.BossId = 0;
                tBossFishPlayer.Award = 0;
                // dữ liệu redis
                foreach (var detail in userShootBossDetail)
                {
                    var col = detail.Key.ToString();
                    var id = 0;
                    var isBool = false;
                    switch (col)
                    {
                        case "UserId":
                            int.TryParse(detail.Value, out id);
                            tBossFishPlayer.UserId = id;
                            break;
                        case "DisplayName":
                            tBossFishPlayer.DisplayName = detail.Value;
                            break;
                        case "IsGetAward":
                            bool.TryParse(detail.Value.ToString().ToLower(), out isBool);
                            tBossFishPlayer.IsGetAward = isBool;
                            break;
                        case "Score":
                            int.TryParse(detail.Value, out id);
                            tBossFishPlayer.UserScore = id;
                            break;
                        case "Rank":
                            int.TryParse(detail.Value, out id);
                            tBossFishPlayer.Rank = id;
                            break;
                        case "Award":
                            int.TryParse(detail.Value, out id);
                            tBossFishPlayer.Award = id;
                            break;
                    }
                }

                lstData.Add(tBossFishPlayer);
            }

            return lstData;
        }

        public List<LogShootFishRedisModel> LoadLogShootFishRedis(DateTime dtmSelectedDate, DateTime dtmTimeBegin,
            DateTime dtmTimeEnd, string strUserId)
        {
            var intYear = dtmSelectedDate.Year;
            var intMonth = dtmSelectedDate.Month;
            var intDay = dtmSelectedDate.Day;
            // tim tat cac key:  lg: Năm : Tháng : Ngày : UserID
            //string strKey = "lg:" + intYear + ":" + (intMonth >= 10 ? intMonth.ToString() : "0" + intMonth) + ":" + (intDay >= 10 ? intDay.ToString() : "0" + intDay) + ":" + strUserId;
            var strKey = string.Format("lg:{0:0000}:{1:00}:{2:00}:{3}", intYear, intMonth, intDay, strUserId);
            //CommonLogger.DefaultLogger.DebugFormat("LoadLogShootFishRedis | strKey: {0}", strKey);
            // Lay du lieu Redis
            var lstData = new List<LogShootFishRedisModel>();
            var objRedisData = RedisFactory.MyRedis.HashGetAll(strKey);
            //CommonLogger.DefaultLogger.DebugFormat("LoadLogShootFishRedis | objRedisData: {0}", JsonConvert.SerializeObject(objRedisData));
            foreach (var data in objRedisData)
                lstData.Add(new LogShootFishRedisModel
                {
                    RedisKey = data.Key,
                    RedisValue = data.Value
                });
            return lstData;
        }

        public List<LogShootBossRedisModel> LoadLogShootBossFishRedis(string id)
        {
            var strKey = string.Format("BU:{0}", id);

            // Lay du lieu Redis
            var lstData = new List<LogShootBossRedisModel>();

            var objRedisData = RedisFactory.MyRedis.HashGetAll(strKey);
            //CommonLogger.DefaultLogger.DebugFormat("LoadLogShootFishRedis | objRedisData: {0}", JsonConvert.SerializeObject(objRedisData.Keys));

            foreach (var data in objRedisData)
            {
                int userId;
                long money;

                if (int.TryParse(data.Key, out userId) && long.TryParse(data.Value, out money))
                    lstData.Add(new LogShootBossRedisModel
                    {
                        UserId = userId,
                        Money = money
                    });
            }

            return lstData;
        }

        public void Kick(string key, string message)
        {
            RedisFactory.MyRedis.PublishMessage(key, message);
        }

        public void KickUser(int userId, int controllerId)
        {
            // RedisFactory.MyRedis.PublishMessage(key, message);
            var message = JsonConvert.SerializeObject(new { UserID = userId });
            RedisFactory.MyRedis.PublishMessage("BC_K", message);
        }

        public void KickUserMaster(int userId, int controllerId)
        {
            // RedisFactory.MyRedis.PublishMessage(key, message);
            var message = JsonConvert.SerializeObject(new { UserID = userId });
            RedisFactory.MyRedis.PublishMessage("BCM_K", message);
        }

        public void GetUser(string key, string message)
        {
            RedisFactory.MyRedis.PublishMessage(key, message);
        }
    }
}