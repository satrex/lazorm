using System;
namespace LazormFluxorGenerator
{
    public partial class LoadListActionTemplate
    {
        public string entityClassName;
        public string crud;
        public string actionType;
        public string entityClassNamePlural;
        public string namespaceText = "Lazorm";

        public LoadListActionTemplate(ActionContext context)
        {
            entityClassName = context.EntityClassName;
            entityClassNamePlural = context.SchemaName;
            crud = context.CrudKind;
            actionType = context.ActionKind;
            namespaceText = context.NameSpace;
        }
    }
}
