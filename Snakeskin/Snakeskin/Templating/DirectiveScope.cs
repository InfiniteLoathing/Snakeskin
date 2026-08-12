using System;
using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Diagnostics;
using InfiniteLoathing.Snakeskin.Exceptions;
using InfiniteLoathing.Snakeskin.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class SemanticScope
    {
        public IReadOnlyDictionary<string, string> ReplacementValueMap => _replacementValueMap;
        private Dictionary<string, string> _replacementValueMap;
        private Action<ITemplateDiagnostic, TextSpan> _handleDiagnostic;
        
        public SemanticScope(Action<ITemplateDiagnostic, TextSpan> handleDiagnostic)
        {
            _replacementValueMap = new Dictionary<string, string>();
            _handleDiagnostic = handleDiagnostic;
        }

        public ParentNode ValidateAndAdd(DirectiveSyntax directiveSyntax)
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

        private ParentNode AddToScope(ReplaceDirectiveSyntax replaceDirectiveSyntax)
        {
            throw new NotImplementedException();
        }
    }
}