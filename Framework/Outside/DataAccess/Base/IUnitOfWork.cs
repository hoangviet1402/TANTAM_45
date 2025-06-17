/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: IUnitOfWork Partten
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Transactions;
using EntitiesObject.Entities;

namespace DataAccess.Base
{
    internal interface IUnitOfWork<TConcext, TEntity> : IDisposable
        where TEntity : class
        where TConcext : DbContext
    {
        TConcext Context { get; }

        IRepositoryBase<TConcext, TEntity> GetRepository();

        //void SetIsolationLevel(System.Data.IsolationLevel iso);
        IEnumerable<TResult> ExecuteStoredProcedure<TResult>(IStoredProcedure<TResult> procedure,
            string storeName = null);

        TransactionScope BeginTransaction(IsolationLevel isolationLevel);
        int CommitChanges();
    }
}