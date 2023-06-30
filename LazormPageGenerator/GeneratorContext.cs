using System;
using System.Diagnostics;

namespace LazormPageGenerator
{
    public class GeneratorContext
    {
        public string EntityClassName { get; set; } = string.Empty;

        public string EntityClassNamePlural { get; set; } = string.Empty;

        public string EntityName { get { return this.EntityClassName.Uncapitalize(); } } 

        public string EntityNamePlural { get { return this.EntityClassNamePlural.Uncapitalize(); } }

        public List<EntityProperty> EntityProperties { get; set; }  = new List<EntityProperty>();

        public string Namespace { get; set; } = "Lazorm";
    }

    public class EntityProperty
    {
        public EntityProperty(string name, bool editable, string returnTypeName)
        {
            this.Name = name;
            this.Editable = editable;
            this.TypeName = returnTypeName;
        }
        public string Name { get; }
        public bool Editable { get; }
        public string TypeName { get; }
    }
}
