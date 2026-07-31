using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Sample
{
    [Generator]
    public class Generator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(AddEmbeddedAttribute);
        }

        private static void AddEmbeddedAttribute(IncrementalGeneratorPostInitializationContext context) =>
            context.AddEmbeddedAttributeDefinition();
    }
}