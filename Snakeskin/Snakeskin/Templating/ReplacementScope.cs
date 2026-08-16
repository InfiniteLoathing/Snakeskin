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

        public ReplacementScope(Regex expression, ImmutableDictionary<string, ValueNode> replaceValues)
        {
            _expression = expression;
            _replaceValues = replaceValues;
            _longestReplaceValue = replaceValues.Keys.Max(x => x.Length);
        }

        public string[] Split(string text) => _expression.Split(text);

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