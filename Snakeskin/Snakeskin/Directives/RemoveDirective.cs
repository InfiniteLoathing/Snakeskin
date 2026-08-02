using System.Text;

namespace InfiniteLoathing.Snakeskin.Directives
{
    internal class RemoveDirective : ParentDirective
    {
        public override TemplateNodeKind Kind => TemplateNodeKind.Remove;

        public override StringBuilder Render(StringBuilder builder) => builder;
    }
}