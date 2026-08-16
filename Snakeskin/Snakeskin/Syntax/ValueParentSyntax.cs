using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class ValueParentSyntax
    {
        public readonly string Identifier;

        public readonly Location Location;

        public ValueParentSyntax(string identifier, Location location)
        {
            Identifier = identifier;
            Location = location;
        }
    }
}