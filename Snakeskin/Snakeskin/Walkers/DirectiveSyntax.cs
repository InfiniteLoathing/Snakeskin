using System.Collections.Immutable;

namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal abstract class DirectiveSyntax
    {
        public abstract SyntaxKind Kind { get; }
    }
}