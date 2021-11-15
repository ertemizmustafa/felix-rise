using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Felix.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumarationValue) where T : struct
        {
            var type = enumarationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(enumarationValue)} must be enum type", nameof(enumarationValue));
            }

            var memberInfo = type.GetMember(enumarationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return enumarationValue.ToString();
        }
    }
}
