using System.Text;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class TextNode : ITemplateNode
    {
        public string Text { get; }
        
        public TextNode(string text)
        {
            this.Text = text;
        }

        public StringBuilder Render(StringBuilder builder)
        {
            //todo: this
            throw new System.NotImplementedException();
        }
    }
}