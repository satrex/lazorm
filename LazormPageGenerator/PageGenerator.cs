using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace LazormPageGenerator
{
    public class PageGenerator
    {
        public static void GeneratePageFromFile(string filePath, string outDir, string pageNamespace, bool withFluxor = false, bool isWasm = false, IEnumerable<string>? children = default(IEnumerable<string>))
        {
            GeneratorContext context = new GeneratorContext(filePath);
            var dir = Directory.CreateDirectory(path: Path.Combine(outDir, context.EntityClassNamePlural));

            #region BlazorWasm
            if (isWasm)
            {
                // creating api controller
                ControllerTemplate controllerTemplate = new ControllerTemplate(context: context);
                string controllerContent = controllerTemplate.TransformText();
                var contDir = Directory.CreateDirectory(path: Path.Combine(Environment.CurrentDirectory, "Controllers"));
                File.WriteAllText(Path.Combine(contDir.FullName, $"{context.EntityClassName}Controller.cs"), controllerContent);
                // creating list page file
                ListPageWasmTemplate listWasmTemplate = new(context: context);
                string listWasmPageContent = listWasmTemplate.TransformText();
                File.WriteAllText(Path.Combine(dir.FullName, "Index.razor"), listWasmPageContent);
                return;
            }
	    #endregion

	        #region BlazorServer
	        #region Fluxor
            if (withFluxor)
            {
                foreach(var child in children ?? new string[0])
                {
                    if (!File.Exists(child)) {
                        Trace.WriteLine($"child file {child} not found. skipping...");
		            }
                    else 
		            {
                        GeneratorContext childContext = new GeneratorContext(child);
                        TableComponentTemplate childTableComponentTemplate = new TableComponentTemplate(childContext);
                        string childTableComponentContent = childTableComponentTemplate.TransformText();
                        File.WriteAllText(Path.Combine(dir.FullName, $"{childContext.EntityClassName}TableComponent.razor"), childTableComponentContent);
                        TableComponentTemplate editableTableComponentTemplate = new TableComponentTemplate(context: childContext, mode: TableComponentTemplate.Mode.Edit);
                        string editableTableComponentContent = editableTableComponentTemplate.TransformText();
                        File.WriteAllText(Path.Combine(dir.FullName, $"{childContext.EntityClassName}EditTableComponent.razor"), editableTableComponentContent);
                    }
                }
                // creating list page file
                ListPageWithFluxorTemplate listFluxorTemplate = new(context: context);
                string listFluxorPageContent = listFluxorTemplate.TransformText();
                File.WriteAllText(Path.Combine(dir.FullName, $"{context.EntityClassNamePlural}Page.razor"), listFluxorPageContent);
                TableComponentTemplate tableComponentTemplate = new TableComponentTemplate(context: context, mode:TableComponentTemplate.Mode.Show);
                string tableComponentContent = tableComponentTemplate.TransformText();
                File.WriteAllText(Path.Combine(dir.FullName, $"{context.EntityClassName}TableComponent.razor"), tableComponentContent);
                //ListPageWithFluxorCsTemplate listFluxorCs = new (context: context);
                //string listFluxorCsContent = listFluxorCs.TransformText();
                //File.WriteAllText(Path.Combine(dir.FullName, $"{context.EntityClassNamePlural}Page.razor.cs"), listFluxorCsContent);

                // creating detail page with fluxor
                DetailPageWithFluxorTemplate detailPageWithFluxor = new DetailPageWithFluxorTemplate(context: context, children ?? new string[0]);
                string detailPageWithFluxorContent = detailPageWithFluxor.TransformText();
                File.WriteAllText(Path.Combine(dir.FullName, $"Edit{context.EntityClassName}Page.razor"), detailPageWithFluxorContent);

                ShowPageWithFluxorTemplate showPageWithFluxor = new ShowPageWithFluxorTemplate(context: context);
                string showPageWithFluxorContent = showPageWithFluxor.TransformText();
                File.WriteAllText(Path.Combine(dir.FullName, $"Show{context.EntityClassName}Page.razor"), showPageWithFluxorContent);

                return;
            }
            #endregion

            // creating list page file
            ListPageTemplate listServerTemplate = new(context: context);
            string listServerPageContent = listServerTemplate.TransformText();
            File.WriteAllText(Path.Combine(dir.FullName, "Index.razor"), listServerPageContent);

            // creating confirmation dialog file
            ConfirmDialogTemplate confirmDialogTemplate = new ConfirmDialogTemplate();
            string ConfirmDialogContent = confirmDialogTemplate.TransformText();

            // creating detail page file
            DetailPageTemplate detailPageTemplate = new(context: context);
            string detailPageContent = detailPageTemplate.TransformText();
            File.WriteAllText(Path.Combine(dir.FullName, $"Edit{context.EntityClassName}Page.razor"), detailPageContent);

     #endregion 


        }
        public static string GenerateReadonlyElement(EntityProperty entityProperty, string bindValueName)
        {
            string input = string.Empty;
            switch (entityProperty.TypeName.RemoveNullable())
            {
                case "int":
                case "Int32":
                case "Int64":
                case "long":
                case "single":
                case "Single":
                case "double":
                case "Double":
                case "decimal":
                case "Decimal":
                    input = $"<InputNumber class=\"form-control\" id=\"input{entityProperty.Name}\" @bind-Value=\"{bindValueName}.{entityProperty.Name}\" />\n"; ;
                    Trace.WriteLine($"InputNumber property: {entityProperty.Name} type: {entityProperty.TypeName}");
                    break;
                case "bool":
                    input = $"<InputCheckbox class=\"form-control\" id=\"input{entityProperty.Name}\" @bind-Value=\"{bindValueName}.{entityProperty.Name}\" />\n";
                    Trace.WriteLine($"InputCheckbox property: {entityProperty.Name} type: {entityProperty.TypeName}");
                    break;
                case "System.DateTime":
                case "DateTime":
                    input = $"<InputDate class=\"form-control\" id=\"input{entityProperty.Name}\" @bind-Value=\"{bindValueName}.{entityProperty.Name}\" />\n";
                    Trace.WriteLine($"InputDate property: {entityProperty.Name} type: {entityProperty.TypeName}");
                    break;
                default:
                    input = $"<InputText class=\"form-control\" id=\"input{entityProperty.Name}\" @bind-Value=\"{bindValueName}.{entityProperty.Name}\" />\n";
                    Trace.WriteLine($"InputText property: {entityProperty.Name} type: {entityProperty.TypeName}");
                    break;
            }
            return input;
        }

        public static string GenerateFormInput(EntityProperty entityProperty, string bindValueName)
        {
            string input = string.Empty;
            switch (entityProperty.TypeName.RemoveNullable())
            {
                case "int":
                case "Int32":
                case "Int64":
                case "long":
                case "single":
                case "Single":
                case "double":
                case "Double":
                case "decimal":
                case "Decimal":
                    input = $"<InputNumber class=\"form-control\" id=\"input{entityProperty.Name}\" @bind-Value=\"{bindValueName}.{entityProperty.Name}\" />\n"; ;
                    Trace.WriteLine($"InputNumber property: {entityProperty.Name} type: {entityProperty.TypeName}");
                    break;
                case "bool":
                    input = $"<InputCheckbox class=\"form-control\" id=\"input{entityProperty.Name}\" @bind-Value=\"{bindValueName}.{entityProperty.Name}\" />\n";
                    Trace.WriteLine($"InputCheckbox property: {entityProperty.Name} type: {entityProperty.TypeName}");
                    break;
                case "System.DateTime":
                case "DateTime":
                    input = $"<InputDate class=\"form-control\" id=\"input{entityProperty.Name}\" @bind-Value=\"{bindValueName}.{entityProperty.Name}\" />\n";
                    Trace.WriteLine($"InputDate property: {entityProperty.Name} type: {entityProperty.TypeName}");
                    break;
                default:
                    input = $"<InputText class=\"form-control\" id=\"input{entityProperty.Name}\" @bind-Value=\"{bindValueName}.{entityProperty.Name}\" />\n";
                    Trace.WriteLine($"InputText property: {entityProperty.Name} type: {entityProperty.TypeName}");
                    break;
            }
            return input;
        }

        //public static void Generate(T entity, string outDir)
        //{
        //    if (entity is null)
        //        throw new ArgumentException("Entity Name must be specified.");
        //    if (string.IsNullOrWhiteSpace(outDir))
        //        outDir = Environment.CurrentDirectory;
        //    Environment.CurrentDirectory = outDir;


        //    var pluralizer = new Pluralize.NET.Pluralizer();
        //    GeneratorContext context = new GeneratorContext()
        //    {
        //        EntityClassName = entity.GetType().Name
        //    };
        //    context.PluralName = pluralizer.Pluralize(context.EntityClassName);

        //    #region ListPage
        //    ListPageTemplate listTemplate = new ListPageTemplate(context: context);
        //    string listReducerPageContent = listTemplate.TransformText();
        //    Directory.CreateDirectory($"Pages/{context.PluralName}/");
        //    File.WriteAllText($"Pages/{context.PluralName}/Index.cs", listReducerPageContent);
        //    #endregion

        //    // Detail Page Templating


        //}
    }
}