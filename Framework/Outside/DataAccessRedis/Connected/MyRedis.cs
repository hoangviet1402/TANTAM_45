using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace DataAccessRedis
{
    /// <summary>
    ///     Create by Duynd - 16/05/2016
    /// </summary>
    public interface IMyRedis
    {
        /// <summary>
        ///     Tao du lieu kieu string
        ///     zaads
        /// </summary>
        /// <param name="key">Key khong duoc rong, khong duoc null, 2 ky tu tro len</param>
        /// <param name="value">Noi dung muon luu tru</param>
        /// <param name="expire">Thoi gian het han (khong bat buoc truyen vao)</param>
        void Set(string key = "", string value = "", DateTime? expire = null);

        /// <summary>
        ///     Lay du lieu kieu string
        /// </summary>
        /// <param name="key">Key khong duoc rong, khong duoc null, 2 ky tu tro len</param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>
        ///     Tao du lieu kieu hash
        /// </summary>
        /// <param name="key">Key khong duoc rong, khong duoc null, 2 ky tu tro len</param>
        /// <param name="dictionary">Du lieu muon luu tru</param>
        void HashSet(string key, Dictionary<string, string> dictionary);

        /// <summary>
        ///     Lay du lieu kieu hash
        /// </summary>
        /// <param name="key">Key khong duoc rong, khong duoc null, 2 ky tu tro len</param>
        /// <returns></returns>
        Dictionary<RedisValue, RedisValue> HashGetAll(string key);

        /// <summary>
        ///     Xoa mot key
        /// </summary>
        /// <param name="key">Key khong duoc rong, khong duoc null, 2 ky tu tro len</param>
        /// <returns></returns>
        void Delete(string key);

        /// <summary>
        ///     Xoa danh sach key
        /// </summary>
        /// <param name="listKey">Danh sach key can xoa</param>
        void Delete(List<string> listKey);

        /// <summary>
        ///     Tim tat ca cac key theo pattern, VD: guo:*
        /// </summary>
        /// <param name="pattern">pattern khong duoc rong, khong duoc null, 2 ky tu tro len</param>
        /// <returns></returns>
        string[] Scan(string pattern);

        /// <summary>
        ///     Lay danh sach co sap xep
        /// </summary>
        /// <param name="key">Key can sort</param>
        /// <param name="skip">So element bo qua</param>
        /// <param name="take">So element muon lay</param>
        /// <param name="order">Sap xep</param>
        /// <param name="sortType">Kieu du lieu sap xep</param>
        /// <param name="by">Column dung de sap xep</param>
        /// <param name="get">Danh sach value muon lay</param>
        /// <param name="flags">....</param>
        List<string> Sort(string key, long skip = 0, long take = -1, Order order = Order.Ascending,
            SortType sortType = SortType.Numeric, string by = "", RedisValue[] get = null,
            CommandFlags flags = CommandFlags.None);

        /// <summary>
        ///     Lay danh sach co sap xep
        /// </summary>
        /// <param name="key">Key can sort</param>
        /// <param name="skip">So element bo qua</param>
        /// <param name="take">So element muon lay</param>
        /// <param name="order">Sap xep</param>
        /// <param name="exclude">Kieu du lieu sap xep</param>
        /// <param name="flags">....</param>
        /// <returns>value:score</returns>
        List<SortedSetEntry> SortSetEntryByScore(string key, long skip = 0, long take = -1,
            Order order = Order.Ascending, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None);

        /// <summary>
        ///     Lay score cua member trong danh sach sortedset
        /// </summary>
        /// <param name="key">Key chua data (kieu sortedset)</param>
        /// <param name="member">Value</param>
        /// <param name="flags">....</param>
        /// <returns>value:score</returns>
        double SortSetEntryScore(string key, string member, CommandFlags flags = CommandFlags.None);


        void PublishMessage(string channel, string message);
    }

    internal class MyRedis : IMyRedis
    {
        private readonly ConnectionMultiplexer _connection;
        private readonly IDatabase _db;
        private readonly int _defaultDatabase;

        public MyRedis(ConnectionMultiplexer conn, int defaultDatabase)
        {
            _defaultDatabase = defaultDatabase;
            if (_connection == null)
                _connection = conn;

            _db = _connection.GetDatabase();
            _defaultDatabase = defaultDatabase;
        }


        /// <summary>
        ///     Tao du lieu kieu string
        /// </summary>
        /// <param name="key">Key khong duoc rong, khong duoc null, 2 ky tu tro len</param>
        /// <param name="value">Noi dung muon luu tru</param>
        /// <param name="expire">Thoi gian het han (khong bat buoc truyen vao)</param>
        public void Set(string key, string value, DateTime? expire)
        {
            if (!string.IsNullOrEmpty(key) && key.Length > 1)
            {
                var ts = new TimeSpan();
                if (expire != null)
                {
                    ts = expire.Value - DateTime.Now;
                    _db.StringSetAsync(key, value, ts);
                }
                else
                {
                    _db.StringSetAsync(key, value);
                }
            }
        }

        /// <summary>
        ///     Lay du lieu kieu string
        /// </summary>
        /// <param name="key">Key khong duoc rong, khong duoc null, 2 ky tu tro len</param>
        /// <returns></returns>
        public string Get(string key)
        {
            if (!string.IsNullOrEmpty(key) && key.Length > 1)
            {
                var result = _db.StringGet(key).ToString();
                return result ?? "";
            }

            return "";
        }

        /// <summary>
        ///     Tao du lieu kieu hash
        /// </summary>
        /// <param name="key">Key khong duoc rong, khong duoc null, 2 ky tu tro len</param>
        /// <param name="dictionary">Du lieu muon luu tru</param>
        public void HashSet(string key, Dictionary<string, string> dictionary)
        {
            if (!string.IsNullOrEmpty(key) && key.Length > 1)
            {
                var fields = dictionary.Select(pair => new HashEntry(pair.Key, pair.Value)).ToArray();
                _db.HashSetAsync(key, fields);
            }
        }

        /// <summary>
        ///     Lay du lieu kieu hash
        /// </summary>
        /// <param name="key">Key khong duoc rong, khong duoc null, 2 ky tu tro len</param>
        /// <returns></returns>
        public Dictionary<RedisValue, RedisValue> HashGetAll(string key)
        {
            if (!string.IsNullOrEmpty(key) && key.Length > 1)
                return _db.HashGetAll(key).ToDictionary();

            return null;
        }

        /// <summary>
        ///     Xoa mot key
        /// </summary>
        /// <param name="key">Key khong duoc rong, khong duoc null, 2 ky tu tro len</param>
        /// <returns></returns>
        public void Delete(string key)
        {
            if (!string.IsNullOrEmpty(key) && key.Length > 1)
                Delete(new List<string> { key });
        }

        /// <summary>
        ///     Xoa danh sach key
        /// </summary>
        /// <param name="listKey">Danh sach key can xoa</param>
        public void Delete(List<string> listKey)
        {
            if (listKey != null && listKey.Any())
            {
                var lst = new List<RedisKey>();
                foreach (var item in listKey)
                    if (!string.IsNullOrEmpty(item) && item.Length > 0)
                        lst.Add(item);

                if (lst.Any())
                    _db.KeyDelete(lst.ToArray());
            }
        }

        /// <summary>
        ///     Tim key redis tra ve kieu mang string
        ///     Create by Duynd - 29/06/2016
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public string[] Scan(string pattern)
        {
            if (!string.IsNullOrEmpty(pattern) && pattern.Length > 1)
            {
                var server = _connection.GetServer(_connection.GetEndPoints()[0]);
                var result = server.Keys(pattern: pattern, database: _defaultDatabase).ToArray();

                if (result.Any())
                    return Array.ConvertAll(result, x => (string)x);
            }

            return new string[0] { };
        }

        /// <summary>
        ///     Lay danh sach co sap xep
        /// </summary>
        /// <param name="key">Key can sort</param>
        /// <param name="skip">So element bo qua</param>
        /// <param name="take">So element muon lay</param>
        /// <param name="order">Sap xep</param>
        /// <param name="sortType">Kieu du lieu sap xep</param>
        /// <param name="by">Column dung de sap xep</param>
        /// <param name="get">Danh sach value muon lay</param>
        /// <param name="flags">....</param>
        public List<string> Sort(string key, long skip = 0, long take = -1, Order order = Order.Ascending,
            SortType sortType = SortType.Numeric, string by = "", RedisValue[] get = null,
            CommandFlags flags = CommandFlags.None)
        {
            if (string.IsNullOrEmpty(key)) return new List<string>();

            var redisKey = (RedisKey)key;
            var redisValueResult = _db.Sort(redisKey, skip, take, order, sortType, by, get, flags);
            if (redisValueResult == null || redisValueResult.Length == 0) return new List<string>();

            return redisValueResult.Select(r => r.ToString()).ToList();
        }

        /// <summary>
        ///     Lay danh sach co sap xep
        /// </summary>
        /// <param name="key">Key can sort</param>
        /// <param name="skip">So element bo qua</param>
        /// <param name="take">So element muon lay</param>
        /// <param name="order">Sap xep</param>
        /// <param name="exclude">Kieu du lieu sap xep</param>
        /// <param name="flags">....</param>
        /// <returns>value:score</returns>
        public List<SortedSetEntry> SortSetEntryByScore(string key, long skip = 0, long take = -1,
            Order order = Order.Ascending, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            if (string.IsNullOrEmpty(key)) return new List<SortedSetEntry>();

            var redisKey = (RedisKey)key;
            var redisValueResult = _db.SortedSetRangeByScoreWithScores(redisKey, double.NegativeInfinity,
                double.PositiveInfinity, exclude, order, skip, take, flags);
            if (redisValueResult == null || redisValueResult.Length == 0) return new List<SortedSetEntry>();

            return redisValueResult.ToList();
        }

        /// <summary>
        ///     Lay score cua member trong danh sach sortedset
        /// </summary>
        /// <param name="key">Key chua data (kieu sortedset)</param>
        /// <param name="member">Value</param>
        /// <param name="flags">....</param>
        /// <returns>value:score</returns>
        public double SortSetEntryScore(string key, string member, CommandFlags flags = CommandFlags.None)
        {
            if (string.IsNullOrEmpty(key)) return 0;

            var redisKey = (RedisKey)key;
            var redisValue = (RedisValue)member;
            var redisValueResult = _db.SortedSetScore(redisKey, redisValue, flags);
            return redisValueResult.HasValue ? redisValueResult.GetValueOrDefault(0) : 0;
        }

        /// <summary>
        ///     đẩy thông tin xuống channel
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public void PublishMessage(string channel, string message)
        {
            _db.Publish(channel, message);
        }
    }
}