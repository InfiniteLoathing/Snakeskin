using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin
{
    internal class SyntaxTreeLocator
    {
        private readonly SyntaxTree _syntaxTree;
        private readonly int _offset;
        
        public SyntaxTreeLocator(SyntaxTree syntaxTree, int offset)
        {
            _syntaxTree = syntaxTree;
            _offset = offset;
        }

        public Location Locate(TextSpan textSpan) =>
            Location.Create(_syntaxTree, new TextSpan(textSpan.Start + _offset, textSpan.Length));
    }
}