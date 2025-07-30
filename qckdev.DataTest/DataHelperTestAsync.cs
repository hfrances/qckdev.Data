using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using qckdev.Data;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace qckdev.DataTest
{
    [TestClass]
    public class DataHelperTestAsync
    {

        #region command auto

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public async Task ExecuteNonQueryAutoAsyncTest(bool openBeforeStart)
        {

            using (var conn = CreateConnection())
            {
                if (openBeforeStart)
                    await conn.OpenAsync();

                using (var comm = conn.CreateCommand())
                {
                    int rdo;

                    comm.CommandText = "sp_who2";
                    comm.CommandType = System.Data.CommandType.StoredProcedure;
                    rdo = await comm.ExecuteNonQueryAutoAsync();
                    Assert.AreNotEqual(0, rdo);
                }

                if (openBeforeStart)
                    Assert.AreEqual(System.Data.ConnectionState.Open, conn.State);
                else
                    Assert.AreEqual(System.Data.ConnectionState.Closed, conn.State);
            }
        }

        [TestMethod]
        [DataRow(false, "hola mundo", "hola mundo")]
        [DataRow(false, "", "")]
        [DataRow(false, AssertExt.DBNullCONST, null)]
        [DataRow(true, AssertExt.DBNullCONST, null)]
        public async Task ExecuteScalarAutoAsyncTest(bool openBeforeStart, object expected, string value)
        {

            using (var conn = CreateConnection())
            {
                if (openBeforeStart)
                    await conn.OpenAsync();

                using (var comm = conn.CreateCommand())
                {
                    object rdo;

                    comm.CommandText = $"SELECT @param";
                    comm.Parameters.AddWithValue("@param", (object)value ?? DBNull.Value);
                    rdo = await comm.ExecuteScalarAutoAsync();
                    AssertExt.AreEqualDBNull(expected, rdo);
                }

                if (openBeforeStart)
                    Assert.AreEqual(System.Data.ConnectionState.Open, conn.State);
                else
                    Assert.AreEqual(System.Data.ConnectionState.Closed, conn.State);
            }
        }

        [TestMethod]
        [DataRow(false, "hola mundo", "hola mundo")]
        [DataRow(false, "", "")]
        [DataRow(false, null, null)]
        [DataRow(true, null, null)]
        public async Task ExecuteScalarAutoAsyncTTest(bool openBeforeStart, object expected, string value)
        {

            using (var conn = CreateConnection())
            {
                if (openBeforeStart)
                    await conn.OpenAsync();

                using (var comm = conn.CreateCommand())
                {
                    string rdo;

                    comm.CommandText = $"SELECT @param";
                    comm.Parameters.AddWithValue("@param", (object)value ?? DBNull.Value);
                    rdo = await comm.ExecuteScalarAutoAsync<string>();
                    AssertExt.AreEqualDBNull(expected, rdo);
                }

                if (openBeforeStart)
                    Assert.AreEqual(System.Data.ConnectionState.Open, conn.State);
                else
                    Assert.AreEqual(System.Data.ConnectionState.Closed, conn.State);
            }
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public async Task ExecuteReaderAutoAsyncTest(bool openBeforeStart)
        {

            using (var conn = CreateConnection())
            {
                bool hasResults = false;

                if (openBeforeStart)
                    await conn.OpenAsync();

                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = $"SELECT * FROM syslanguages";
                    using (var reader = await comm.ExecuteReaderAutoAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            hasResults = true;
                        }
                    }
                    Assert.IsTrue(hasResults, "No rows");
                }

                if (openBeforeStart)
                    Assert.AreEqual(System.Data.ConnectionState.Open, conn.State);
                else
                    Assert.AreEqual(System.Data.ConnectionState.Closed, conn.State);
            }
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public async Task ExecuteDataTableAutoAsyncTest(bool openBeforeStart)
        {

            using (var conn = CreateConnection())
            {
                if (openBeforeStart)
                    await conn.OpenAsync();

                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = $"SELECT * FROM syslanguages";
                    using (var dataTable = await comm.ExecuteDataTableAutoAsync())
                    {
                        Assert.IsTrue(dataTable.Columns.OfType<DataColumn>().Any(), "No columns were loaded.");
                        Assert.IsTrue(dataTable.Rows.OfType<DataRow>().Any(), "No rows were loaded");
                    }
                }

                if (openBeforeStart)
                    Assert.AreEqual(System.Data.ConnectionState.Open, conn.State);
                else
                    Assert.AreEqual(System.Data.ConnectionState.Closed, conn.State);
            }
        }

        #endregion


        #region create parameter

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
            {
                using (var comm = conn.CreateCommand())
                {
                    object rdo = null;

                    comm.CommandText = "SELECT @param";
                    comm.Parameters.Add(comm.CreateParameterWithValue("@param", parameterValue));

                    for (int i = 0; i < 1000; i++) // Repetir varias veces para comprobar eficiencia.
                    {
                        if (castResult)
                            rdo = comm.ExecuteScalarAuto<T>();
                        else
                            rdo = comm.ExecuteScalarAuto();
                    }
                    AssertExt.AreEqualDBNull(expectedResult, rdo);
                }
            }
        }


        #endregion


        #region utils

        static readonly string _CONNSTRING;

        static DataHelperTestAsync()
        {
            var settings = Configuration.ConfigurationHelper.GetSettings();
            
            _CONNSTRING = settings.ConnectionStrings.TestConnection;
            var options = new DbContextOptionsBuilder<TestDbContext>()
                    .UseSqlServer(_CONNSTRING)
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

#if NETCOREAPP
        static Microsoft.Data.SqlClient.SqlConnection CreateConnection()
            => new Microsoft.Data.SqlClient.SqlConnection(_CONNSTRING);
#else
        static System.Data.SqlClient.SqlConnection CreateConnection()
            => new System.Data.SqlClient.SqlConnection(_CONNSTRING);
#endif
        #endregion

    }
}
