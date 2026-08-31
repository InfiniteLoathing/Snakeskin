using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class StringBuilderTemplate : ITemplate
    {
        private ParentNode Root { get; }

        private ImmutableArray<ValueNode> RequiredValues { get; }

        private string Namespace { get; }

        private string ClassName { get; }

        public StringBuilderTemplate(
            ParentNode root,
            ImmutableArray<ValueNode> requiredValues,
            string @namespace,
            string className)
        {
            this.Root = root;
            this.RequiredValues = requiredValues;
            this.Namespace = @namespace;
            this.ClassName = className;
        }

        public string Render()
        {
            using (var writer = new StringWriter())
            using (var indentedWriter = new IndentedTextWriter(writer))
            {
                indentedWriter.WriteLine($"namespace {this.Namespace}");
                indentedWriter.WriteLine("{");
                indentedWriter.Indent++;
                indentedWriter.WriteLine($"internal class {this.ClassName}Template");
                indentedWriter.WriteLine("{");
                indentedWriter.Indent++;
                if (this.RequiredValues.Any())
                {
                    foreach (var requiredValue in this.RequiredValues.Where(x => x.Type == ValueType.Object))
                    {
                        requiredValue.RenderInterface(indentedWriter);
                    }

                    indentedWriter.WriteLine("public interface ITemplateValues");
                    indentedWriter.WriteLine("{");
                    indentedWriter.Indent++;
                    foreach (var requiredValue in this.RequiredValues)
                    {
                        requiredValue.RenderProperty(indentedWriter);
                    }
                    indentedWriter.Indent--;
                    indentedWriter.WriteLine("}");
                    indentedWriter.WriteLine("public static string Render(ITemplateValues values)");
                }
                else
                {
                    indentedWriter.WriteLine("public static string Render()");
                }
                
                
                indentedWriter.WriteLine("{");
                indentedWriter.Indent++;
                foreach (var requiredValue in this.RequiredValues)
                {
                    indentedWriter.WriteLine(
                        $"var {requiredValue.GetSourceIdentifier()} = values.{requiredValue.Identifier};");
                }
                indentedWriter.WriteLine($"var {SourceConstants.StringBuilder} = new System.Text.StringBuilder();");
                this.Root.Render(indentedWriter);
                indentedWriter.WriteLine($"return {SourceConstants.StringBuilder}.ToString();");
                indentedWriter.Indent--;
                indentedWriter.WriteLine("}");
                indentedWriter.Indent--;
                indentedWriter.WriteLine("}");
                indentedWriter.Indent--;
                indentedWriter.WriteLine("}");
                return writer.ToString();
            }
        }
    }
}