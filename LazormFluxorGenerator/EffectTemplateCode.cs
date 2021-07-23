using System;
namespace LazormFluxorGenerator
{
    public partial class EffectTemplate
    {
        public string entityClassName;
        public string entityClassNamePlural;
        public string crud;
        public string actionType;

        public EffectTemplate(ActionContext context)
        {
            entityClassName = context.EntityClassName;
            entityClassNamePlural = context.SchemaName;
            crud = context.CrudKind;
            actionType = context.ActionKind;
        }
    }
}
