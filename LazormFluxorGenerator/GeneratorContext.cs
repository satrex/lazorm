using System;
namespace LazormFluxorGenerator
{
    public class GeneratorContext
    {
        public GeneratorContext()
        {
        }

        public string EntityClassName { get; set; }

        public string CrudKind { get; set; }

        public string SchemaName { get; set; }
    }
}
