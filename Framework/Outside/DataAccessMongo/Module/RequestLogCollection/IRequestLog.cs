namespace DataAccessMongo.Module.RequestLogCollection
{
    public interface IRequestLog
    {
        void InsertRequestLog(string ip, string domain, string referer, dynamic param);
    }
}