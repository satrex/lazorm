using System;
namespace Utilities
{
    public static class StringExpander
    {
        private const string NULLABLE = "System.Nullable<";
        public static string ToCamelCase(this string str)
        {
            return str.Split(new[] { '_', '-', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => char.ToLowerInvariant(s[0]) + s.Substring(1, s.Length - 1)).Aggregate(string.Empty, (s1, s2) => s1 + s2);

        }
        public static string ToPascalCase(this string str)
        {
            return str.Split(new[] { '_', '-', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1)).Aggregate(string.Empty, (s1, s2) => s1 + s2);

        }
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
<<<<<<< HEAD

=======
}
>>>>>>> 0a0e9422dfe1fcb53900b609a0d93d613ff3b010

