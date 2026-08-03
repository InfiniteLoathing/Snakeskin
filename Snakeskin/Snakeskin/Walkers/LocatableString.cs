using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal readonly struct LocatableString
    {
        private readonly SyntaxTree _syntaxTree;
        
        public readonly Location Location;
        
        public readonly string Value;
        
        public LocatableString(SyntaxTree syntaxTree, Location location, string value)
        {
            _syntaxTree = syntaxTree;
            Location = location;
            Value = value;
        }

        public static implicit operator string(LocatableString locatableString) => locatableString.Value;

        public Location GetSubLocation(Match match) =>
            Location.Create(_syntaxTree, new TextSpan(Location.SourceSpan.Start + match.Index, match.Length));
    }
}