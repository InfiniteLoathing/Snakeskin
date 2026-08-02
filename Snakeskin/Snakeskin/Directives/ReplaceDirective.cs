using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfiniteLoathing.Snakeskin.Directives
{
    internal class ReplaceDirective : ParentDirective
    {
        public override TemplateNodeKind Kind => TemplateNodeKind.Replace;

        public override bool HasValues => true;

        public override Dictionary<string, ValueNode> Values { get; }

        public ReplaceDirective(IEnumerable<ValueNode> values)
        {
            this.Values = values.ToDictionary(x => x.Identifier, x => x);
        }

        public override StringBuilder Render(StringBuilder builder)
        {
            builder.AppendLine(string.Join(",", this.Values));
            return base.Render(builder);
        }
    }
}