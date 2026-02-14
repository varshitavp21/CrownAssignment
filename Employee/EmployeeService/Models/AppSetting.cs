using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeModels.Models
{
    public class AppSetting
    {
        public ConnectionString ConnectionString { get; set; }

        public Jwt Jwt { get; set; }
    }

    public class ConnectionString
    {
        public string Connection { get; set; }
        public string LogTableName { get; set; }

        public string StorageAccountConnection { get; set; }
        public string StorageAccountName { get; set; }
        public string BlobContainerName { get; set; }
        public string ModuleName { get; set; }



    }

    public class Jwt
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public int ClockSkew { get; set; }
        public int ExpiresIn { get; set; }
    }
}
