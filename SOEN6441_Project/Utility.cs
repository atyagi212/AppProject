using System;
using System.Data;
using System.Reflection;

namespace SOEN6441_Project
{
    public static class Utility
    {

        public static List<T> ConvertDataTableToList<T>(IEnumerable<DataRow> dr)
        {
            DataTable dt = new DataTable();
            List<T> data = new List<T>();
            if (dr.Count() > 0)
            {
                dt = dr.CopyToDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
                return data;
            }
            else
                return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        if (dr[column.ColumnName] is DBNull)
                            pro.SetValue(obj, null, null);
                        else
                            pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}

