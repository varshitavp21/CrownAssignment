using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.Interface
{
    public interface IParamManager
    {
        IDbDataParameter CreateOutputParameter(string name, SqlDbType sqlDbType, int size);
        IDbDataParameter CreateParameter(string name, object value, DbType dbType, ParameterDirection parameterDirection = ParameterDirection.Input);
        IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection parameterDirection = ParameterDirection.Input);
        IDbDataParameter CreateParameter(string name, object value, SqlDbType sqlDbType, ParameterDirection parameterDirection = ParameterDirection.Input);
    }
}
