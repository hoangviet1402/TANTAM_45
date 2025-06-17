using System;
using System.Configuration;
using StackExchange.Redis;
//using MyConfig;

namespace DataAccessRedis
{
    /// <summary>
    ///     Create by Duynd - 16/05/2016
    /// </summary>
    public class RedisFactory
    {
        private static readonly Lazy<ConfigurationOptions> configOptions = new Lazy<ConfigurationOptions>(() =>
        {
            var configOptions = new ConfigurationOptions();
            configOptions.EndPoints.Add(RedisHost);
            configOptions.Password = RedisPassword;
            configOptions.ConnectTimeout = ConnectTimeout;
            configOptions.SyncTimeout = SyncTimeout;
            configOptions.DefaultDatabase = DefaultDatabase;
            configOptions.AbortOnConnectFail = AbortOnConnectFail;
            configOptions.AllowAdmin = true;
            return configOptions;
        });

        private static Lazy<ConnectionMultiplexer>
            _lazyConnection; // = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configOptions.Value));

        public static ConnectionMultiplexer Connection
        {
            get
            {
                if (_lazyConnection == null || !_lazyConnection.Value.IsConnected)
                    _lazyConnection =
                        new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configOptions.Value));
                return _lazyConnection.Value;
            }
        }

        public static IMyRedis MyRedis => new MyRedis(Connection, DefaultDatabase);

        /// <summary>
        ///     Ngat ket noi
        /// </summary>
        public static void Dispose()
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