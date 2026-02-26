using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using qckdev.Data;
using qckdev.DataTest.Configuration;
using System;
using System.Data;
using System.Linq;

namespace qckdev.DataTest
{
    public abstract class DataHelperTestBase
    {
        private readonly object _initLock = new object();
        private bool _initialized;
        private string _connectionString;

        protected abstract string ProviderName { get; }
        protected abstract bool IsEnabled(Settings settings);
        protected abstract string ResolveConnectionString(Settings settings);
        protected abstract IDbConnection CreateConnection(string connectionString);
        protected abstract void EnsureDatabaseReady(string connectionString);

        protected virtual string NonQueryText => "UPDATE Entities SET Name = Name";
        protected virtual string QueryAllText => "SELECT * FROM Entities";
        protected virtual string QueryParameterText => "SELECT @param";

        [TestInitialize]
        public void TestInitialize()
        {
            var settings = ConfigurationHelper.GetSettings();
            if (!IsEnabled(settings))
            {
                Assert.Inconclusive($"{ProviderName} tests disabled by configuration.");
            }

            EnsureInitialized(settings);
        }

        protected IDbConnection CreateConnection()
        {
            return CreateConnection(_connectionString);
        }

        private void EnsureInitialized(Settings settings)
        {
            if (_initialized)
            {
                return;
            }

            lock (_initLock)
            {
                if (_initialized)
                {
                    return;
                }

                _connectionString = ResolveConnectionString(settings);
                if (string.IsNullOrWhiteSpace(_connectionString))
                {
                    Assert.Inconclusive($"{ProviderName} connection string is missing.");
                }

                EnsureDatabaseReady(_connectionString);
                _initialized = true;
            }
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void ExecuteNonQueryAutoTest(bool openBeforeStart)
        {
            using (var conn = CreateConnection())
            {
                if (openBeforeStart)
                    conn.Open();

                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = NonQueryText;
                    comm.CommandType = CommandType.Text;
                    var rdo = comm.ExecuteNonQueryAuto();
                    Assert.IsTrue(rdo > 0, "No rows were affected.");
                }

                Assert.AreEqual(openBeforeStart ? ConnectionState.Open : ConnectionState.Closed, conn.State);
            }
        }

        [TestMethod]
        [DataRow(false, "hola mundo", "hola mundo")]
        [DataRow(false, "", "")]
        [DataRow(false, AssertExt.DBNullCONST, null)]
        [DataRow(true, AssertExt.DBNullCONST, null)]
        public void ExecuteScalarAutoTest(bool openBeforeStart, object expected, string value)
        {
            using (var conn = CreateConnection())
            {
                if (openBeforeStart)
                    conn.Open();

                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = QueryParameterText;
                    comm.Parameters.Add(comm.CreateParameterWithValue("@param", value));
                    var rdo = comm.ExecuteScalarAuto();
                    AssertExt.AreEqualDBNull(expected, rdo);
                }

                Assert.AreEqual(openBeforeStart ? ConnectionState.Open : ConnectionState.Closed, conn.State);
            }
        }

        [TestMethod]
        [DataRow(false, "hola mundo", "hola mundo")]
        [DataRow(false, "", "")]
        [DataRow(false, null, null)]
        [DataRow(true, null, null)]
        public void ExecuteScalarAutoTTest(bool openBeforeStart, object expected, string value)
        {
            using (var conn = CreateConnection())
            {
                if (openBeforeStart)
                    conn.Open();

                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = QueryParameterText;
                    comm.Parameters.Add(comm.CreateParameterWithValue("@param", value));
                    var rdo = comm.ExecuteScalarAuto<string>();
                    AssertExt.AreEqualDBNull(expected, rdo);
                }

                Assert.AreEqual(openBeforeStart ? ConnectionState.Open : ConnectionState.Closed, conn.State);
            }
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void ExecuteReaderAutoTest(bool openBeforeStart)
        {
            using (var conn = CreateConnection())
            {
                bool hasResults = false;

                if (openBeforeStart)
                    conn.Open();

                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = QueryAllText;
                    using (var reader = comm.ExecuteReaderAuto())
                    {
                        while (reader.Read())
                        {
                            hasResults = true;
                        }
                    }
                }

                Assert.IsTrue(hasResults, "No rows");
                Assert.AreEqual(openBeforeStart ? ConnectionState.Open : ConnectionState.Closed, conn.State);
            }
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void ExecuteDataTableAutoTest(bool openBeforeStart)
        {
            using (var conn = CreateConnection())
            {
                if (openBeforeStart)
                    conn.Open();

                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = QueryAllText;
                    using (var dataTable = comm.ExecuteDataTableAuto())
                    {
                        Assert.IsTrue(dataTable.Columns.OfType<DataColumn>().Any(), "No columns were loaded.");
                        Assert.IsTrue(dataTable.Rows.OfType<DataRow>().Any(), "No rows were loaded");
                    }
                }

                Assert.AreEqual(openBeforeStart ? ConnectionState.Open : ConnectionState.Closed, conn.State);
            }
        }

        [TestMethod]
        [DataRow(true, "hello world", "hello world")]
        [DataRow(false, "hello world", "hello world")]
        [DataRow(true, null, null)]
        [DataRow(false, null, AssertExt.DBNullCONST)]
        public void CreateParameterWithValueTest_String(bool castResult, string parameterValue, object expectedResult)
        {
            CreateParameterWithValueTest<string>(castResult, parameterValue, expectedResult);
        }

        [TestMethod]
        [DataRow(true, 1, 1)]
        [DataRow(false, 1, 1)]
        [DataRow(true, 0, 0)]
        [DataRow(false, 0, 0)]
        public void CreateParameterWithValueTest_Integer(bool castResult, int parameterValue, object expectedResult)
        {
            CreateParameterWithValueTest<int>(castResult, parameterValue, expectedResult);
        }

        [TestMethod]
        [DataRow(true, 1, 1)]
        [DataRow(false, 1, 1)]
        [DataRow(true, 0, 0)]
        [DataRow(false, 0, 0)]
        [DataRow(true, null, null)]
        [DataRow(false, null, AssertExt.DBNullCONST)]
        public void CreateParameterWithValueTest_IntegerNullable(bool castResult, int? parameterValue, object expectedResult)
        {
            CreateParameterWithValueTest<int?>(castResult, parameterValue, expectedResult);
        }

        private void CreateParameterWithValueTest<T>(bool castResult, T parameterValue, object expectedResult)
        {
            using (var conn = CreateConnection())
            using (var comm = conn.CreateCommand())
            {
                object rdo = null;
                comm.CommandText = QueryParameterText;
                comm.Parameters.Add(comm.CreateParameterWithValue("@param", parameterValue));

                for (int i = 0; i < 1000; i++)
                {
                    if (castResult)
                        rdo = comm.ExecuteScalarAuto<T>();
                    else
                        rdo = comm.ExecuteScalarAuto();
                }
                AssertAreEquivalent(expectedResult, rdo);
            }
        }

        private static void AssertAreEquivalent(object expected, object actual)
        {
            if (expected is int expectedInt && actual is long actualLong)
            {
                Assert.AreEqual(Convert.ToInt64(expectedInt), actualLong);
                return;
            }

            AssertExt.AreEqualDBNull(expected, actual);
        }
    }

    [TestClass]
    public sealed class DataHelperSqlServerTest : DataHelperTestBase
    {
        protected override string ProviderName => "SqlServer";
        protected override bool IsEnabled(Settings settings) => settings?.Tests?.RunSqlServer == true;

        protected override string ResolveConnectionString(Settings settings)
            => settings?.ConnectionStrings?.SqlServer ?? settings?.ConnectionStrings?.TestConnection;

        protected override void EnsureDatabaseReady(string connectionString)
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new TestDbContext(options))
            {
                context.Database.EnsureCreated();
                if (!context.Entities.Any())
                {
                    context.Entities.Add(new Entity { Id = 0, Name = "Test", Description = null });
                    context.SaveChanges();
                }
            }
        }

        protected override IDbConnection CreateConnection(string connectionString)
        {
#if NETCOREAPP
            return new Microsoft.Data.SqlClient.SqlConnection(connectionString);
#else
            return new System.Data.SqlClient.SqlConnection(connectionString);
#endif
        }
    }

#if NETCOREAPP
    [TestClass]
    public sealed class DataHelperSqliteTest : DataHelperTestBase
    {
        protected override string ProviderName => "Sqlite";
        protected override bool IsEnabled(Settings settings) => settings?.Tests?.RunSqlite == true;
        protected override string ResolveConnectionString(Settings settings) => settings?.ConnectionStrings?.Sqlite;

        protected override void EnsureDatabaseReady(string connectionString)
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlite(connectionString)
                .Options;

            using (var context = new TestDbContext(options))
            {
                context.Database.EnsureCreated();
                if (!context.Entities.Any())
                {
                    context.Entities.Add(new Entity { Id = 0, Name = "Test", Description = null });
                    context.SaveChanges();
                }
            }
        }

        protected override IDbConnection CreateConnection(string connectionString)
            => new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
    }
#endif
}
