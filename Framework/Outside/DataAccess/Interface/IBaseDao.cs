namespace DataAccess.Interface
{
    public interface IBaseFactories<TEntity> where TEntity : class
    {
        bool Add(TEntity entity);
        bool Delete(object id);
        bool Delete(TEntity entity);
        TEntity GetOne(object id);
        bool Save(TEntity entity);
    }
}