#if NO_ASYNC // EXCLUDE.
#else

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace qckdev.Data
{
    /// <summary>
    /// Defines the asynchronous extension methods to the <see cref="System.Data"/> namespace.
    /// </summary>
    public static partial class DataExtensions
    {
        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework data provider asynchronously, and returns the number of rows affected.
        /// If the connection is closed, it is automatically opened and closed. Otherwise the connection status does not change.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation with the number of rows affected.</returns>
        /// <exception cref="System.InvalidOperationException">The connection does not exist. -or- The connection is not open.</exception>
        public static Task<int> ExecuteNonQueryAutoAsync(this IDbCommand command, CancellationToken cancellationToken = default)
        {
            return CommandHelper.ExecuteNonQueryAutoAsync(command, cancellationToken);
        }

        /// <summary>
        /// Executes the query asynchronously, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation with the first column of the first row in the resultset.</returns>
        public static Task<object> ExecuteScalarAutoAsync(this IDbCommand command, CancellationToken cancellationToken = default)
        {
            return CommandHelper.ExecuteScalarAutoAsync(command, cancellationToken);
        }

        /// <summary>
        /// Executes the query asynchronously, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// If the connection is closed, it is automatically opened and closed. Otherwise the connection status does not change.
        /// </summary>
        /// <typeparam name="T">The returned value type.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>
        /// A task representing the asynchronous operation with the first column of the first row in the resultset converted to <typeparamref name="T"/> type. 
        /// When the result is <see cref="DBNull.Value"/>, it is converted to null.
        /// </returns>
        public static Task<T> ExecuteScalarAutoAsync<T>(this IDbCommand command, CancellationToken cancellationToken = default)
        {
            return CommandHelper.ExecuteScalarAutoAsync<T>(command, cancellationToken);
        }

        /// <summary>
        /// Executes the <see cref="System.Data.IDbCommand.CommandText"/> against the <see cref="System.Data.IDbCommand.Connection"/> 
        /// and builds an <see cref="System.Data.IDataReader"/> asynchronously.
        /// If the connection is closed, it is automatically opened and closed. Otherwise the connection status does not change.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation with an <see cref="IDataReader"/> object.</returns>
        public static Task<IDataReader> ExecuteReaderAutoAsync(this IDbCommand command, CancellationToken cancellationToken = default)
        {
            return CommandHelper.ExecuteReaderAutoAsync(command, cancellationToken);
        }

        /// <summary>
        /// Executes the <see cref="System.Data.IDbCommand.CommandText"/> against the <see cref="System.Data.IDbCommand.Connection"/>, 
        /// and builds an <see cref="System.Data.IDataReader"/> asynchronously using one of the <see cref="System.Data.CommandBehavior"/> values.
        /// If the connection is closed, it is automatically opened and closed. Otherwise the connection status does not change.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="behavior">One of the System.Data.CommandBehavior values.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation with an <see cref="IDataReader"/> object.</returns>
        public static Task<IDataReader> ExecuteReaderAutoAsync(this IDbCommand command, CommandBehavior behavior, CancellationToken cancellationToken = default)
        {
            return CommandHelper.ExecuteReaderAutoAsync(command, behavior, cancellationToken);
        }

        /// <summary>
        /// Returns a <see cref="System.Data.DataTable"/> with values from a data source using the supplied <see cref="System.Data.IDbCommand"/> asynchronously. 
        /// </summary>
        /// <param name="command">The <see cref="System.Data.IDbCommand"/> to execute.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation with a <see cref="DataTable"/>.</returns>
        public static Task<DataTable> ExecuteDataTableAutoAsync(this IDbCommand command, CancellationToken cancellationToken = default)
        {
            return CommandHelper.ExecuteDataTableAutoAsync(command, cancellationToken);
        }

        /// <summary>
        /// Returns a <see cref="System.Data.DataTable"/> with values from a data source using the supplied <see cref="System.Data.IDbCommand"/> asynchronously. 
        /// </summary>
        /// <param name="command">The <see cref="System.Data.IDbCommand"/> to execute.</param>
        /// <param name="loadOption">
        /// A value from the System.Data.LoadOption enumeration that indicates how rows already in the <see cref="System.Data.DataTable"/> are combined with incoming rows that share the same primary key.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation with a <see cref="DataTable"/>.</returns>
        public static Task<DataTable> ExecuteDataTableAutoAsync(this IDbCommand command, LoadOption loadOption, CancellationToken cancellationToken = default)
        {
            return CommandHelper.ExecuteDataTableAutoAsync(command, loadOption, cancellationToken);
        }

        /// <summary>
        /// Fills a <see cref="System.Data.DataTable"/> with values from a data source using the supplied <see cref="System.Data.IDbCommand"/> asynchronously. 
        /// If the <see cref="System.Data.DataTable"/> already contains rows, the incoming data from the data source is merged with the existing rows according to the value of the <paramref name="loadOption"/> parameter.
        /// </summary>
        /// <param name="command">The <see cref="System.Data.IDbCommand"/> to execute.</param>
        /// <param name="table">The <see cref="System.Data.DataTable"/> to fill.</param>
        /// <param name="loadOption">
        /// A value from the System.Data.LoadOption enumeration that indicates how rows already in the <see cref="System.Data.DataTable"/> are combined with incoming rows that share the same primary key.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task ExecuteDataTableAutoAsync(this IDbCommand command, DataTable table, LoadOption loadOption, CancellationToken cancellationToken = default)
        {
            return CommandHelper.ExecuteDataTableAutoAsync(command, table, loadOption, cancellationToken);
        }

        /// <summary>
        /// This is the asynchronous version of <see cref="System.Data.Common.DbDataReader.Read"/>. 
        /// Providers should override with an appropriate implementation. The cancellationToken may optionally be ignored. 
        /// The default implementation invokes the synchronous <see cref="System.Data.Common.DbDataReader.Read"/> method and returns a completed task, blocking the calling thread. 
        /// The default implementation will return a cancelled task if passed an already cancelled cancellationToken.
        /// Exceptions thrown by Read will be communicated via the returned Task Exception property. 
        /// Do not invoke other methods and properties of the DbDataReader object until the returned Task is complete.
        /// </summary>
        /// <param name="reader">The <see cref="System.Data.IDataReader"/> to read.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <exception cref="System.Data.Common.DbException">An error occurred while executing the command text.</exception>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<bool> ReadAsync(this IDataReader reader, CancellationToken cancellationToken = default)
        {
            return DataReaderHelper.ReadAsync(reader, cancellationToken);
        }

    }
}

#endif