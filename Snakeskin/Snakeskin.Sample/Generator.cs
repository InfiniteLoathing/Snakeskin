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
                var testTemplate = TestTemplate.Render(vals);
                var blankTemplate = BlankTemplate.Render();
                
                c.AddSource("TestTemplateFilled.cs", testTemplate);
                c.AddSource("BlankTemplate.cs", blankTemplate);
            });
        }
    }
}
#endif