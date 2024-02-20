using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
namespace Common.Utilities
{
    public static class DataTableVsListOfType
    {
        /// <summary>
        /// Convert DataTable to List of target type
        /// </summary>
        /// <typeparam name="TTarget">Target type </typeparam>
        /// <param name="dataTable">DataTable to convert</param>
        /// <returns>List of target type</returns>
        /// Modified by Gautam Sharma - Logic Change
        public static List<TTarget> ConvertToTargetTypeList<TTarget>(this DataTable dataTable) where TTarget : class, new()
        {
            try
            {
                List<TTarget> list = new List<TTarget>();
                foreach (var row in dataTable.AsEnumerable())
                {
                    TTarget obj = new TTarget();
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Convert list of source type to DataTable
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <param name="data">List of source data type</param>
        /// <returns>Return DataTable</returns>
        public static DataTable ConvertToDataTable<TSource>(this IList<TSource> data)
        {
            DataTable dt = new DataTable(typeof(TSource).Name);
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] props = typeof(TSource).GetProperties(flags);
            foreach (var prop in props)
            {
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (TSource item in data)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }


        public static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static List<T> ConvertDataRowToList<T>(DataRow[] dataRows)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dataRows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            try
            {
                Type temp = typeof(T);
                T obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name.ToLower() == column.ColumnName.ToLower())
                        {
                            var _val = dr[column.ColumnName];
                            if (dr[column.ColumnName] == DBNull.Value)
                            {
                                if (pro.PropertyType == typeof(int) || pro.PropertyType == typeof(double))
                                    _val = 0;
                                else
                                    _val = null;
                            }
                            pro.SetValue(obj, Convert.ChangeType(_val, pro.PropertyType), null);

                        }
                        else
                            continue;
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Author: Gautam Sharma
        /// Convert DataTable to Respective Model
        /// </summary>
        /// <typeparam name="T">Pass the Model Class that needs to parsed.</typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T ConvertDataTableToModel<T>(DataRow dr) where T : class
        {
            return GetRowItem<T>(dr);
        }

        /// <summary>
        /// Author: Gautam Sharma
        /// Convert DataTable to Respective Model
        /// </summary>
        /// <typeparam name="T">Pass the DataRow Class that needs to parsed.</typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static T GetRowItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToLower() == column.ColumnName.ToLower())
                    {
                        var _val = dr[column.ColumnName];
                        if (dr[column.ColumnName] == DBNull.Value)
                        {
                            _val = null;
                        }
                        pro.SetValue(obj, Convert.ChangeType(_val, pro.PropertyType), null);

                    }

                    else
                        continue;
                }
            }
            return obj;
        }

    }
}
