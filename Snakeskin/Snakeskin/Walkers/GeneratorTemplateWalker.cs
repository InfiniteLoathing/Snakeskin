using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using InfiniteLoathing.Snakeskin.Directives;
using InfiniteLoathing.Snakeskin.Exceptions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal class GeneratorTemplateWalker : TemplateWalker
    {
        public GeneratorTemplateWalker(SourceText sourceText, int templateCursor, string name)
            : base(sourceText, templateCursor, name)
        {
            NestedRegionIsDirective.Push(true);
        }

        public override void VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
        {
            var regionText = TemplateReader.GetRegionText(node);
            if (!TemplateReader.IsDirective(regionText))
            {
                NestedRegionIsDirective.Push(false);
                return;
            }

            NestedRegionIsDirective.Push(true);

            var parent = NestedDirectives.Peek();
            var span = TextSpan.FromBounds(TemplateTextCursor, this.GetLineSpan(node).Start);
            if (span.Length != 0)
            {
                parent.Children.AddRange(this.ProcessTemplateText(span));
            }

            var regionMatch = TemplateReader.MatchRegionDirective(regionText);

            if (!regionMatch.Directive.Success)
            {
                throw new InvalidDirectiveException(regionText);
            }

            if (!TemplateReader.TryParseDirective(regionMatch.Directive, out var directive))
            {
                throw new InvalidDirectiveException(regionMatch.Directive.Value);
            }

            if (directive.HasValues)
            {
                var validValues = new List<ValueNode>();
            
                foreach (Capture capture in regionMatch.Values.Captures)
                {
                    var value = TemplateReader.CreateValue(capture.Value);
                    if (!SyntaxFacts.IsValidIdentifier(value.Identifier))
                    {
                        throw new InvalidValueNodeException($"{value} is not a legal identifier");
                    }

                    if (!directive.SupportsValueKind(value.ValueKind))
                    {
                        throw new InvalidValueNodeException(
                            $"{value.ValueKind} is not of a legal kind for {directive.Kind} directives");
                    }

                    if (validValues.Any(x => x.Identifier == value.Identifier))
                    {
                        throw new InvalidValueNodeException(
                            $"Value with identifier {value.Identifier} is already declared");
                    }
                
                    validValues.Add(value);
                }

                foreach (var validValue in validValues)
                {
                    if (Values.ContainsKey(validValue.Identifier))
                    {
                        throw new InvalidValueNodeException(
                            $"Value with identifier {validValue.Identifier} is already declared in the current context");
                    }

                    if (ValueKinds.ContainsKey(validValue.Identifier))
                    {
                        if (ValueKinds[validValue.Identifier] != validValue.ValueKind)
                        {
                            throw new InvalidValueNodeException(
                                $"Value with identifier {validValue.Identifier} is already declared in the template with a different type ({ValueKinds[validValue.Identifier]})");
                        }
                    }
                    else
                    {
                        ValueKinds[validValue.Identifier] = validValue.ValueKind;
                    }
                    
                    Values.Add(validValue.Identifier, validValue);
                    NestedValues.Push(validValues.Select(x => x.Identifier).ToImmutableArray());
                }
            }
            else
            {
                if (regionMatch.Values.Success
                    && regionMatch.Values.Captures.Count > 0
                    && regionMatch.Values.Captures.Cast<Capture>().Any(x => x.Value.Trim().Length != 0))
                {
                    throw new InvalidValueNodeException(
                        $"{directive.Kind} does not accept values");
                }
            }

            parent.Children.Add(directive);
            NestedDirectives.Push(directive);
            TemplateTextCursor = this.GetLineSpan(node).End;
        }

        public override void VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
        {
            if (!NestedRegionIsDirective.Pop())
            {
                return;
            }
            
            var endingDirective = NestedDirectives.Pop();

            var span = TextSpan.FromBounds(TemplateTextCursor, this.GetLineSpan(node).Start);
            if (span.Length != 0)
            {
                endingDirective.Children.AddRange(this.ProcessTemplateText(span));
            }
            
            TemplateTextCursor = this.GetLineSpan(node).End;

            if (endingDirective.HasValues)
            {
                foreach (var endingValue in NestedValues.Pop())
                {
                    Values.Remove(endingValue);
                }
            }
        }

        public ITemplateNode GetResult()
        {
            var topLevelDirective = NestedDirectives.Pop();

            if (NestedDirectives.Count != 0)
            {
                throw new InvalidTemplateException("Template finalized with open region");
            }
            
            var span = TextSpan.FromBounds(TemplateTextCursor, SourceText.Length);

            if (span.Length != 0)
            {
                topLevelDirective.Children.AddRange(this.ProcessTemplateText(span));
            }

            return topLevelDirective;
        }

        private IEnumerable<ITemplateNode> ProcessTemplateText(TextSpan textSpan)
        {
            yield return new TextNode(SourceText.ToString(textSpan));
        }
    }
}