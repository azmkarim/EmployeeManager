using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.ApiClient.Security
{
    public class JwtInfo
    {
        public string token { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string FullName { get; set; }
    }
}
