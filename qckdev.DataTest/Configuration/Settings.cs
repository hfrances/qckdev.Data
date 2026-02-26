using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qckdev.DataTest.Configuration
{
    public sealed class Settings
    {

        public sealed class ConnectionStringSettngs
        {
            public string TestConnection { get; set; }
            public string SqlServer { get; set; }
            public string Sqlite { get; set; }
        }

        public sealed class TestSettings
        {
            public bool RunSqlServer { get; set; }
            public bool RunSqlite { get; set; }
        }

        public ConnectionStringSettngs ConnectionStrings { get; set; }
        public TestSettings Tests { get; set; }

    }
}
