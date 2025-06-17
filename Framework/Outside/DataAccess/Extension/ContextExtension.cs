using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using EntitiesObject.Entities;

namespace DataAccess.Extension
{
    public static class ContextExtension
    {
        public static int ExecuteStoredProcedure(this Database context, string storeProcName,
            params object[] parameters)
        {
            var command = "EXEC " + storeProcName + " ";
            for (var i = 0; i < parameters.Length; i++)
            {
                command += "@" + parameters[i];
                command += i - 1 == parameters.Length ? string.Empty : ", ";
            }

            return context.ExecuteSqlCommand(command, parameters);
        }

        #region support method

        /// <summary>
        ///     thong.nguyen
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isolationLevel"></param>
        internal static void SetIsolationLevel(this DbContext context, IsolationLevel isolationLevel)
        {
            string sql;

            switch (isolationLevel)
            {
                case IsolationLevel.ReadUncommitted:
                    sql = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;";
                    break;
                case IsolationLevel.ReadCommitted:
                    sql = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED;";
                    break;
                default:
                    throw new Exception("ISOLATION LEVEL is not defined in this method.");
            }

            //(context as IObjectContextAdapter).ObjectContext.ExecuteStoreCommand(sql, null);
            if (context.Database.Connection.State != ConnectionState.Open)
                // Explicitly open the connection, this connection will close when context is disposed
                context.Database.Connection.Open();

            context.Database.ExecuteSqlCommand(sql);
        }


        public static IEnumerable<TResult> ExecuteStoredProcedure<TResult>(this Database database,
            IStoredProcedure<TResult> procedure, string storeName = null)
        {
            var parameters = CreateSqlParametersFromProperties(procedure);

            var format = CreateSpCommand<TResult>(parameters, storeName);

            return database.SqlQuery<TResult>(format, parameters.Cast<object>().ToArray());
        }

        private static List<SqlParameter> CreateSqlParametersFromProperties<TResult>(
            IStoredProcedure<TResult> procedure)
        {
            var procedureType = procedure.GetType();
            var propertiesOfProcedure = procedureType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var parameters =
                propertiesOfProcedure.Select(propertyInfo => new SqlParameter(string.Format("@{0}", propertyInfo.Name),
                        propertyInfo.GetValue(procedure, new object[] { })))
                    .ToList();
            return parameters;
        }

        private static string CreateSpCommand<TResult>(List<SqlParameter> parameters, string storeName = null)
        {
            var queryString = storeName;
            if (string.IsNullOrEmpty(queryString))
            {
                storeName = typeof(TResult).Name;
                queryString = storeName.Substring(0, storeName.LastIndexOf('_'));
            }

            parameters.ForEach(x => queryString = string.Format("{0} {1},", queryString, x.ParameterName));

            return queryString.TrimEnd(',');
        }

        #endregion
    }
}