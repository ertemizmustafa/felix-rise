using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Felix.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidJson(this string text)
        {
            text = text?.Trim() ?? "";

            if ((text.StartsWith("{") && text.EndsWith("}")) || //For object
               (text.StartsWith("[") && text.EndsWith("]"))) //for array
            {
                try
                {
                    var obj = JToken.Parse(text);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static object ConverToJsonObject(this string text)
        {
            return text.IsValidJson() ? JsonConvert.DeserializeObject(text) : null;
        }

        public static T ConvertToJsonObject<T>(this string text)
        {
            return text.IsValidJson() ? JsonConvert.DeserializeObject<T>(text) : default;
        }

        public static string ToTitleCase(this string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public static Dictionary<string, string> ConvertUsingRegexToDictionary(this string text)
        {
            return text.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(entry => Regex.Match(entry, @"^\s*(?<key>\S+)\s*=\s*(?<value>\S+)\s*$"))
                .Where(match => match.Success)
                .ToDictionary(match => match.Groups["key"].Value, match => match.Groups["value"].Value);
        }
    }
}
