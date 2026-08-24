using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis.CSharp;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class TextNode : ITemplateNode
    {
        public string Text { get; }
        
        public TextNode(string text)
        {
            this.Text = text;
        }

        public void Render(IndentedTextWriter writer) =>
            writer.WriteLine(
                $"{SourceConstants.StringBuilder}.Append({SymbolDisplay.FormatLiteral(this.Text, true)});");
    }
}