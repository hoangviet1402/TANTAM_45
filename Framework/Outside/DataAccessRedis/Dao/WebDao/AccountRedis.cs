using System;
using System.Collections.Generic;
using DataAccessRedis.Constant;
using DataAccessRedis.Model;
//using DataAccess;
//using MyConfig;

namespace DataAccessRedis.Dao.WebDao
{
    public interface IAccountRedis
    {
        List<AssociationMember_ListUserNewOrOld_Redis> GetListUserNewOrOld(int associationId, int top, int type,
            DateTime dtmClearCacheRedis);
        //RedisStatusCode SetListUserNewOrOld(int associationId, int top, int type, DateTime dtmClearCacheRedis);

        //List<Account_GetBirthdayUsers_Redis> GetSinhNhatVip(int pageLength, DateTime dtmClearCacheRedis);

        //RedisStatusCode SetSinhNhatVip(int pageLength, DateTime dtmClearCacheRedis);

        void Level_LevelUp(int userID);
    }

    internal class AccountRedis : IAccountRedis
    {
        #region user chua tham gia Bang

        public List<AssociationMember_ListUserNewOrOld_Redis> GetListUserNewOrOld(int associationId, int top, int type,
            DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetListUserNewOrOld + ":" + associationId + ":" + top + ":" + type;
            var lstData = new List<AssociationMember_ListUserNewOrOld_Redis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<AssociationMember_ListUserNewOrOld_Redis>>(objRedisData);

            //temp delete
            //if (lstData == null || !lstData.Any())
            //    SetListUserNewOrOld(associationId, top, type, dtmClearCacheRedis);

            return lstData;
        }

        //public RedisStatusCode SetListUserNewOrOld(int associationId, int top, int type, DateTime dtmClearCacheRedis)
        //{
        //    var strKey = RedisConstants.GetListUserNewOrOld + ":" + associationId + ":" + top + ":" + type;
        //    var lstData = DaoFactory.AssociationMember.Out_AssociationMember_ListUserNewOrOld(associationId, top, type);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(x => new AssociationMember_ListUserNewOrOld_Redis
        //        {
        //            UserID = x.UserID,
        //            Coin = x.Coin.GetValueOrDefault(),
        //            PubUserID = x.PubUserID.GetValueOrDefault(0),
        //            NickName = x.NickName,
        //            VIPPoint = x.VIPPoint.GetValueOrDefault(),
        //            CreateDate = x.CreateDate.GetValueOrDefault(),

        //        }).ToList();
        //        string lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion

        #region Level

        public void Level_LevelUp(int userID)
        {
            var channel = string.Format("_pub_from_{0}_chan_{1}", DateTime.Now.Ticks, "_User.Request.Web");
            var Message = string.Format("{0}", userID);

            //Logger.CommonLogger.DefaultLogger.DebugFormat("channel: {0} -- Message: {1}", channel, Message);

            RedisFactory.MyRedis.PublishMessage(channel, Message);
        }

        #endregion

        #region Get Sinh nhat vip

        public List<Account_GetBirthdayUsers_Redis> GetSinhNhatVip(int pageLength, DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetBirthdayUsers + ":" + pageLength;
            var lstData = new List<Account_GetBirthdayUsers_Redis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<Account_GetBirthdayUsers_Redis>>(objRedisData);

            //temp delete
            //if (lstData == null || !lstData.Any())
            //    SetSinhNhatVip(pageLength, dtmClearCacheRedis);

            return lstData;
        }

        //public RedisStatusCode SetSinhNhatVip(int pageLength, DateTime dtmClearCacheRedis)
        //{
        //    var strKey = RedisConstants.GetBirthdayUsers + ":" + pageLength;
        //    var lstData = DaoFactory.Association.GetBirthdayUsers(pageLength); 

        //    if (lstData.Any())
        //    {
        //        var list = lstData.Select(x => new Account_GetBirthdayUsers_Redis
        //        {
        //            UserID = x.UserID,
        //            PubUserID = x.PubUserID.GetValueOrDefault(),
        //            NickName = x.NickName,
        //            VipPoint = x.VipPoint.GetValueOrDefault(),
        //            BirthDayID = x.BirthDayID,
        //        }).ToList();
        //        string lst = RedisCommon.ConvertToRedis(lstData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion
    }
}