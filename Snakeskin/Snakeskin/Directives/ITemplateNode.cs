using System.Text;

namespace InfiniteLoathing.Snakeskin.Directives
{
    internal interface ITemplateNode
    {
        StringBuilder Render(StringBuilder builder);
    }
}