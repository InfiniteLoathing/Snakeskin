using System;
using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Directives;
using InfiniteLoathing.Snakeskin.Exceptions;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal class TemplateScope
    {
        public IReadOnlyDictionary<string, string> ReplacementValueMap => _replacementValueMap;
        private Dictionary<string, string> _replacementValueMap;
        private Action<ITemplateDiagnostic, TextSpan> _handleDiagnostic;
        
        public TemplateScope(Action<ITemplateDiagnostic, TextSpan> handleDiagnostic)
        {
            _replacementValueMap = new Dictionary<string, string>();
            _handleDiagnostic = handleDiagnostic;
        }

        public TemplateContainer ValidateAndAdd(DirectiveSyntax directiveSyntax)
        {
            switch (directiveSyntax)
            {
                case ReplaceDirectiveSyntax replaceDirectiveSyntax:
                    return this.AddToScope(replaceDirectiveSyntax);
                default:
                    throw new InvalidTemplateException("Unsupported syntax type");
            }
        }

        public IEnumerable<ITemplateNode> RenderTextSection(string templateText)
        {
            yield break;
        }

        private TemplateContainer AddToScope(ReplaceDirectiveSyntax replaceDirectiveSyntax)
        {
            throw new NotImplementedException();
        }
    }
}