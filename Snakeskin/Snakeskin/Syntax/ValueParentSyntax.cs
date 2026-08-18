using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class ValueParentSyntax
    {
        public string Identifier { get; }

        public Location Location { get; }

        public ValueParentSyntax(string identifier, Location location)
        {
            this.Identifier = identifier;
            this.Location = location;
        }
    }
}