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
}
