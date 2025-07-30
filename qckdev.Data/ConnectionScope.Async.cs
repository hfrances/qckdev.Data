#if NO_ASYNC // EXCLUDE.
#else

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace qckdev.Data
{
    sealed partial class ConnectionScope
    {

        #region static async

        public static async Task<ConnectionScope> CreateAsync(IDbConnection connection, CancellationToken cancellationToken = default)
        {
            return await CreateAsync(connection, createTransaction: false, cancellationToken);
        }

        public static async Task<ConnectionScope> CreateAsync(IDbConnection connection, bool createTransaction, CancellationToken cancellationToken = default)
        {
            return await CreateAsync(connection, createTransaction, isolationLevel: null, cancellationToken);
        }

        public static async Task<ConnectionScope> CreateAsync(IDbConnection connection, IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            return await CreateAsync(connection, createTransaction: true, isolationLevel: isolationLevel, cancellationToken);
        }

        private static async Task<ConnectionScope> CreateAsync(IDbConnection connection, bool createTransaction, IsolationLevel? isolationLevel, CancellationToken cancellationToken = default)
        {
            System.Data.Common.DbConnection connectionAsync;
            ConnectionState initialState = connection.State;
            IDbTransaction transaction;

            /* Open with check */
            connectionAsync = connection as System.Data.Common.DbConnection;
            if (initialState == ConnectionState.Closed)
            {
                if (connectionAsync == null)
                    connection.Open();
                else
                    await connectionAsync.OpenAsync();
            }
            
            /* Begin transaction */
            if (createTransaction)
            {
                if (isolationLevel == null)
                    transaction = connection.BeginTransaction();
                else
                    transaction = connection.BeginTransaction(isolationLevel.Value);
            }
            else
            {
                transaction = null;
            }

            return new ConnectionScope(connection, transaction, initialState);
        }

        // Private constructor for async creation
        private ConnectionScope()
        {
        }

        #endregion

    }
}

#endif