#if NO_ASYNC // EXCLUDE.
#else

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace qckdev.Data
{
    static partial class DataReaderHelper
    {

        public static async Task<bool> ReadAsync(IDataReader reader, CancellationToken cancellationToken = default)
        {
            bool result;

            if (reader is System.Data.Common.DbDataReader readerAsync)
                result = await readerAsync.ReadAsync(cancellationToken);
            else
                result = reader.Read();
            return result;
        }

    }
}

#endif