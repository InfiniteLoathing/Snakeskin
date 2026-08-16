using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal abstract class DirectiveSyntax
    {
        public virtual bool IsValid => true;
        
        public abstract DirectiveSyntaxKind Kind { get; }
    }
}