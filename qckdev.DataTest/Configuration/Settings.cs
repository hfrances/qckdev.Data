using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qckdev.DataTest.Configuration
{
    sealed class Settings
    {

        public class ConnectionStringSettngs
        {
            public string TestConnection { get; set; }
        }

        public ConnectionStringSettngs ConnectionStrings { get; set; }

    }
}
