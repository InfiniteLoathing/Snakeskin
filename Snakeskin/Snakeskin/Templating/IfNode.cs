using System.CodeDom.Compiler;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class IfNode : ParentNode
    {
        private readonly ValueNode _condition;
        
        public IfNode(ValueNode condition)
        {
            _condition = condition;
        }
        
        public override void Render(IndentedTextWriter writer)
        {
            writer.WriteLine($"if ({_condition.GetSourceIdentifier()})");
            writer.WriteLine("{");
            writer.Indent++;
            base.Render(writer);
            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}