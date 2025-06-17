using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Transactions;
using DataAccess.Extension;
using EntitiesObject.Entities;
/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: UnitOfWork Partten
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/


namespace DataAccess.Base
{
    internal class UnitOfWork<TConcext, TEntity> : IUnitOfWork<TConcext, TEntity>
        where TEntity : class
        where TConcext : DbContext, new()
    {
        //private Dictionary<Type, IRepositoryBase<TConcext, TEntity>> _repositories;
        private bool _disposed;

        public UnitOfWork(string connectionStringOrName = null)
        {
            Context = new TConcext();
            Context.Configuration.ValidateOnSaveEnabled = false;
            if (!string.IsNullOrEmpty(connectionStringOrName))
                Context.Database.Connection.ConnectionString = connectionStringOrName;
            //_repositories = new Dictionary<Type, IRepositoryBase<TConcext, TEntity>>();
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para></para>
        ///     Get Repository dua tren Entity Type
        /// </summary>
        /// <returns></returns>
        public IRepositoryBase<TConcext, TEntity> GetRepository()
        {
            //var repositoryKey = typeof(TEntity);
            //if (_repositories.Keys.Contains(repositoryKey))
            //    return _repositories[repositoryKey];

            IRepositoryBase<TConcext, TEntity> repository = new RepositoryBase<TConcext, TEntity>(Context);
            //_repositories.Add(repositoryKey, repository);
            return repository;
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para>
        ///         Khoi tai transaction voi IsolationLevel
        ///     </para>
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public TransactionScope BeginTransaction(IsolationLevel isolationLevel)
        {
            var options = new TransactionOptions { IsolationLevel = isolationLevel };
            return new TransactionScope(TransactionScopeOption.RequiresNew, options);
        }

        public TConcext Context { get; private set; }

        /// <summary>
        ///     Author: ThongNT
        ///     <para></para>
        ///     Luu cac thay doi xuong DB
        /// </summary>
        /// <returns></returns>
        public int CommitChanges()
        {
            return Context.SaveChanges();
        }

        public IEnumerable<TResult> ExecuteStoredProcedure<TResult>(IStoredProcedure<TResult> procedure,
            string storeName = null)
        {
            return Context.Database.ExecuteStoredProcedure(procedure);
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para></para>
        ///     Dispose Context (Important)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetIsolationLevel(IsolationLevel iso)
        {
            //Context.SetIsolation(iso);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (!disposing) return;
            if (Context == null) return;
            Context.Dispose();
            Context = null;
            _disposed = true;
            //_repositories = null;
        }
    }
}