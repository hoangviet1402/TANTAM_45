//using MyConfig;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataAccessRedis.Constant;
using DataAccessRedis.Infrastructure;
using DataAccessRedis.Model.Event;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DataAccessRedis.Dao.EventDao
{
    public interface IEventInfoDao
    {
        List<UserEventNoHu> GetListEvent(int gameId, int userid);

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2016-08-05</para>
        ///     <para>Description: Lay top danh sach user</para>
        /// </summary>
        /// <param name="gameId">Id game can lay</param>
        /// <param name="top">So luong top user can lay</param>
        /// <returns></returns>
        List<EventNoHuUserRankModel> ENoHuGetTopUser(int gameId = 0, int top = 10);

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2016-08-05</para>
        ///     <para>Description: Lay gold cua user</para>
        /// </summary>
        /// <param name="gameId">Id game can lay</param>
        /// <param name="userId">So luong top user can lay</param>
        /// <returns></returns>
        double ENoHuGetGoldUser(int gameId = 0, int userId = 0);

        string GettotalEventnohu();

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2016-08-23</para>
        ///     <para>Description: Lay tong dong gop cua user</para>
        /// </summary>
        /// <returns></returns>
        string GettotalEventnohuMini();

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2016-08-05</para>
        ///     <para>Description: Lay gold cua user</para>
        /// </summary>
        /// <param name="gameId">Id game can lay</param>
        /// <param name="userId">So luong top user can lay</param>
        /// <returns></returns>
        double ENoHuMiniGetGoldUser(int gameId = 0, int userId = 0);

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2016-08-05</para>
        ///     <para>Description: Lay top danh sach user</para>
        /// </summary>
        /// <param name="gameId">Id game can lay</param>
        /// <param name="top">So luong top user can lay</param>
        /// <returns></returns>
        List<EventNoHuUserRankModel> ENoHuMiniGetTopUser(int gameId = 0, int top = 10);
    }

    internal class EventInfoDao : IEventInfoDao
    {
        public IRedisRepository RedisRepository { get; set; }

        public List<UserEventNoHu> GetListEvent(int gameId, int userid)
        {
            var lstData = new List<UserEventNoHu>();
            var strKey = RedisConstantsEvent.EventNoHu + ":" + gameId + ":" + userid;
            var objRedisData = RedisRepository.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = JsonConvert.DeserializeObject<List<UserEventNoHu>>(objRedisData);
            return lstData;
        }

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2016-08-05</para>
        ///     <para>Description: Lay top danh sach user</para>
        /// </summary>
        /// <param name="gameId">Id game can lay</param>
        /// <param name="top">So luong top user can lay</param>
        /// <returns></returns>
        public List<EventNoHuUserRankModel> ENoHuGetTopUser(int gameId = 0, int top = 10)
        {
            var strKey = RedisConstantsEvent.EventNoHu + ":" + gameId;
            var objRedisData = RedisFactory.MyRedis.SortSetEntryByScore(strKey, 0, top, Order.Descending);
            if (objRedisData != null && objRedisData.Any())
                return objRedisData.Select(objData => new EventNoHuUserRankModel
                {
                    Gold = objData.Score,
                    UserId = objData.Element.HasValue
                        ? int.Parse(objData.Element.ToString())
                        : 0
                }).ToList();

            return new List<EventNoHuUserRankModel>();
        }

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2016-08-05</para>
        ///     <para>Description: Lay gold cua user</para>
        /// </summary>
        /// <param name="gameId">Id game can lay</param>
        /// <param name="userId">So luong top user can lay</param>
        /// <returns></returns>
        public double ENoHuGetGoldUser(int gameId = 0, int userId = 0)
        {
            var strKey = RedisConstantsEvent.EventNoHu + ":" + gameId;
            var objRedisData =
                RedisFactory.MyRedis.SortSetEntryScore(strKey, userId.ToString(CultureInfo.CurrentCulture));
            return objRedisData > 0 ? objRedisData : 0;
        }

        public string GettotalEventnohu()
        {
            const string strKey = RedisConstantsEvent.EventNoHuTotal;
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            return objRedisData;
        }

        public string GettotalEventnohuMini()
        {
            const string strKey = RedisConstantsEvent.EventNoHuTotalMini;
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            return objRedisData;
        }

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2016-08-05</para>
        ///     <para>Description: Lay gold cua user</para>
        /// </summary>
        /// <param name="gameId">Id game can lay</param>
        /// <param name="userId">So luong top user can lay</param>
        /// <returns></returns>
        public double ENoHuMiniGetGoldUser(int gameId = 0, int userId = 0)
        {
            var strKey = RedisConstantsEvent.EventNoHuMini + ":" + gameId;
            var objRedisData =
                RedisFactory.MyRedis.SortSetEntryScore(strKey, userId.ToString(CultureInfo.CurrentCulture));
            return objRedisData > 0 ? objRedisData : 0;
        }

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2016-08-05</para>
        ///     <para>Description: Lay top danh sach user</para>
        /// </summary>
        /// <param name="gameId">Id game can lay</param>
        /// <param name="top">So luong top user can lay</param>
        /// <returns></returns>
        public List<EventNoHuUserRankModel> ENoHuMiniGetTopUser(int gameId = 0, int top = 10)
        {
            var strKey = RedisConstantsEvent.EventNoHuMini + ":" + gameId;
            var objRedisData = RedisFactory.MyRedis.SortSetEntryByScore(strKey, 0, top, Order.Descending);
            if (objRedisData != null && objRedisData.Any())
                return objRedisData.Select(objData => new EventNoHuUserRankModel
                {
                    Gold = objData.Score,
                    UserId = objData.Element.HasValue
                        ? int.Parse(objData.Element.ToString())
                        : 0
                }).ToList();

            return new List<EventNoHuUserRankModel>();
        }
    }
}