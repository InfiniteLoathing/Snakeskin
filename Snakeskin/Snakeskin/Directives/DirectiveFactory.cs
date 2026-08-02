using System;
using System.Linq;
using System.Text.RegularExpressions;
using InfiniteLoathing.Snakeskin.Exceptions;
using Microsoft.CodeAnalysis.CSharp;

namespace InfiniteLoathing.Snakeskin.Directives
{
    internal static class DirectiveFactory
    {
        private const string DirectiveGroupName = "Replace";
        private const string ArgumentsGroupName = "Arguments";
        private static readonly Regex DirectiveExpression =
            new Regex($@"^\s*@(?<{DirectiveGroupName}>\w+)(?:\s+(?<{ArgumentsGroupName}>.*?))?\s*$");

        private const string ReplaceDirective = "replace";
        private const string RemoveDirective = "remove";

        public static bool TryCreateFromRegion(string directiveString, out ParentDirective directive)
        {
            var match = DirectiveExpression.Match(directiveString);

            var directiveGroup = match.Groups[DirectiveGroupName];
            var argumentsGroup = match.Groups[ArgumentsGroupName];
            if (match.Success && directiveGroup.Success)
            {
                switch (directiveGroup.Value.ToLowerInvariant())
                {
                    case ReplaceDirective:
                        directive = CreateReplaceDirective(argumentsGroup);
                        return true;
                    case RemoveDirective:
                        directive = CreateRemoveDirective(argumentsGroup);
                        return true;
                }
            }
            else
            {
                
            }

            directive = null;
            return false;
        }

        private static ReplaceDirective CreateReplaceDirective(Group argumentsGroup)
        {
            if (!argumentsGroup.Success || argumentsGroup.Value.Length == 0)
            {
                throw new InvalidDirectiveException(ReplaceDirective);
            }

            var arguments = argumentsGroup.Value.Split(',');

            if (arguments.Length == 0)
            {
                // current: handle failure
                throw new InvalidDirectiveException(ReplaceDirective);
            }

            var values = ValueNodeFactory.Create(arguments).ToArray();

            if (values.Any(x => x.IsArray || x.ValueKind != ValueNodeKind.String))
            {
                throw new InvalidOperationException(ReplaceDirective);
            }

            return new ReplaceDirective(values);
        }

        private static RemoveDirective CreateRemoveDirective(Group argumentsGroup)
        {
            if (argumentsGroup.Success && argumentsGroup.Value.Length != 0)
            {
                // current: handle failure
                throw new InvalidDirectiveException(RemoveDirective);
            }
            return new RemoveDirective();
        }
    }
    
}