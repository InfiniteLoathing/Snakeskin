#if RENDER_TEMPLATE
using Microsoft.CodeAnalysis;
using Snakeskin.Templates;

namespace InfiniteLoathing.Snakeskin.Sample
{
    [Generator]
    public class Generator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(c =>
            {
                var vals = new TestTemplateValues
                {
                    TestObj = new TestObj
                    {
                        StringLiteral = "This was replaced"
                    },
                    Namespace = "Concrete.Namespace",
                    ClassName = "ConcreteClass",
                    Strings = new[]{"test1", "test2", "test3"},
                    Properties = new []
                    {
                        new Property
                        {
                            Type = "string",
                            Name = "Text"
                        },
                        new Property
                        {
                            Type = "int",
                            Name = "Position"
                        },
                        new Property
                        {
                            Type = "object",
                            Name = "Value"
                        }
                    }
                };
                c.AddSource("TestTemplateFilled.cs", TestTemplate.Render(vals));
            });
        }
    }
}
#endif