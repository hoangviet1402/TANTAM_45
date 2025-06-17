/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: BaseDao CRUD function
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System.Data.Entity;

namespace DataAccess.Base
{
    internal class BaseDao<TContext, TEntity> : IBaseDao<TContext, TEntity>
        where TContext : DbContext, new() where TEntity : class
    {
        private IUnitOfWork<TContext, TEntity> _unitOfWork;

        /// <summary>
        ///     Author: ThongNT
        ///     Them moi Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add(TEntity entity)
        {
            using (Uow)
            {
                var repository = Uow.GetRepository();
                repository.Add(entity);
                return Uow.CommitChanges() > 0;
            }
        }

        /// <summary>
        ///     Author: ThongNT
        ///     Xoa Entity su dung Key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(object id)
        {
            using (Uow)
            {
                var repository = Uow.GetRepository();
                repository.Delete(id);
                return Uow.CommitChanges() > 0;
            }
        }

        /// <summary>
        ///     Author: ThongNT
        ///     Xoa Entity su dung Entity Instant
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(TEntity entity)
        {
            using (Uow)
            {
                var repository = Uow.GetRepository();
                repository.Delete(entity);
                return Uow.CommitChanges() > 0;
            }
        }

        /// <summary>
        ///     Author: ThongNT
        ///     Get One Entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetOne(object id)
        {
            using (Uow)
            {
                var repository = Uow.GetRepository();
                return repository.GetOne(id);
            }
        }

        /// <summary>
        ///     Author: ThongNT
        ///     Update Entity to DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Save(TEntity entity)
        {
            using (Uow)
            {
                var repository = Uow.GetRepository();
                repository.Save(entity);
                return Uow.CommitChanges() > 0;
            }
        }

        public IUnitOfWork<TContext, TEntity> Uow
        {
            get => _unitOfWork ?? (_unitOfWork = new UnitOfWork<TContext, TEntity>());
            set => _unitOfWork = value;
        }
    }
}