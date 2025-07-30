using System;
using System.Data;
using System.Threading;

namespace qckdev.Data
{
    sealed partial class ConnectionScope : IDisposable
    {

        #region ctor

        public ConnectionScope(IDbConnection connection)
            : this(connection, createTransaction: false) { }

        public ConnectionScope(IDbConnection connection, bool createTransaction)
            : this(connection, createTransaction, isolationLevel: null) { }

        public ConnectionScope(IDbConnection connection, IsolationLevel isolationLevel)
            : this(connection, createTransaction: true, isolationLevel: IsolationLevel.Unspecified) { }

        private ConnectionScope(IDbConnection connection, bool createTransaction, IsolationLevel? isolationLevel)
        {
            this.Connection = connection;

            ConnectionHelper.OpenWithCheck(connection, out ConnectionState initialState);
            this.InitialState = initialState;

            if (createTransaction)
            {
                if (isolationLevel == null)
                    this.Transaction = connection.BeginTransaction();
                else
                    this.Transaction = connection.BeginTransaction(isolationLevel.Value);
            }
        }

        private ConnectionScope(IDbConnection connection, IDbTransaction transaction, ConnectionState initialState)
        {
            this.Connection = connection;
            this.Transaction = transaction;
            this.InitialState = initialState;
        }

        #endregion

        #region properties

        public IDbConnection Connection { get; }
        public ConnectionState InitialState { get; }
        public IDbTransaction Transaction { get; }

        #endregion

        #region methods

        public void Dispose()
        {
            Transaction?.Dispose();
            ConnectionHelper.CloseWithCheck(this.Connection, this.InitialState);
        }

        #endregion

        #region static

        public static ConnectionScope Create(IDbConnection connection)
        {
            return Create(connection, createTransaction: false);
        }

        public static ConnectionScope Create(IDbConnection connection, bool createTransaction)
        {
            return Create(connection, createTransaction, isolationLevel: null);
        }

        public static ConnectionScope Create(IDbConnection connection, IsolationLevel isolationLevel)
        {
            return Create(connection, createTransaction: true, isolationLevel: isolationLevel);
        }

        private static ConnectionScope Create(IDbConnection connection, bool createTransaction, IsolationLevel? isolationLevel)
        {
            ConnectionState initialState;
            IDbTransaction transaction;

            ConnectionHelper.OpenWithCheck(connection, out initialState);

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

        #endregion

    }
}