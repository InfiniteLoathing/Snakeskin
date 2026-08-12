using System.Linq;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal static class RootValidator
    {
        public static bool RegionsAreValid(SyntaxNode syntaxNode) =>
            syntaxNode.GetDiagnostics().All(x => x.Id != "CS1038");
    }
}