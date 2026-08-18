using System.CodeDom.Compiler;
using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Syntax;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Templating
{
    // current: Split replacement nodes?
    internal class ValueNode : ITemplateNode, ITemplateValue
    {
        public string ParentIdentifier { get; }

        public string Identifier { get; }

        public Location Location { get; }

        public bool IsArray { get; }

        public bool IsObject { get; }

        protected Dictionary<string, ValueNode> Properties { get; }

        public ValueNode(
            string identifier,
            Location location,
            bool isArray = false,
            bool isObject = false)
        {
            this.ParentIdentifier = null;
            this.Identifier = identifier;
            this.Location = location;
            this.IsArray = isArray;
            this.IsObject = isObject;
            this.Properties = isObject ? new Dictionary<string, ValueNode>() : null;
        }

        public ValueNode(
            string parentIdentifier,
            string identifier,
            Location location,
            bool isArray = false,
            bool isObject = false)
        {
            this.ParentIdentifier = parentIdentifier;
            this.Identifier = identifier;
            this.Location = location;
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
                valueSyntax.Location,
                valueSyntax.IsArray,
                valueSyntax.IsObject);
            
            this.Properties.Add(valueSyntax.Identifier, property);

            return property;
        }

        // todo: rename with new convention
        public virtual string GetIdentifierName() => this.ParentIdentifier is null
            ? this.Identifier
            : $"{this.ParentIdentifier}.{this.Identifier}";

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