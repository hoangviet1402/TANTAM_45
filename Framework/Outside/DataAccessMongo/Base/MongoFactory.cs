using System.Configuration;
using MongoDB.Driver;

namespace DataAccessMongo.Base
{
    public class MongoFactory
    {
        public static bool MongoEnable => true;

        /// <summary>
        ///     Host server Redis
        /// </summary>
        private static string MongoHost
        {
            get
            {
                var value = ConfigurationManager.AppSettings["MongoHost"];
                if (string.IsNullOrEmpty(value))
                    return "192.168.1.63";
                return value;
            }
        }

        private static int MongoPort
        {
            get
            {
                var getValue = ConfigurationManager.AppSettings["MongoPort"];
                var value = 0;
                if (string.IsNullOrEmpty(getValue))
                    return 27017;
                int.TryParse(getValue, out value);
                return value;
            }
        }

        private static string MongoDatabase
        {
            get
            {
                var value = ConfigurationManager.AppSettings["MongoDatabase"];
                if (string.IsNullOrEmpty(value))
                    return "BanCa";
                return value;
            }
        }


        public static IMongoDatabase GetDatabase
        {
            get
            {
                var settings = new MongoClientSettings();
                settings.Server = new MongoServerAddress(MongoHost, MongoPort);
                var client = new MongoClient(settings);
                return client.GetDatabase(MongoDatabase);
            }
        }

        public static IMongoBb MongoBb => new MongoBb(GetDatabase);
    }
}