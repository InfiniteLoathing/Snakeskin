using System;
using System.Text;

namespace InfiniteLoathing.Snakeskin.Directives
{
    internal class ValueNode : ITemplateNode
    {
        public TemplateNodeKind Kind => TemplateNodeKind.Value;
        
        public ValueNodeKind ValueKind { get; }
        
        public bool IsArray { get; }
        
        public ValueNode(string identifier, ValueNodeKind valueKind, bool isArray)
        {
            this.Identifier = identifier;
            this.ValueKind = valueKind;
            this.IsArray = isArray;
        }
        
        public string Identifier { get; }
        
        public StringBuilder Render(StringBuilder builder) => builder.Append(this.Identifier);
    }
}