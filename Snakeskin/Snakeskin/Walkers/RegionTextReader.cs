using System.Text.RegularExpressions;
using InfiniteLoathing.Snakeskin.Directives;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal class RegionTextReader
    {
        private const string DirectivePrefix = "@";
        
        private const string DirectiveGroupName = "Directive";
        private const string ArgumentsGroupName = "Arguments";
        private static readonly Regex DirectiveExpression =
            new Regex($@"^\s*@(?<{DirectiveGroupName}>\w+)"
                      + @"\s*"
                      + $@"(?<{ArgumentsGroupName}>[^,\s]+)?"
                      + $@"(?:\s*,\s*(?<{ArgumentsGroupName}>[^,\s]+))*$");

        public static RegionText GetRegionText(RegionDirectiveTriviaSyntax syntax) => new RegionText(
            syntaxTree: syntax.SyntaxTree,
            location: syntax.EndOfDirectiveToken.LeadingTrivia.First().GetLocation(),
            value: syntax.EndOfDirectiveToken.LeadingTrivia.ToFullString().Trim());

        public static bool IsDirective(string directiveString) => directiveString.StartsWith(DirectivePrefix);

        public static (Group Directive, Group Values) MatchRegionDirective(string directiveString)
        {
            var match = DirectiveExpression.Match(directiveString);
            return (match.Groups[DirectiveGroupName], match.Groups[ArgumentsGroupName]);
        }


        private const string ReplaceDirective = "replace";
        private const string RemoveDirective = "remove";
        
        public static bool TryParseDirective(Group directiveGroup, out ParentDirective directive)
        {
            if (directiveGroup.Success)
            {
                switch (directiveGroup.Value.ToLowerInvariant())
                {
                    case ReplaceDirective:
                        directive = new ReplaceDirective();
                        return true;
                    case RemoveDirective:
                        directive = new RemoveDirective();
                        return true;
                }
            }
            directive = null;
            return false;
        }
        
        private const string ArraySuffix = "[]";

        public static ValueNode CreateValue(string nodeText)
        {
            if (nodeText.EndsWith(ArraySuffix))
            {
                return new ValueNode(
                    identifier: nodeText.Substring(0, nodeText.Length - ArraySuffix.Length),
                    valueKind: ValueNodeKind.String,
                    isArray: true);
            }

            return new ValueNode(
                identifier: nodeText,
                valueKind: ValueNodeKind.String,
                isArray: false);
        }
    }
}