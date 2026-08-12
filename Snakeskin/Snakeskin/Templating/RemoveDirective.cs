using System.Text;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class RemoveDirective : ParentNode
    {
        public override StringBuilder Render(StringBuilder builder) => builder;
    }
}