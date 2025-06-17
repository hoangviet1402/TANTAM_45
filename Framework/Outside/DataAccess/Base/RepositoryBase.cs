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
using EntityState = System.Data.Entity.EntityState;

namespace DataAccess.Base
{
    internal sealed class RepositoryBase<TConcext, TEntity> : IRepositoryBase<TConcext, TEntity>
        where TEntity : class
        where TConcext : DbContext
    {
        private readonly TConcext _context;

        public RepositoryBase(TConcext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Entities => _context.Set<TEntity>();

        public TEntity GetOne(object id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void Add(TEntity entity)
        {
            SetEntityState(entity, EntityState.Added);
        }

        public void Delete(TEntity entity)
        {
            SetEntityState(entity, EntityState.Deleted);
        }

        public void Delete(object id)
        {
            var entity = GetOne(id);
            SetEntityState(entity, EntityState.Deleted);
        }

        public void Save(TEntity entity)
        {
            SetEntityState(entity, EntityState.Modified);
        }

        public DbEntityEntry<TEntity> Entry(TEntity entity)
        {
            return _context.Entry(entity);
        }

        private void SetEntityState(TEntity entity, EntityState state)
        {
            var dbEntry = _context.Entry(entity);
            if (dbEntry != null && dbEntry.State != state) dbEntry.State = state;
        }
    }
}