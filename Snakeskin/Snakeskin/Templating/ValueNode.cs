using System;
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

        public ValueType Type { get; }

        protected Dictionary<string, ValueNode> Properties { get; }

        public ValueNode(
            string identifier,
            TextSpan textSpan,
            ValueType type = ValueType.String,
            bool isArray = false)
        {
            this.ParentIdentifier = null;
            this.Identifier = identifier;
            this.TextSpan = textSpan;
            this.Type = type;
            this.IsArray = isArray;
            this.Properties = type == ValueType.Object ? new Dictionary<string, ValueNode>() : null;
        }

        public ValueNode(
            string parentIdentifier,
            string identifier,
            TextSpan textSpan,
            ValueType type = ValueType.String,
            bool isArray = false)
        {
            this.ParentIdentifier = parentIdentifier;
            this.Identifier = identifier;
            this.TextSpan = textSpan;
            this.Type = type;
            this.IsArray = isArray;
            this.Properties = type == ValueType.Object ? new Dictionary<string, ValueNode>() : null;
        }

        public bool TypeMatches(ValueSyntax valueSyntax) =>
            this.IsArray == valueSyntax.IsArray && this.Type == valueSyntax.Type;

        public virtual bool TryGetProperty(string identifier, out ValueNode property) =>
            this.Properties.TryGetValue(identifier, out property);

        public virtual ValueNode AddProperty(ValueSyntax valueSyntax)
        {
            var property = new ValueNode(
                this.Identifier,
                valueSyntax.Identifier,
                valueSyntax.TextSpan,
                valueSyntax.Type,
                valueSyntax.IsArray);
            
            this.Properties.Add(valueSyntax.Identifier, property);

            return property;
        }

        public virtual void Render(IndentedTextWriter writer) =>
            writer.WriteLine(
                $"{SourceConstants.StringBuilder}.Append({this.GetSourceIdentifier()});");

        public void RenderInterface(IndentedTextWriter writer)
        {
            writer.WriteLine($"public interface {this.GetSourceInterface()}");
            writer.WriteLine("{");
            writer.Indent++;

            foreach (var property in this.Properties.Values)
            {
                property.RenderInterfaceProperty(writer);
            }

            writer.Indent--;
            
            writer.WriteLine("}");
        }

        public void RenderInterfaceProperty(IndentedTextWriter writer) =>
            writer.WriteLine($"{this.GetSourceType()} {this.Identifier} {{ get; }}");

        public string GetDiagnosticIdentifier() => this.ParentIdentifier is null
            ? this.Identifier
            : $"{this.ParentIdentifier}.{this.Identifier}";

        public string GetSourceIdentifier() => this.ParentIdentifier is null
            ? $"_v{this.Identifier}"
            : $"_v{this.ParentIdentifier}.{this.Identifier}";

        private string GetSourceInterface() => $"I{this.Identifier}";

        private string GetSourceType()
        {
            if (this.Type == ValueType.Object)
            {
                return this.IsArray ? $"{this.GetSourceInterface()}[]" : this.GetSourceInterface();
            }

            switch (this.Type)
            {
                case ValueType.String:
                    return this.IsArray ? "string[]" : "string";
                case ValueType.Bool:
                    return this.IsArray ? "bool[]" : "bool";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}