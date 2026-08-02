using System.Linq;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Extensions
{
    internal static class SyntaxNodeExtensions
    {
        public static bool RegionsAreValid(this SyntaxNode syntaxNode) =>
            syntaxNode.GetDiagnostics().All(x => x.Id != "CS1038");
    }
}