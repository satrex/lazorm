using System;
namespace LazormPageGenerator
{
	public partial class ControllerTemplate
	{
        public string entityClassNameSingular;
        public string entityClassNamePlural;
        public string entityNameSingular;
        public string entityNamePlural;
        public string pageNamespace; 
        public List<EntityProperty> entityProperties;

        public ControllerTemplate(GeneratorContext context)
        {
            entityClassNameSingular = context.EntityClassName;
            entityClassNamePlural = context.EntityClassNamePlural;
            entityNameSingular = entityClassNameSingular.Uncapitalize();
            entityNamePlural = entityClassNamePlural.Uncapitalize();
            entityProperties = context.EntityProperties;
            pageNamespace = context.Namespace;
        }
	}
}

