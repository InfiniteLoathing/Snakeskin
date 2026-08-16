using System.Text;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class RemoveNode : ParentNode
    {
        public override StringBuilder Render(StringBuilder builder) => builder;
    }
}