#if NO_ASYNC // EXCLUDE.
#else

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace qckdev.Data
{
    static partial class CommandHelper
    {

        public static async Task<T> ExecuteScalarAsync<T>(IDbCommand command, CancellationToken cancellationToken = default)
        {
            T rdo;
            object val;

            if (command is System.Data.Common.DbCommand commandAsync)
                val = await commandAsync.ExecuteScalarAsync();
            else
                val = command.ExecuteScalar();

            if (val == DBNull.Value)
            {
                rdo = default(T);
            }
            else
            {
                var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                rdo = (T)Convert.ChangeType(val, type);
            }
            return rdo;
        }

        public static async Task<int> ExecuteNonQueryAutoAsync(IDbCommand command, CancellationToken cancellationToken = default)
        {
            int rdo;

            using (var scope = await ConnectionScope.CreateAsync(command.Connection, cancellationToken))
            {
                if (command is System.Data.Common.DbCommand commandAsync)
                    rdo = await commandAsync.ExecuteNonQueryAsync(cancellationToken);
                else
                    rdo = command.ExecuteNonQuery();
            }
            return rdo;
        }

        public static async Task<object> ExecuteScalarAutoAsync(IDbCommand command, CancellationToken cancellationToken = default)
        {
            object rdo;

            using (var scope = await ConnectionScope.CreateAsync(command.Connection, cancellationToken))
            {
                if (command is System.Data.Common.DbCommand commandAsync)
                    rdo = await commandAsync.ExecuteScalarAsync(cancellationToken);
                else
                    rdo = command.ExecuteScalar();
            }
            return rdo;
        }

        public static async Task<T> ExecuteScalarAutoAsync<T>(IDbCommand command, CancellationToken cancellationToken = default)
        {
            T rdo;

            using (var scope = await ConnectionScope.CreateAsync(command.Connection, cancellationToken))
            {
                rdo = await ExecuteScalarAsync<T>(command, cancellationToken);
            }
            return rdo;
        }

        public static async Task<IDataReader> ExecuteReaderAutoAsync(IDbCommand command, CancellationToken cancellationToken = default)
        {
            return await ExecuteReaderAutoAsync(command, CommandBehavior.Default, cancellationToken);
        }

        public static async Task<IDataReader> ExecuteReaderAutoAsync(IDbCommand command, CommandBehavior behavior, CancellationToken cancellationToken = default)
        {
            IDataReader rdo;
            ConnectionState connectionState = await ConnectionHelper.OpenWithCheckAsync(command.Connection, cancellationToken);

            if (connectionState == ConnectionState.Closed && (behavior & CommandBehavior.CloseConnection) != CommandBehavior.CloseConnection)
            {
                behavior = behavior | CommandBehavior.CloseConnection; // Si la conexión estaba originalmente cerrada, cerrarla otra vez tras terminar el DataReader.
            }

            var asyncCommand = command as System.Data.Common.DbCommand;
            if (asyncCommand != null)
            {
                rdo = await asyncCommand.ExecuteReaderAsync(behavior, cancellationToken);
            }
            else
            {
                rdo = command.ExecuteReader(behavior);
            }
            return rdo;
        }

        public static async Task<DataTable> ExecuteDataTableAutoAsync(IDbCommand command, CancellationToken cancellationToken = default)
        {
            return await ExecuteDataTableAutoAsync(command, LoadOption.PreserveChanges, cancellationToken);
        }

        public static async Task<DataTable> ExecuteDataTableAutoAsync(IDbCommand command, LoadOption loadOption, CancellationToken cancellationToken = default)
        {
            var table = new DataTable();

            await ExecuteDataTableAutoAsync(command, table, loadOption, cancellationToken);
            return table;
        }

        public static async Task ExecuteDataTableAutoAsync(IDbCommand command, DataTable table, LoadOption loadOption, CancellationToken cancellationToken = default)
        {
            try
            {
                table.BeginLoadData();
                using (var reader = await ExecuteReaderAutoAsync(command, cancellationToken))
                {
                    table.Load(reader, loadOption);
                }
            }
            finally
            {
                table.EndLoadData();
            }
        }
    }

}

#endif