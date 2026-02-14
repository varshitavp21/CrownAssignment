using EmployeeModels.Models;
using EmployeeService.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.Services
{
    public class DatabaseHandler : IDatabaseHandler
    {
        private readonly string connectionString;

        public DatabaseHandler(IOptions<AppSetting> configuration)
        {
            connectionString = configuration.Value.ConnectionString.Connection;
        }

        //public DatabaseHandler(IOptions<AppSetting> configuration)
        //{
        //    connectionString = configuration.Value.ConnectionString.Connection;
        //}

        //public void CloseConnection(DbConnection dbConnection)
        //{
        //    if (dbConnection != null)
        //    {
        //        dbConnection.Close();
        //        dbConnection.Dispose();
        //    }
        //}

        //public IDataAdapter CreateAdapter(DbCommand dbCommand)
        //{
        //    return new SqlDataAdapter((SqlCommand)dbCommand);
        //}

        //public DbCommand CreateCommand(string commandText, DbConnection connection)
        //{
        //    return new SqlCommand
        //    {
        //        CommandText = commandText,
        //        CommandTimeout = 3600,
        //        Connection = (SqlConnection)connection,
        //        CommandType = CommandType.StoredProcedure
        //    };
        //}

        //public DbConnection CreateConnection()
        //{
        //    return new SqlConnection(connectionString);
        //}

        //public IDbDataParameter CreateParameter(DbCommand command)
        //{
        //    throw new NotImplementedException();
        //}

        public DbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }

        public async Task<DbConnection> GetOpenConnectionAsync()
        {
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public DbCommand CreateCommand(string commandText, DbConnection connection)
        {
            return new SqlCommand
            {
                CommandText = commandText,
                CommandTimeout = 3600,
                Connection = (SqlConnection)connection,
                CommandType = CommandType.StoredProcedure
            };
        }

        public IDataAdapter CreateAdapter(DbCommand dbCommand)
        {
            return new SqlDataAdapter((SqlCommand)dbCommand);
        }

        public IDbDataParameter CreateParameter(DbCommand command)
        {
            return command.CreateParameter();
        }

        public void CloseConnection(DbConnection dbConnection)
        {
            if (dbConnection?.State == ConnectionState.Open)
            {
                dbConnection.Close();
            }
            dbConnection?.Dispose();
        }
    }
}
