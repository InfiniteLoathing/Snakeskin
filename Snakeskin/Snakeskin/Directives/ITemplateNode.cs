using System.Text;

namespace InfiniteLoathing.Snakeskin.Directives
{
    internal interface ITemplateNode
    {
        TemplateNodeKind Kind { get; }

        StringBuilder Render(StringBuilder builder);
    }
}