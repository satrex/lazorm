using System;
namespace LazormFluxorGenerator
{
    public partial class RootStateTemplate
    {
        public string entityClassName;
        public string crud;
        public string entityClassNamePlural;
        public string namespaceText = "Lazorm";

        public RootStateTemplate(GeneratorContext context)
        {
            entityClassName = context.EntityClassName;
            crud = context.CrudKind;
            entityClassNamePlural = context.SchemaName;
            namespaceText = context.NameSpace;
        }
    }
}
