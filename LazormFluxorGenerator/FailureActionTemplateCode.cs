using System;
namespace LazormFluxorGenerator
{
    public partial class FailureActionTemplate
    {
        public string namespaceText = "Lazorm";

        public FailureActionTemplate(GeneratorContext context)
        {
            namespaceText = context.NameSpace;
        }
    }
}
