using System;
using System.Collections.Generic;
using System.Text;

namespace InfiniteLoathing.Snakeskin.Directives
{
    // current: Figure out cleaner inheritance
    internal abstract class ParentDirective : ITemplateNode
    {
        public abstract TemplateNodeKind Kind { get; }

        public virtual bool HasValues => false;

        public virtual bool SupportsValueKind(ValueNodeKind kind, bool isArray) => false;

        public virtual StringBuilder Render(StringBuilder builder)
        {
            foreach (var child in Children)
            {
                child.Render(builder);
            }

            return builder;
        }
        
        public readonly List<ITemplateNode> Children = new List<ITemplateNode>();
    }
}
