using System;
using System.Diagnostics;

namespace LazormPageGenerator
{
	public partial class TableComponentTemplate
	{
        public string entityClassNameSingular;
        public string entityClassNamePlural;
        public string entityNameSingular;
        public string entityNamePlural;
        public string pageNamespace;
        public List<EntityProperty> entityProperties;
        public Mode editMode;
        public enum Mode { 
            Show,
            Edit
	    }

        public TableComponentTemplate(GeneratorContext context, Mode mode = Mode.Show)
        {
            entityClassNameSingular = context.EntityClassName;
            entityClassNamePlural = context.EntityClassNamePlural;
            entityNameSingular = entityClassNameSingular.Uncapitalize();
            entityNamePlural = entityClassNamePlural.Uncapitalize();
            entityProperties = context.EntityProperties;
            pageNamespace = context.Namespace;
            editMode = mode;
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

        public string CreateTableBody(Mode mode)
        {
            if (mode == Mode.Edit) return CreateEditableTableBody();

            var td = string.Empty;
            td = $"        @foreach (var {entityNameSingular} in {entityClassNamePlural} ?? new List<{entityClassNameSingular}>())"
		    +    $"{{\n             <tr @onclick=\"async () => await OnRowClickCallback.InvokeAsync({ entityNameSingular })\">";
            entityProperties.ForEach(p =>
            {
                var str = $"{nameof(p.Name)}:{p.Name}, {nameof(p.Editable)}:{p.Editable}, {nameof(p.TypeName)}:{p.TypeName}";
                Trace.WriteLine(str);
                td += $"                <td>@{entityNameSingular}.{p.Name}</td>\n";
            });
            td += @"    </tr>
		    }";
            return td;
        }

        public string CreateEditableTableBody()
        {
            var td = string.Empty;
            td = $"        @foreach (var {entityNameSingular} in {entityClassNamePlural} ?? new List<{entityClassNameSingular}Validation>())"
		    +    $"{{\n             <tr @onclick=\"async () => await OnRowClickCallback.InvokeAsync({ entityNameSingular })\">";
            entityProperties.ForEach(p =>
            {
                var str = $"{nameof(p.Name)}:{p.Name}, {nameof(p.Editable)}:{p.Editable}, {nameof(p.TypeName)}:{p.TypeName}";
                string input = PageGenerator.GenerateFormInput(p, $"{entityNameSingular}");
                Trace.WriteLine(str);
                td += $"                <td>{input}</td>\n";
            });
            td += @"    </tr>
		    }";
            td += $"<tr><td><a @onclick=\"async () => await OnRowClickCallback.InvokeAsync({ entityNameSingular })\" >+ add row... </a></td></tr>";
            return td;
        }

        public string CreateEntityList(Mode editMode)
        {
            if(editMode == Mode.Show)
            {
                return $"public IEnumerable<{entityClassNameSingular}> {entityClassNamePlural} {{get; set; }} = new {entityClassNameSingular}[0];";
            }
            else if(editMode == Mode.Edit)
            { 
                return $"public IEnumerable<{entityClassNameSingular}Validation> {entityClassNamePlural} {{get; set; }} = new {entityClassNameSingular}Validation[0];";
	        }
            throw new NotImplementedException($"Mode {editMode} is not implemented.");
        }

        public string CreateEntityCallback(Mode editMode)
        { 
            if(editMode == Mode.Show)
            {
                return $"public EventCallback<{ entityClassNameSingular}> OnRowClickCallback {{ get; set; }}";
            }
            else if(editMode == Mode.Edit)
            { 
                return $"public EventCallback<{ entityClassNameSingular}Validation> OnRowClickCallback {{ get; set; }}";
	        }
            throw new NotImplementedException($"Mode {editMode} is not implemented.");
 
	    }
    }
}

