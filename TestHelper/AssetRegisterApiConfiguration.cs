using System;
using System.Collections.Generic;
using System.Text;

namespace TestHelper
{
    public class AssetRegisterApiConfiguration
    {
        public ConnectionStringConfiguration ConnectionStrings { get; set; }

        public class ConnectionStringConfiguration
        {
            public string AssetRegisterApiDb { get; set; }
        }
    }
}
