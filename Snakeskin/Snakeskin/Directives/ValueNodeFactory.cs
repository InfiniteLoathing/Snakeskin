using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Exceptions;
using Microsoft.CodeAnalysis.CSharp;

namespace InfiniteLoathing.Snakeskin.Directives
{
    internal static class ValueNodeFactory
    {

        private const string ArraySuffix = "[]";

        public static IEnumerable<ValueNode> Create(IEnumerable<string> nodeTexts)
        {
            foreach (var nodeText in nodeTexts)
            {
                if (TryCreate(nodeText, out var node))
                {
                    yield return node;
                }
            }
        }

        private static bool TryCreate(string nodeText, out ValueNode node)
        {
            if (nodeText.EndsWith(ArraySuffix))
            {
                return TryCreateStringArrayValue(nodeText, out node);
            }

            return TryCreateStringValue(nodeText, out node);
        }

        private static bool TryCreateStringValue(string nodeText, out ValueNode node)
        {
            if (!SyntaxFacts.IsValidIdentifier(nodeText))
            {
                // current: handle failure
                throw new InvalidValueNodeException(nodeText);
            }
            
            node = new ValueNode(nodeText, ValueNodeKind.String, false);
            return true;
        }

        private static bool TryCreateStringArrayValue(string nodeText, out ValueNode node)
        {
            var nameWithoutSuffix = nodeText.Substring(0, nodeText.Length - 2);

            if (!SyntaxFacts.IsValidIdentifier(nameWithoutSuffix))
            {
                // current: handle failure
                throw new InvalidValueNodeException(nodeText);
            }
            
            node = new ValueNode(nameWithoutSuffix, ValueNodeKind.String, true);
            return true;
        }
    }
}