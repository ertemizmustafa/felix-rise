using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Felix.Common.Extensions
{
    public static class CommonExtensions
    {
        public static bool HasFileContent(this byte[] input)
        {
            return input != null && input.Length > 0;
        }

        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public static string GetFileExtensions(this string fileName)
        {
            return Path.GetExtension(fileName);
        }

        public static bool ContainItem<T>(this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }

        public static Stream ConvertByteArrayToStream(this byte[] input)
        {
            if (input == null)
            {
                return null;
            }

            return new MemoryStream(input, 0, input.Length);
        }

        public static List<List<T>> Split<T>(List<T> paramList, int sliceSize)
        {
            List<List<T>> list = new List<List<T>>();
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

        public static T TryConvertTo<T>(this IConvertible obj)
        {
            Type t = typeof(T);
            Type u = Nullable.GetUnderlyingType(t);

            try
            {


                if (u != null)
                {
                    return (obj == null) ? default : (T)Convert.ChangeType(obj, u);
                }
                else
                {
                    return (T)Convert.ChangeType(obj, t);
                }
            }
            catch
            {
                return default;
            }
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static string[] Permutations(this IEnumerable<string> source, string seperator)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            return Enumerable
                .Range(0, 1 << (source.Count()))
                .Select(index => string.Join(seperator ?? " ", source.Where((v, i) => (index & (1 << i)) != 0).ToArray()))
                .Where(x => !string.IsNullOrEmpty(x))
                .OrderBy(x => x)
                .ToArray();
        }

    }
}
