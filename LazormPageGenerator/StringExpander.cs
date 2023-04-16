using System;
namespace LazormPageGenerator
{
	public static class StringExpander
	{
        private const string NULLABLE = "Nullable<";

        public static string Capitalize(this string str)
        {
            if (str.Length == 0)
                return string.Empty;
            else if (str.Length == 1)
                return char.ToUpper(str[0]).ToString();
            else
                return char.ToUpper(str[0]) + str.Substring(1);
        }
        
        public static string Uncapitalize(this string str)
        {
            if (str.Length == 0)
                return string.Empty;
            else if (str.Length == 1)
                return char.ToLower(str[0]).ToString();
            else
                return char.ToLower(str[0]) + str.Substring(1);
        }

        public static string RemoveNullable(this string str)
        {
            if (str.Length == 0)
                return string.Empty;
            else if (str.StartsWith(NULLABLE))
                return str.Replace(NULLABLE, string.Empty).Replace(">", string.Empty);
            else
                return str;
        }
    }
}

