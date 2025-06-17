/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: IBaseDao
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System.Data.Entity;

namespace DataAccess.Base
{
    internal interface IBaseDao<TContext, TEntity>
        where TEntity : class
        where TContext : DbContext, new()
    {
        IUnitOfWork<TContext, TEntity> Uow { get; set; }
        bool Add(TEntity entity);
        bool Delete(object id);
        bool Delete(TEntity entity);
        TEntity GetOne(object id);
        bool Save(TEntity entity);
    }
}