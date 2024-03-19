using System;
using System.Diagnostics;

namespace LazormPageGenerator
{
    public partial class ShowPageWithFluxorTemplate
    {
        public string entityClassNameSingular;
        public string entityClassNamePlural;
        public string entityNameSingular;
        public string entityNamePlural;
        public string pageNamespace;
        public List<EntityProperty> entityProperties;

        public ShowPageWithFluxorTemplate(GeneratorContext context)
        {
            entityClassNameSingular = context.EntityClassName;
            entityClassNamePlural = context.EntityClassNamePlural;
            entityNameSingular = entityClassNameSingular.Uncapitalize();
            entityNamePlural = entityClassNamePlural.Uncapitalize();
            entityProperties = context.EntityProperties;
            pageNamespace = context.Namespace;
        }

        public string CreateFormBody()
        {
            var td = string.Empty;
            entityProperties.ForEach(p =>
            {
                var str = $"{nameof(p.Name)}:{p.Name}, {nameof(p.Editable)}:{p.Editable}, {nameof(p.TypeName)}:{p.TypeName}";
                Trace.WriteLine(str);
                td += $"       <div class=\"form-group\">\n";
                td += $"            <label for=\"{p.Name}\">{p.Name}:</label>\n";
                td += $"            @{entityNamePlural}State?.Value.CurrentEntity.{p.Name}\n";
                td += $"       </div>\n";
            });
            return td;

        }
    }
}
