using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class ForEachDirectiveSyntax : DirectiveSyntax
    {
        public override DirectiveSyntaxKind Kind => DirectiveSyntaxKind.ForEach;

        public ForEachDirectiveSyntax(ValueSyntax iterator, ValueSyntax array)
        {
            this.Iterator = iterator;
            this.Array = array;
        }

        public ValueSyntax Iterator { get; }

        public ValueSyntax Array { get; }
    }
}