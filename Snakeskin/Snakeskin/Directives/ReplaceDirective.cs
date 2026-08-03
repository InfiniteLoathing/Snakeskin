using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfiniteLoathing.Snakeskin.Directives
{
    internal class ReplaceDirective : ParentDirective
    {
        public override TemplateNodeKind Kind => TemplateNodeKind.Replace;

        public override bool SupportsValueKind(ValueNodeKind kind) => kind == ValueNodeKind.String;

        public override bool HasValues => true;
    }
}