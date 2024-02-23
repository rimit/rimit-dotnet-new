using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RimitNetCore.Utilities
{
    public class Config
    {
        // FOR MORE INFO CHECK THE DOCUMENT - https://doc.rimit.co/getting-started/readme#rest
        public const string BASE_URL = "https://uat-gateway.rimit.in/api/rimit/v1";

        // FOR MORE INFO CHECK THE DOCUMENT - https://doc.rimit.co/getting-started/readme#multi-tenant
        public const string IS_MULTY_TENANT_PLATFORM = "NO"; // OPTIONS - YES/NO
        public const string MULTY_TENANT_MODE = "QUERY"; // OPTIONS - QUERY/PARAMS
    }
}
