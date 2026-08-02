using System.Linq;

namespace InfiniteLoathing.Snakeskin.Directives
{
    internal class TemplateRoot : ParentDirective
    {
        public override TemplateNodeKind Kind => TemplateNodeKind.Root;
        
        public TemplateRoot(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}