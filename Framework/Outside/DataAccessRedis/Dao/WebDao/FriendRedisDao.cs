using System;
using DataAccessRedis.Model;
using Newtonsoft.Json;

namespace DataAccessRedis.Dao.WebDao
{
    public interface IFriendRedisDao
    {
        void SaveCacheFriend(int userId, string friendType, object data, DateTime? expireTime = null);
        CacheDefaultData GetCacheFriend(int userId, string friendType);
        void RemoveCacheFriend(int userId, string friendType);
    }

    internal class FriendRedisDao : IFriendRedisDao
    {
        private const int CacheTimeDayDefault = 30;
        private const string CacheNameFormat = "friends:{0}:{1}";

        public void SaveCacheFriend(int userId, string friendType, object data, DateTime? expireTime = null)
        {
            var cacheTime = expireTime == null
                ? DateTime.Now.AddDays(CacheTimeDayDefault)
                : expireTime.GetValueOrDefault();

            var cacheName = string.Format(CacheNameFormat, userId, friendType);
            var cacheData = new CacheDefaultData { Data = data };
            RedisFactory.MyRedis.Set(cacheName, JsonConvert.SerializeObject(cacheData), cacheTime);
        }

        public CacheDefaultData GetCacheFriend(int userId, string friendType)
        {
            var cacheName = string.Format(CacheNameFormat, userId, friendType);
            var cacheDataString = RedisFactory.MyRedis.Get(cacheName);

            return string.IsNullOrEmpty(cacheDataString)
                ? null
                : JsonConvert.DeserializeObject<CacheDefaultData>(cacheDataString);
        }

        public void RemoveCacheFriend(int userId, string friendType)
        {
            var cacheName = string.Format(CacheNameFormat, userId, friendType);
            RedisFactory.MyRedis.Delete(cacheName);
        }
    }
}