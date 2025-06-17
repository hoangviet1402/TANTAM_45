/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: Repository Partten
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DataAccess.Base
{
    internal interface IRepositoryBase<TConcext, TEntity>
        where TEntity : class
        where TConcext : DbContext
    {
        DbSet<TEntity> Entities { get; }
        void Add(TEntity entity);

        void Delete(object id);
        void Delete(TEntity entity);
        DbEntityEntry<TEntity> Entry(TEntity entity);

        TEntity GetOne(object id);

        void Save(TEntity entity);
    }
}