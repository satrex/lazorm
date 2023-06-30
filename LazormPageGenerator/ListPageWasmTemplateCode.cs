using System;
using System.Diagnostics;

namespace LazormPageGenerator
{
    public partial class ListPageWasmTemplate
    {
        public string entityClassNameSingular;
        public string entityClassNamePlural;
        public string entityNameSingular;
        public string entityNamePlural;
        public string pageNamespace; 
        public List<EntityProperty> entityProperties;

        public ListPageWasmTemplate(GeneratorContext context)
        {
            entityClassNameSingular = context.EntityClassName;
            entityClassNamePlural = context.EntityClassNamePlural;
            entityNameSingular = entityClassNameSingular.Uncapitalize();
            entityNamePlural = entityClassNamePlural.Uncapitalize();
            entityProperties = context.EntityProperties;
            pageNamespace = context.Namespace;
        }


        public string CreateTableHeader()
        {
            var th = string.Empty;
            entityProperties.ForEach(p =>
            {
                var str = $"{nameof(p.Name)}:{p.Name}, {nameof(p.Editable)}:{p.Editable}, {nameof(p.TypeName)}:{p.TypeName}";
                Trace.WriteLine(str);
                th += $"            <th scope=\"col\">{p.Name}</th>\n";
            });
            return th;
        }

        public string CreateTableBody() 
        {
            var td = string.Empty;
            entityProperties.ForEach(p =>
            {
                var str = $"{nameof(p.Name)}:{p.Name}, {nameof(p.Editable)}:{p.Editable}, {nameof(p.TypeName)}:{p.TypeName}";
                Trace.WriteLine(str);
                if (p.Name == "Name" || p.Name == "Title" || p.Name == "Id")
                {
                    td += $"                <td>";
                    td += $"<a href=/{ entityNamePlural }/show/@{entityNameSingular}.Id >";
                    td += $"@{entityNameSingular}.{p.Name} </a> </td>\n";
                }
                else
                    td += $"                <td>@{entityNameSingular}.{p.Name}</td>\n";
            });
            return td;
        }


    }
}
