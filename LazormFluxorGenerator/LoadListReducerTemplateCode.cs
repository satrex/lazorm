﻿using System;
namespace LazormFluxorGenerator
{
    public partial class LoadListReducerTemplate
    {
        public string entityClassName;
        public string entityClassNamePlural;
        public string crud;
        public string namespaceText = "Lazorm";

        public LoadListReducerTemplate(GeneratorContext context)
        {
            entityClassName = context.EntityClassName;
            crud = context.CrudKind;
            entityClassNamePlural = context.SchemaName;
            namespaceText = context.NameSpace;
        }
    }
}
