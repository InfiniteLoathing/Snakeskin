using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin
{
    internal class SyntaxTreeLocator : ILocator
    {
        private readonly SyntaxTree _syntaxTree;
        
        public SyntaxTreeLocator(SyntaxTree syntaxTree)
        {
            _syntaxTree = syntaxTree;
        }

        public Location Locate(TextSpan textSpan) => Location.Create(_syntaxTree, textSpan);
    }
}