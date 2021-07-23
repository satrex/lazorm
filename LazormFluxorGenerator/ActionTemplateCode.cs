using System;
namespace LazormFluxorGenerator
{
    public partial class ActionTemplate
    {
        public string entityClassName;
        public string entityClassNamePlural;
        public string crud;
        public string actionType;
        
        public ActionTemplate(ActionContext context)
        {
            entityClassName = context.EntityClassName;
            entityClassNamePlural = context.SchemaName;
            crud = context.CrudKind;
            actionType = context.ActionKind;
        }
    }
}
