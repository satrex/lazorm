namespace LazormFluxorGenerator
{
    public static class StringExpander
	{
        public static string Capitalize(this string str)
        {
            return Utilities.StringExpander.Capitalize(str);
        }

        public static string Uncapitalize(this string str)
        {
            return Utilities.StringExpander.Uncapitalize(str);
        }

        public static string RemoveNullable(this string str)
        {
            return Utilities.StringExpander.RemoveNullable(str);
        }

        public static string ToCamelCase(this string str)
        {
            return Utilities.StringExpander.ToCamelCase(str);
        }

        public static string ToPascalCase(this string str)
        {
            return Utilities.StringExpander.ToPascalCase(str);
        }
    }
    }
}

