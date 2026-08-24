using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class ReplacementScope
    {
        private readonly Regex _expression;
        private readonly ImmutableDictionary<string, ValueNode> _replaceValues;
        private readonly int _longestReplaceValue;

        public ReplacementScope(ImmutableDictionary<string, ValueNode> replaceValues)
        {
            _expression = replaceValues.Any()
                ? new Regex($"({string.Join("|", replaceValues.Keys.Select(Regex.Escape))})")
                : null;
            _replaceValues = replaceValues;
            _longestReplaceValue = replaceValues.Any() ? replaceValues.Keys.Max(x => x.Length) : 0;
        }

        public IEnumerable<string> Split(string text)
        {
            if (_expression is null)
            {
                yield return text;
                yield break;
            }

            foreach (var section in _expression.Split(text))
            {
                yield return section;
            }
        }

        public bool TryGetReplaceNode(string textSection, out ValueNode node)
        {
            if (textSection.Length <= _longestReplaceValue)
            {
                return _replaceValues.TryGetValue(textSection, out node);
            }

            node = null;
            return false;
        }
    }
}