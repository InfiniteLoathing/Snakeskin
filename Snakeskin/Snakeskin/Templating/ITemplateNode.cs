using System.Text;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal interface ITemplateNode
    {
        StringBuilder Render(StringBuilder builder);
    }
}