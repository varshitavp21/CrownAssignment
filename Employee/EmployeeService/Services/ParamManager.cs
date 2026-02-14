using EmployeeService.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.Services
{
    public class ParamManager : IParamManager
    {
        public IDbDataParameter CreateParameter(string name, object value, DbType dbType, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            var param = new SqlParameter
            {
                DbType = dbType,
                ParameterName = name,
                Direction = parameterDirection,
                Value = value ?? DBNull.Value
            };

            // 👇 Force nvarchar(max) for strings (DbType.String or DbType.AnsiString)
            if (dbType == DbType.String || dbType == DbType.AnsiString)
            {
                param.Size = -1;
            }

            return param;
        }

        public IDbDataParameter CreateOutputParameter(string name, SqlDbType sqlDbType, int size)
        {
            return new SqlParameter
            {
                SqlDbType = sqlDbType,
                ParameterName = name,
                Direction = ParameterDirection.Output,
                Size = size
            };
        }

        public IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            return new SqlParameter
            {
                DbType = dbType,
                Size = size,
                ParameterName = name,
                Direction = parameterDirection,
                Value = value ?? DBNull.Value
            };
        }

        public IDbDataParameter CreateParameter(string name, object value, SqlDbType sqlDbType, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            return new SqlParameter
            {
                SqlDbType = sqlDbType,
                ParameterName = name,
                Direction = parameterDirection,
                Value = value ?? DBNull.Value
            };
        }
    }
}
