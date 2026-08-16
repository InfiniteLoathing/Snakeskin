using System.Collections.Immutable;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class ReplaceDirectiveSyntax : DirectiveSyntax
    {
        public override bool IsValid => !this.Values.IsDefaultOrEmpty;
        
        public override DirectiveSyntaxKind Kind => DirectiveSyntaxKind.Replace;

        public ReplaceDirectiveSyntax(ImmutableArray<ValueSyntax> values)
        {
            this.Values = values;
        }

        public ImmutableArray<ValueSyntax> Values { get; }
    }
}