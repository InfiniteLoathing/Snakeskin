using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal abstract class DirectiveSyntax
    {
        
        public abstract DirectiveSyntaxKind Kind { get; }
    }
}