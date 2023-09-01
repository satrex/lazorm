using System;
using System.Diagnostics;

namespace LazormPageGenerator
{
    public partial class DetailPageWithFluxorTemplate
    {
        public string entityClassNameSingular;
        public string entityClassNamePlural;
        public string entityNameSingular;
        public string entityNamePlural;
        public string pageNamespace; 
        public List<EntityProperty> entityProperties;

        public DetailPageWithFluxorTemplate(GeneratorContext context)
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
                string input = PageGenerator.GenerateFormInput(p, $"the{ entityClassNameSingular}");
                td += $"       <div class=\"form-group\">\n";
                td += $"            <label for=\"input{p.Name}\">{p.Name}:</label>\n";
                td += $"            {input}\n";
                td += $"       </div>\n";
            });
            return td;

        }
    }
}

