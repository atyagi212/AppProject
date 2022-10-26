using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using ManifestInformation.Entities.Output;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Newtonsoft.Json.Linq;

namespace ManifestInformation
{
    public class ManifestDbContext
    {
        public static string connectionString = string.Empty;
        public ManifestDbContext(IConfiguration config)
        {
            connectionString = config.GetValue<string>("ConnectionStrings:myconn");
        }

        public bool DeleteCollection<T>(T type)
        {
            MySqlConnection conn = OpenConnection();
            if (conn != null)
            {
                try
                {
                    string sql = GenerateDeleteQuery<T>(type);
                    bool result = ExecuteNonQuery(conn, sql);
                    conn.Close();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
            return false;
        }

        public IEnumerable<DataRow> SelectCollection<T>(T type)
        {
            MySqlConnection conn = OpenConnection();
            if (conn != null)
            {
                try
                {
                    string sql = GenerateSelectAllQuery<T>(type);
                    DataTable dt = ExecuteReader(conn, sql);
                    conn.Close();
                    return dt.AsEnumerable();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
            return null;
        }

        public IEnumerable<DataRow> SelectAllCollection<T>(T type)
        {
            MySqlConnection conn = OpenConnection();
            if (conn != null)
            {
                try
                {
                    string sql = GenerateSelectAllQuery<T>(type);
                    DataTable dt = ExecuteReader(conn, sql);
                    conn.Close();
                    return dt.AsEnumerable();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
            return null;
        }

        public bool InsetCollection<T>(T type)
        {
            MySqlConnection conn = OpenConnection();
            if (conn != null)
            {
                try
                {
                    string sql = GenerateInsertQuery<T>(type);
                    bool result = ExecuteNonQuery(conn, sql);
                    conn.Close();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
            return false;
        }

        public bool UpdateCollection<T>(T type)
        {
            MySqlConnection conn = OpenConnection();
            if (conn != null)
            {
                try
                {
                    string sql = GenerateUpdateQuery<T>(type);
                    bool result = ExecuteNonQuery(conn, sql);
                    conn.Close();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
            return false;
        }


        private MySqlConnection OpenConnection()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            if (conn != null)
                return conn;
            else
                return null;
        }

        private bool ExecuteNonQuery(MySqlConnection conn, string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            return true;
        }

        private DataTable ExecuteReader(MySqlConnection conn, string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            DataTable datatable = new DataTable();
            datatable.Load(rdr);
            rdr.Close();
            return datatable;
        }

        private string ExecuteScalar(MySqlConnection conn, string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                return Convert.ToString(result);
            }
            else
                return string.Empty;
        }

        private string GenerateInsertQuery<T>(T type)
        {
            Type t = type.GetType();
            string tableName = t.Name;

            PropertyInfo[] propertyInfos = t.GetProperties();
            string paramNames = string.Empty;
            string paramValues = string.Empty;

            foreach (var property in propertyInfos)
            {
                if (property.GetValue(type) != null)
                {
                    paramNames = paramNames + property.Name + ", ";
                    Type propType = property.GetType();
                    if (propType == typeof(String) || propType == typeof(DateTime))
                        paramValues = paramValues + "'" + property.GetValue(type) + "', ";
                    else
                        paramValues = paramValues + property.GetValue(type) + ", ";
                }
            }

            string sql = "INSERT INTO " + tableName + "(" + paramNames.Trim().TrimEnd(',') + ") Values (" + paramValues.Trim().TrimEnd(',') + ");";
            return sql;
        }

        private string GenerateUpdateQuery<T>(T type)
        {
            Type t = type.GetType();
            string tableName = t.Name;

            PropertyInfo[] propertyInfos = t.GetProperties();
            string updateParams = string.Empty;

            foreach (var property in propertyInfos)
            {
                if (property.GetValue(type) != null)
                {
                    Type propType = property.GetType();
                    if (propType == typeof(String) || propType == typeof(DateTime))
                        updateParams = updateParams + property.Name + " = '" + property.GetValue(type) + "', ";
                    else
                        updateParams = updateParams + property.Name + " = " + property.GetValue(type) + ", ";
                }
            }

            string sql = "UPDATE " + tableName + "SET " + updateParams.Trim().TrimEnd(',') + " WHERE " + propertyInfos[0].Name + " = " + propertyInfos[0].GetValue(type) + ";";
            return sql;
        }

        private string GenerateDeleteQuery<T>(T type)
        {
            Type t = type.GetType();
            string tableName = t.Name;

            PropertyInfo[] propertyInfos = t.GetProperties();

            string sql = "DELETE FROM " + tableName + " WHERE " + propertyInfos[0].Name + " = " + propertyInfos[0].GetValue(type) + ";";
            return sql;
        }

        private string GenerateSelectQuery<T>(T type)
        {
            Type t = type.GetType();
            string tableName = t.Name;

            PropertyInfo[] propertyInfos = t.GetProperties();

            string sql = "SELECT * FROM " + tableName + " WHERE " + propertyInfos[0].Name + " = " + propertyInfos[0].GetValue(type) + ";";
            return sql;
        }

        private string GenerateSelectAllQuery<T>(T type)
        {
            Type t = type.GetType();
            string tableName = t.Name;

            PropertyInfo[] propertyInfos = t.GetProperties();

            string sql = "SELECT * FROM " + tableName + ";";
            return sql;
        }
    }
}

