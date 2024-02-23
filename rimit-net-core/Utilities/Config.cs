using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RimitNetCore.Utilities
{
    public class Config
    {
        // FOR MORE INFO CHECK THE DOCUMENT - https://developer.rimit.in/getting-started/readme
        public const string BASE_URL = "https://uat-gateway.rimit.in/api/client/rimit/v1";  // FOR UAT API
        // public const string BASE_URL: "https://api-gateway.rimit.in/api/client/rimit/v1", // FOR PRODUCTION API

        // FOR MORE INFO CHECK THE DOCUMENT - https://developer.rimit.in/getting-started/readme#multi-tenant
        public const string IS_MULTY_TENANT_PLATFORM = "NO"; // OPTIONS - YES/NO
        public const string MULTY_TENANT_MODE = "QUERY"; // OPTIONS - QUERY/PARAMS
    }
}
