using System.CodeDom.Compiler;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal interface ITemplateNode
    {
        void Render(IndentedTextWriter writer);
    }
}