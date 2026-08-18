using System.CodeDom.Compiler;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class ForEachNode : ParentNode
    {
        private readonly ValueNode _iterator;
        private readonly ValueNode _array;
        
        public ForEachNode(ValueNode iterator, ValueNode array)
        {
            _iterator = iterator;
            _array = array;
        }
        
        public override void Render(IndentedTextWriter writer)
        {
            writer.WriteLine($"foreach (var {_iterator.GetSourceVar()} in {_array.GetSourceVar()})");
            writer.WriteLine("{");
            writer.Indent++;
            base.Render(writer);
            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}