using System.Text;

namespace InfiniteLoathing.Snakeskin.Directives
{
    internal class RemoveDirective : TemplateContainer
    {
        public override StringBuilder Render(StringBuilder builder) => builder;
    }
}