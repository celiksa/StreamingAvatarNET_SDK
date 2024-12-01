using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HeyGen.StreamingAvatar
{
    internal static class StringExtensions
    {
        public static string ToSnakeCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) 
                ? "_" + x.ToString() 
                : x.ToString())).ToLower();
        }

        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var words = str.Split(new[] { '_', ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
            var result = words[0].ToLower();
            
            for (int i = 1; i < words.Length; i++)
            {
                result += char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }

            return result;
        }

        public static bool IsValidJson(this string str)
        {
            try
            {
                System.Text.Json.JsonDocument.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ToValidFileName(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            return string.Join("_", str.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}