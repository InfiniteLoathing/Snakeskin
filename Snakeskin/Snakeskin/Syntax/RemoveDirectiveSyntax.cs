using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class RemoveDirectiveSyntax : DirectiveSyntax
    {
        public override DirectiveSyntaxKind Kind => DirectiveSyntaxKind.Remove;
    }
}