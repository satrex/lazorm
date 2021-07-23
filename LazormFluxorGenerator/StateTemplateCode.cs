using System;
namespace LazormFluxorGenerator
{
    public partial class StateTemplate
    {
        public string entityClassName;
        public string crud;
        public string entityClassNamePlural;

        public StateTemplate(GeneratorContext context)
        {
            entityClassName = context.EntityClassName;
            crud = context.CrudKind;
            entityClassNamePlural = context.SchemaName;
        }
    }
}
