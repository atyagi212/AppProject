using System;
using System.Data;
using Microsoft.AspNetCore.Hosting.Server;
using MySqlConnector;

namespace SOEN6441_Project.Interfaces
{
    public interface IDataMapper
    {
        bool DeleteCollection<T>(T type);

        bool DeleteAllCollection<T>(T type);

        IEnumerable<DataRow> SelectCollection<T>(T type, List<string> parameteres);

        IEnumerable<DataRow> SelectAllCollection<T>(T type);

        bool InsertCollection<T>(T type);

        bool UpdateCollection<T>(T type);

        bool ExecuteNonQuery(MySqlConnection conn, string sql);

        DataTable ExecuteReader(MySqlConnection conn, string sql);

        string ExecuteScalar(MySqlConnection conn, string sql);

        string GenerateInsertQuery<T>(T type);

        string GenerateUpdateQuery<T>(T type);

        string GenerateDeleteQuery<T>(T type);

        string GenerateSelectQuery<T>(T type, List<string> parameters);

        string GenerateSelectAllQuery<T>(T type);

        string GenerateDeleteAllQuery<T>(T type);
    }
}

