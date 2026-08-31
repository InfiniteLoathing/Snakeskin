namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class IfDirectiveSyntax : DirectiveSyntax
    {
        public override bool IsValid => this.Condition != null;

        public override DirectiveSyntaxKind Kind => DirectiveSyntaxKind.If;

        public IfDirectiveSyntax(ValueSyntax condition)
        {
            this.Condition = condition;
        }

        public ValueSyntax Condition { get; }
    }
}