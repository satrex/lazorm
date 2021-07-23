using System;
namespace LazormFluxorGenerator
{
    public partial class FeatureTemplate
    {
        public string entityClassName;
        public string crud;

        public string entityClassNamePlural;

        public FeatureTemplate(GeneratorContext context)
        {
            entityClassName = context.EntityClassName;
            crud = context.CrudKind;
            entityClassNamePlural = context.SchemaName;
        }
    }
}
