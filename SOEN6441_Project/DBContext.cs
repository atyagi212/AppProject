using System;
using MySqlConnector;
using System.Data;
using System.Reflection;
using System.Net.NetworkInformation;

namespace SOEN6441_Project
{
    public class DBContext
    {
        private static DBContext instance = null;
        public static string connectionString = string.Empty;

        private DBContext(IConfiguration config)
        {
            connectionString = config.GetValue<string>("ConnectionStrings:myconn");
        }

        public static DBContext getInstance(IConfiguration config)
        {
            if (instance == null)
            {
                instance = new DBContext(config);
            }
            return instance;
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
            }
            return false;
        }

        public bool DeleteAllCollection<T>(T type)
        {
            MySqlConnection conn = OpenConnection();
            if (conn != null)
            {
                try
                {
                    string sql = GenerateDeleteAllQuery<T>(type);
                    bool result = ExecuteNonQuery(conn, sql);
                    conn.Close();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return false;
        }

        public IEnumerable<DataRow> SelectCollection<T>(T type, List<string> parameteres)
        {
            MySqlConnection conn = OpenConnection();
            if (conn != null)
            {
                try
                {
                    string sql = GenerateSelectQuery<T>(type, parameteres);
                    DataTable dt = ExecuteReader(conn, sql);
                    conn.Close();
                    return dt.AsEnumerable();
                }
                catch (Exception ex)
                {
                    throw ex;
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
            }
            return null;
        }

        public bool InsertCollection<T>(T type)
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
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
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

        private DataTable ExecuteReader(MySqlConnection conn, string sql)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                DataTable datatable = new DataTable();
                datatable.Load(rdr);
                rdr.Close();
                return datatable;
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

        private string ExecuteScalar(MySqlConnection conn, string sql)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                object result = cmd.ExecuteScalar();
                conn.Clone();
                if (result != null)
                {
                    return Convert.ToString(result);
                }
                else
                    return string.Empty;
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
                    if (!Enum.IsDefined(typeof(Constants.SubTables), property.Name.ToUpper()))
                    {
                        paramNames = paramNames + property.Name + ", ";
                        if (property.PropertyType == typeof(String))
                            paramValues = paramValues + "'" + property.GetValue(type) + "', ";
                        else if (property.PropertyType == typeof(DateTime))
                            paramValues = paramValues + "'" + Convert.ToDateTime(property.GetValue(type)).ToString("yyyy-MM-dd HH:mm:ss") + "', ";
                        else
                            paramValues = paramValues + property.GetValue(type) + ", ";
                    }
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
                    if (!Enum.IsDefined(typeof(Constants.SubTables), property.Name.ToUpper()))
                    {
                        if (property.PropertyType == typeof(String))
                            updateParams = updateParams + property.Name + " = '" + property.GetValue(type) + "', ";
                        else if (property.PropertyType == typeof(DateTime))
                            updateParams = updateParams + property.Name + " = '" + Convert.ToDateTime(property.GetValue(type)).ToString("yyyy-MM-dd HH:mm:ss") + "', ";
                        else
                            updateParams = updateParams + property.Name + " = " + property.GetValue(type) + ", ";
                    }
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

        private string GenerateSelectQuery<T>(T type, List<string> parameters)
        {
            Type t = type.GetType();
            string tableName = t.Name;
            string sqlParams = string.Empty;
            string sql = string.Empty;

            PropertyInfo[] propertyInfos = t.GetProperties();

            foreach (var item in parameters)
            {
                if (string.IsNullOrEmpty(sqlParams))
                    sqlParams = item + " = '" + propertyInfos.Where(x => x.Name == item).FirstOrDefault().GetValue(type) + "'";
                else
                    sqlParams = sqlParams + " and " + item + " = '" + propertyInfos.Where(x => x.Name == item).FirstOrDefault().GetValue(type) + "'";
            }

            if (!string.IsNullOrEmpty(sqlParams))
                sql = "SELECT * FROM " + tableName + " WHERE " + sqlParams + ";";
            else
                sql = "SELECT * FROM " + tableName;

            return sql;
        }

        private string GenerateSelectAllQuery<T>(T type)
        {
            Type t = type.GetType();
            string tableName = t.Name;

            string sql = "SELECT * FROM " + tableName + ";";
            return sql;
        }

        private string GenerateDeleteAllQuery<T>(T type)
        {
            Type t = type.GetType();
            string tableName = t.Name;

            string sql = "DELETE FROM " + tableName + ";";
            return sql;
        }

    }
}

