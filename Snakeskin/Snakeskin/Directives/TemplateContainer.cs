using System.Collections.Generic;
using System.Text;

namespace InfiniteLoathing.Snakeskin.Directives
{
    // current: Figure out cleaner inheritance
    internal abstract class TemplateContainer : ITemplateNode
    {
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
