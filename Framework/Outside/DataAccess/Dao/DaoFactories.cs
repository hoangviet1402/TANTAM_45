/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: DaoFactories
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System.Data.Entity;
using DataAccess.Base;

namespace DataAccess.Dao
{
    public abstract class DaoFactories<TContext, TEntity>
        where TEntity : class
        where TContext : DbContext, new()
    {
        private IBaseDao<TContext, TEntity> _baseDao;

        private IBaseDao<TContext, TEntity> BaseDao => _baseDao ?? (_baseDao = new BaseDao<TContext, TEntity>());

        /// <summary>
        ///     Author: ThongNT
        ///     <para>
        ///         Unit Of Work partten
        ///     </para>
        /// </summary>
        internal IUnitOfWork<TContext, TEntity> Uow => BaseDao.Uow;

        /// <summary>
        ///     Author: ThongNT
        ///     <para></para>
        ///     Insert new entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add(TEntity entity)
        {
            return BaseDao.Add(entity);
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para></para>
        ///     Delete entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(object id)
        {
            return BaseDao.Delete(id);
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para></para>
        ///     Delete Entity By Instant
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(TEntity entity)
        {
            return BaseDao.Delete(entity);
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para></para>
        ///     Get One Entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetOne(object id)
        {
            return BaseDao.GetOne(id);
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para></para>
        ///     Update Entity to DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Save(TEntity entity)
        {
            return BaseDao.Save(entity);
        }
    }
}