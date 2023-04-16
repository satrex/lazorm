using System;
using System.Diagnostics;

namespace LazormPageGenerator
{
    public partial class DetailPageTemplate
    {
        public string entityClassNameSingular;
        public string entityClassNamePlural;
        public string entityNameSingular;
        public string entityNamePlural;
        public List<EntityProperty> entityProperties;

        public DetailPageTemplate(GeneratorContext context)
        {
            entityClassNameSingular = context.EntityClassName;
            entityClassNamePlural = context.EntityClassNamePlural;
            entityNameSingular = entityClassNameSingular.Uncapitalize();
            entityNamePlural = entityClassNamePlural.Uncapitalize();
            entityProperties = context.EntityProperties;
        }

        public string CreateFormBody()
        {
            var td = string.Empty;
            entityProperties.ForEach(p =>
            {
                var str = $"{nameof(p.Name)}:{p.Name}, {nameof(p.Editable)}:{p.Editable}, {nameof(p.TypeName)}:{p.TypeName}";
                Trace.WriteLine(str);
                string input = string.Empty;
                switch (p.TypeName.RemoveNullable()) {
                    case "int":
                    case "Int32":
                    case "Int64":
                    case "single":
                    case "Single":
                    case "double":
                    case "Double":
                    case "decimal":
                    case "Decimal":
                        input = $"<InputNumber class=\"form-control\" id=\"input{p.Name}\" @bind-value=\"{entityNameSingular}.{p.Name}\" />";;
                        break;
                    case "bool":
                        input = $"<InputCheckbox class=\"form-control\" id=\"input{p.Name}\" @bind-value=\"{entityNameSingular}.{p.Name}\" />";
                        break;
                    default:
                        input = $"<InputText class=\"form-control\" id=\"input{p.Name}\" @bind-value=\"{entityNameSingular}.{p.Name}\" />";
                        break;
                }
                td += $"       <div class=\"form-group\">\n";
                td += $"            <label for=\"input{p.Name}\">{p.Name}:</label>\n";
                td += $"            {input}\n";
                td += $"       </div>\n";
            });
            return td;
        }


    }
}
