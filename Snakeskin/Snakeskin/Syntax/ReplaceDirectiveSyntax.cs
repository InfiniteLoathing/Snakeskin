using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class ReplaceDirectiveSyntax : DirectiveSyntax
    {
        public override DirectiveSyntaxKind Kind => DirectiveSyntaxKind.Replace;

        public ReplaceDirectiveSyntax(ImmutableArray<ValueSyntax> values)
        {
            this.Values = values;
        }

        public ImmutableArray<ValueSyntax> Values { get; }
    }
}