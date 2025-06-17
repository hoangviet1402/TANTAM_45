using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using StackExchange.Redis;

namespace DataAccessRedis.Infrastructure
{
    public interface IRedisRepository
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
        ///     Xoa danh sach key
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
    }

    public class RedisRepository : IRedisRepository
    {
        private static readonly Lazy<ConfigurationOptions> ConfigOptions = new Lazy<ConfigurationOptions>(() =>
        {
            var configOptions = new ConfigurationOptions();
            configOptions.EndPoints.Add(RedisHost);
            configOptions.Password = RedisPassword;
            configOptions.ConnectTimeout = ConnectTimeout;
            configOptions.SyncTimeout = SyncTimeout;
            configOptions.DefaultDatabase = DefaultDatabase;
            configOptions.AbortOnConnectFail = AbortOnConnectFail;
            return configOptions;
        });

        private readonly IDatabase _db;

        private Lazy<ConnectionMultiplexer>
            _lazyConnection; // = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configOptions.Value));

        public RedisRepository()
        {
            _db = Connection.GetDatabase();
        }

        public ConnectionMultiplexer Connection
        {
            get
            {
                if (_lazyConnection == null || !_lazyConnection.Value.IsConnected)
                    _lazyConnection =
                        new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(ConfigOptions.Value));
                return _lazyConnection.Value;
            }
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
            if (listKey == null || !listKey.Any()) return;
            var lst = listKey.Where(item => !string.IsNullOrEmpty(item) && item.Length > 0)
                .Select(item => (RedisKey)item).ToList();

            if (lst.Any())
                _db.KeyDelete(lst.ToArray());
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
        ///     Ngat ket noi
        /// </summary>
        public void Dispose()
        {
            if (Connection != null)
                Connection.Dispose();
        }

        #region Varriables

        /// <summary>
        ///     Host server Redis
        /// </summary>
        private static string RedisHost
        {
            get
            {
                var value = ConfigurationManager.AppSettings["RedisHost"];
                if (string.IsNullOrEmpty(value))
                    return "";
                return value;
            }
        }

        /// <summary>
        ///     Password server Redis
        /// </summary>
        private static string RedisPassword
        {
            get
            {
                var value = ConfigurationManager.AppSettings["RedisPassword"];
                if (string.IsNullOrEmpty(value))
                    return "";
                return value;
            }
        }

        /// <summary>
        ///     Thoi gian timeout (ms) cho cac hoat dong ket noi
        /// </summary>
        private static int ConnectTimeout
        {
            get
            {
                var value = ConfigurationManager.AppSettings["ConnectTimeout"];
                if (string.IsNullOrEmpty(value))
                    return 100000;

                int i;
                if (int.TryParse(value, out i))
                    return Convert.ToInt32(value);

                return 100000;
            }
        }

        /// <summary>
        ///     Time (ms) to allow for synchronous operations
        /// </summary>
        private static int SyncTimeout
        {
            get
            {
                var value = ConfigurationManager.AppSettings["SyncTimeout"];
                if (string.IsNullOrEmpty(value))
                    return 100000;

                int i;
                if (int.TryParse(value, out i))
                    return Convert.ToInt32(value);

                return 100000;
            }
        }

        /// <summary>
        ///     Chi so co so du lieu mac dinh (mac dinh: 0)
        /// </summary>
        private static int DefaultDatabase
        {
            get
            {
                var value = ConfigurationManager.AppSettings["DefaultDatabase"];
                if (string.IsNullOrEmpty(value))
                    return 0;

                int i;
                if (int.TryParse(value, out i))
                    return Convert.ToInt32(value);

                return 0;
            }
        }

        private static bool AbortOnConnectFail
        {
            get
            {
                var value = ConfigurationManager.AppSettings["AbortOnConnectFail"];
                if (string.IsNullOrEmpty(value))
                    return false;

                bool i;
                if (bool.TryParse(value, out i))
                    return Convert.ToBoolean(value);

                return false;
            }
        }

        #endregion
    }
}