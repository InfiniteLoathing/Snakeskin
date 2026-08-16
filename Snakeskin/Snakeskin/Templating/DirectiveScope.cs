using System.Collections.Generic;
using System.Collections.Immutable;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class DirectiveScope
    {
        public DirectiveScope(
            ImmutableDictionary<string, ValueNode> values = null,
            ImmutableDictionary<string, ValueNode> replacements = null)
        {
            this.Values = values ?? ImmutableDictionary<string, ValueNode>.Empty;
            this.Replacements = replacements?? ImmutableDictionary<string, ValueNode>.Empty;
        }

        public ImmutableDictionary<string, ValueNode> Values { get; }

        public ImmutableDictionary<string, ValueNode> Replacements { get; }
    }
}