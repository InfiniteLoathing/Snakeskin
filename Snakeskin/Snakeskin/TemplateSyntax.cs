using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin
{
    internal class TemplateSyntax
    {
        public TemplateSyntax(string name, SyntaxNode root)
        {
            this.Name = name;
            this.Root = root;
        }

        public string Name { get; }
        
        public SyntaxNode Root { get; }
    }
}