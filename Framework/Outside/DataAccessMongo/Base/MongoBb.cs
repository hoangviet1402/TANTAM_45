using MongoDB.Driver;

namespace DataAccessMongo.Base
{
    public interface IMongoBb
    {
        IMongoDatabase GetDatabase();

        IMongoCollection<T> GetCollection<T>(string CollectionName);
    }

    internal class MongoBb : IMongoBb
    {
        private readonly IMongoDatabase _db;

        public MongoBb(IMongoDatabase db)
        {
            if (!MongoFactory.MongoEnable)
                return;

            _db = db;
        }

        public IMongoCollection<T> GetCollection<T>(string CollectionName)
        {
            if (!MongoFactory.MongoEnable)
                return null;

            var objData = _db.GetCollection<T>(CollectionName);
            return objData;
        }

        public IMongoDatabase GetDatabase()
        {
            if (!MongoFactory.MongoEnable)
                return null;

            return _db;
        }
    }
}