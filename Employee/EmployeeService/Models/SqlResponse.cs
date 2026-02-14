using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.Models
{
    public class SqlResponse
    {
        public dynamic Response { get; set; }
        public SqlResponseStatus Status { get; set; }

        public HttpStatusCode statusCode { get; set; }
        public string Message { get; set; }

        private readonly string _dateTimeFormat = "yyyy-MM-dd HH:mm:ss";

    }

    public enum SqlResponseStatus
    {
        Success = 200,
        Failure = 400,
    }
}
