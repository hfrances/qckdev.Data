#if NO_ASYNC // EXCLUDE.
#else

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace qckdev.Data
{
    static partial class ConnectionHelper
    {

        public static async Task<ConnectionState> OpenWithCheckAsync(IDbConnection connection, CancellationToken cancellationToken = default)
        {
            ConnectionState initialState = connection.State;

            if (initialState == ConnectionState.Closed)
                if (connection is System.Data.Common.DbConnection connectionAsync)
                   await connectionAsync.OpenAsync(cancellationToken);
                else
                    connection.Open();
            return initialState;
        }

    }
}

#endif