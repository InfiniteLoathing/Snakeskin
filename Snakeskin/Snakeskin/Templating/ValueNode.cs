using System.CodeDom.Compiler;
using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class ValueNode : ITemplateNode, IValueDefinition
    {
        public string ParentIdentifier { get; }

        public string Identifier { get; }

        public TextSpan TextSpan { get; }

        public bool IsArray { get; }

        public bool IsObject { get; }

        protected Dictionary<string, ValueNode> Properties { get; }

        public ValueNode(
            string identifier,
            TextSpan textSpan,
            bool isArray = false,
            bool isObject = false)
        {
            this.ParentIdentifier = null;
            this.Identifier = identifier;
            this.TextSpan = textSpan;
            this.IsArray = isArray;
            this.IsObject = isObject;
            this.Properties = isObject ? new Dictionary<string, ValueNode>() : null;
        }

        public ValueNode(
            string parentIdentifier,
            string identifier,
            TextSpan textSpan,
            bool isArray = false,
            bool isObject = false)
        {
            this.ParentIdentifier = parentIdentifier;
            this.Identifier = identifier;
            this.TextSpan = textSpan;
            this.IsArray = isArray;
            this.IsObject = isObject;
            this.Properties = isObject ? new Dictionary<string, ValueNode>() : null;
        }

        public bool TypeMatches(ValueSyntax valueSyntax) =>
            this.IsArray == valueSyntax.IsArray && this.IsObject == valueSyntax.IsObject;

        public virtual bool TryGetProperty(string identifier, out ValueNode property) =>
            this.Properties.TryGetValue(identifier, out property);

        public virtual ValueNode AddProperty(ValueSyntax valueSyntax)
        {
            var property = new ValueNode(
                this.Identifier,
                valueSyntax.Identifier,
                valueSyntax.TextSpan,
                valueSyntax.IsArray,
                valueSyntax.IsObject);
            
            this.Properties.Add(valueSyntax.Identifier, property);

            return property;
        }

        public virtual void Render(IndentedTextWriter writer) =>
            writer.WriteLine(
                $"{SourceConstants.StringBuilder}.Append({this.GetSourceVar()});");

        public void RenderInterface(IndentedTextWriter writer)
        {
            writer.WriteLine($"public interface {this.GetSourceInterface()}");
            writer.WriteLine("{");
            writer.Indent++;

            foreach (var property in this.Properties.Values)
            {
                property.RenderProperty(writer);
            }

            writer.Indent--;
            
            writer.WriteLine("}");
        }

        public void RenderProperty(IndentedTextWriter writer) =>
            writer.WriteLine($"{this.GetSourceType()} {this.Identifier} {{ get; set; }}");

        public string GetDiagnosticIdentifier() => this.ParentIdentifier is null
            ? this.Identifier
            : $"{this.ParentIdentifier}.{this.Identifier}";

        public string GetSourceVar() => this.ParentIdentifier is null
            ? $"_v{this.Identifier}"
            : $"_v{this.ParentIdentifier}.{this.Identifier}";

        private string GetSourceInterface() => $"I{this.Identifier}";

        private string GetSourceType()
        {
            if (this.IsObject)
            {
                return this.IsArray ? $"{this.GetSourceInterface()}[]" : this.GetSourceInterface();
            }

            return this.IsArray ? "string[]" : "string";
        }
    }
}