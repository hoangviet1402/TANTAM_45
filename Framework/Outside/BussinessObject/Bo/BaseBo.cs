/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: BaseBo
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using DataAccess.Interface;

namespace BussinessObject.Bo
{
    public abstract class BaseBo<TEntity>
        where TEntity : class
    {
        protected readonly IBaseFactories<TEntity> BaseFactories;

        protected BaseBo(IBaseFactories<TEntity> daoFactories)
        {
            BaseFactories = daoFactories;
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para></para>
        ///     Insert new entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add(TEntity entity)
        {
            return BaseFactories.Add(entity);
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
            return BaseFactories.Delete(id);
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
            return BaseFactories.Delete(entity);
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
            return BaseFactories.GetOne(id);
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
            return BaseFactories.Save(entity);
        }
    }
}