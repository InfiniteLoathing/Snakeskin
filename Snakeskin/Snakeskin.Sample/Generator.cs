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
                    TestBool = true,
                    Properties = new []
                    {
                        new Property
                        {
                            Type = "string",
                            Name = "Text",
                            TestBoolProp = true
                        },
                        new Property
                        {
                            Type = "int",
                            Name = "Position",
                            TestBoolProp = false
                        },
                        new Property
                        {
                            Type = "object",
                            Name = "Value",
                            TestBoolProp = true
                        }
                    }
                };
                var v1 = TestTemplate.Render(vals);
                var v2 = BlankTemplate.Render();
                
                c.AddSource("TestTemplateFilled.cs", TestTemplate.Render(vals));
                c.AddSource("BlankTemplate.cs", BlankTemplate.Render());
            });
        }
    }
}
#endif