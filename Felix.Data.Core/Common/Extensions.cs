using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace Felix.Data.Core.Common
{
    public static class Extensions
    {
        public static List<List<T>> Split<T>(List<T> paramList, int sliceSize)
        {
            var list = new List<List<T>>();
            for (int i = 0; i < paramList.Count; i += sliceSize)
            {
                list.Add(paramList.GetRange(i, Math.Min(sliceSize, paramList.Count - i)));
            }

            return list;
        }

        public static T ConvertTo<T>(this IConvertible obj)
        {
            Type t = typeof(T);
            Type u = Nullable.GetUnderlyingType(t);
            if (u != null)
            {
                return (obj == null) ? default : (T)Convert.ChangeType(obj, u);
            }
            else
            {
                return (T)Convert.ChangeType(obj, t);
            }
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            
            foreach(T item in data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
