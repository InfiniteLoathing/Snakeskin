using System.Text;

namespace InfiniteLoathing.Snakeskin.Directives
{
    internal class TextNode : ITemplateNode
    {
        public StringBuilder Render(StringBuilder builder) => builder.Append(this.Text);

        public string Text { get; }
        
        public TextNode(string text)
        {
            this.Text = text;
        }
    }
}