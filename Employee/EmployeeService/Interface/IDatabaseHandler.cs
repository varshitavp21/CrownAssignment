using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.Interface
{
    public interface IDatabaseHandler
    {
        DbConnection CreateConnection();
        void CloseConnection(DbConnection dbConnection);
        Task<DbConnection> GetOpenConnectionAsync();
        DbCommand CreateCommand(string commandText, DbConnection connection);
        IDataAdapter CreateAdapter(DbCommand dbCommand);
        IDbDataParameter CreateParameter(DbCommand command);
    }
}
