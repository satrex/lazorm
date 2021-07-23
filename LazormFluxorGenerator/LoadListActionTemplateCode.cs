using System;
namespace LazormFluxorGenerator
{
    public partial class LoadListActionTemplate
    {
        public string entityClassName;
        public string crud;
        public string actionType;
        public string entityClassNamePlural;
        
        public LoadListActionTemplate(ActionContext context)
        {
            entityClassName = context.EntityClassName;
            entityClassNamePlural = context.SchemaName;
            crud = context.CrudKind;
            actionType = context.ActionKind;
        }
    }
}
