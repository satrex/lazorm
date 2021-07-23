using System;
namespace LazormFluxorGenerator
{
    public partial class ReducerTemplate
    {
        public string entityClassName;
        public string entityClassNamePlural;
        public string crud; 
        
        public ReducerTemplate(GeneratorContext context)
        {
            entityClassName = context.EntityClassName;
            crud = context.CrudKind;
            entityClassNamePlural = context.SchemaName;
        }
    }
}
