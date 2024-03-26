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
        public IEnumerable<GeneratorContext> childrenContexts = new List<GeneratorContext>();
        public GeneratorContext mainContext;

        public DetailPageWithFluxorTemplate(GeneratorContext context, IEnumerable<string> children )
        {
            mainContext = context;
            entityClassNameSingular = context.EntityClassName;
            entityClassNamePlural = context.EntityClassNamePlural;
            entityNameSingular = entityClassNameSingular.Uncapitalize();
            entityNamePlural = entityClassNamePlural.Uncapitalize();
            entityProperties = context.EntityProperties;
            pageNamespace = context.Namespace;
            if(children != null)
                childrenContexts = children.Select<string, GeneratorContext>(c => new GeneratorContext(c));
        }

        public string CreateFormBody(GeneratorContext context)
        {
            var td = string.Empty;
            context.EntityProperties.ForEach(p =>
            {
                var str = $"{nameof(p.Name)}:{p.Name}, {nameof(p.Editable)}:{p.Editable}, {nameof(p.TypeName)}:{p.TypeName}";
                Trace.WriteLine(str);
                string input = PageGenerator.GenerateFormInput(p, $"the{context.EntityClassName}");
                td += $"       <div class=\"form-group\">\n";
                td += $"            <label for=\"input{p.Name}\">{p.Name}:</label>\n";
                td += $"            {input}\n";
                td += $"       </div>\n";
            });
            return td;
        }

        public string CreateTableHeader(GeneratorContext context)
        {
            var th = string.Empty;
            context.EntityProperties.ForEach(p =>
            {
                var str = $"{nameof(p.Name)}:{p.Name}, {nameof(p.Editable)}:{p.Editable}, {nameof(p.TypeName)}:{p.TypeName}";
                Trace.WriteLine(str);
                th += $"            <th scope=\"col\">{p.Name}</th>\n";
            });
            return th;
        }

        public string CreateTableBody(GeneratorContext context)
        {
            var td = string.Empty;
            context.EntityProperties.ForEach(p =>
            {
                var str = $"{nameof(p.Name)}:{p.Name}, {nameof(p.Editable)}:{p.Editable}, {nameof(p.TypeName)}:{p.TypeName}";
                Trace.WriteLine(str);
                td += $"                <td>@{context.EntityClassName.Uncapitalize()}.{p.Name}</td>\n";
            });
            return td;
        }

        public string CreateChildrenTables()
        {
            var table = string.Empty;
            foreach(var child in childrenContexts)
            {
                var childClassNameSingular = child.EntityClassName;
                var childClassNamePlural = child.EntityClassNamePlural;
                var childNameSingular = entityClassNameSingular.Uncapitalize();
                var childNamePlural = entityClassNamePlural.Uncapitalize();
                table += $" <{childClassNameSingular}EditTableComponent {childClassNamePlural}=\"the{entityClassNameSingular}.{childClassNamePlural}\"></{childClassNameSingular}EditTableComponent>";
            }

            return table;
        }

    }
}

