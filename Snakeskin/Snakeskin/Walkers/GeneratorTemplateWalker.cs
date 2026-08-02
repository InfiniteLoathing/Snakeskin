using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Directives;
using InfiniteLoathing.Snakeskin.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal class GeneratorTemplateWalker : TemplateWalker
    {
        protected readonly Stack<bool> RegionIsDirective = new Stack<bool>();

        public GeneratorTemplateWalker(SourceText sourceText, int templateStartIndex, string name)
            : base(sourceText, templateStartIndex, name)
        {
            RegionIsDirective.Push(true);
        }

        public override void VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
        {
            var regionText = GetRegionText(node);
            if (!IsDirective(regionText))
            {
                RegionIsDirective.Push(false);
                return;
            }
            
            RegionIsDirective.Push(true);

            var parent = Directives.Peek();
            var span = TextSpan.FromBounds(TextStartIndex, this.GetLineSpan(node).Start);
            if (span.Length != 0)
            {
                parent.Children.Add(new TextNode(SourceText.ToString(span)));
            }

            if (!DirectiveFactory.TryCreateFromRegion(regionText, out var directive))
            {
                throw new InvalidDirectiveException(regionText);
            }
            
            parent.Children.Add(directive);
            Directives.Push(directive);
            TextStartIndex = this.GetLineSpan(node).End;
        }

        public override void VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
        {
            if (!RegionIsDirective.Pop())
            {
                return;
            }
            
            var endingDirective = Directives.Pop();

            var span = TextSpan.FromBounds(TextStartIndex, this.GetLineSpan(node).Start);
            if (span.Length != 0)
            {
                endingDirective.Children.Add(new TextNode(SourceText.ToString(span)));
            }
            

            TextStartIndex = this.GetLineSpan(node).End;
            base.VisitEndRegionDirectiveTrivia(node);
        }

        public ITemplateNode GetResult()
        {
            var topLevelDirective = Directives.Pop();

            if (Directives.Count != 0)
            {
                throw new InvalidTemplateException("Template finalized with open region");
            }
            
            var span = TextSpan.FromBounds(TextStartIndex, SourceText.Length);

            if (span.Length != 0)
            {
                topLevelDirective.Children.Add(new TextNode(SourceText.ToString(span)));
            }

            return topLevelDirective;
        }
    }
}