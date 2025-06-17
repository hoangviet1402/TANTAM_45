using DataAccessMongo.Model.E2003_LogAllEventStatusCollection;

namespace DataAccessMongo.Module.E2003_LogAllEventStatusCollection
{
    public interface IE2003LogAllEventStatus
    {
        void LogAllEventStatus_Insert(E2003_LogAllEventStatusMongoModel e2003_LogAllEventStatusModel);
    }
}