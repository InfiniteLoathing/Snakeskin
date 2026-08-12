using System.Collections.Immutable;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class ReplaceDirectiveSyntax : DirectiveSyntax
    {
        public ReplaceDirectiveSyntax(ImmutableArray<ValueSyntax> values)
        {
            this.Values = values;
        }
        
        public override SyntaxKind Kind => SyntaxKind.Replace;

        public ImmutableArray<ValueSyntax> Values { get; }
    }
}