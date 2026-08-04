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
            var regionText = RegionTextReader.GetRegionText(node);
            if (!regionText.IsDirective(out var regionIndex))
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


            var directiveText = regionText.GetDirective(regionIndex);
            var directiveArguments = regionText.GetArguments(directiveText.Index).ToList();
            var regionMatch = RegionTextReader.MatchRegionDirective(regionText);

            if (!regionMatch.Directive.Success)
            {
                throw new InvalidDirectiveException(regionText);
            }

            if (!RegionTextReader.TryParseDirective(regionMatch.Directive, out var directive))
            {
                throw new InvalidDirectiveException(regionMatch.Directive.Value);
            }

            if (directive.HasValues)
            {
                if (regionMatch.Values.Captures.Count == 0)
                {
                    throw new InvalidDirectiveException($"{directive.Kind} requires value arguments");
                }
                
                var validValues = new List<ValueNode>();
            
                foreach (Capture capture in regionMatch.Values.Captures)
                {
                    var value = RegionTextReader.CreateValue(capture.Value);
                    if (!SyntaxFacts.IsValidIdentifier(value.Identifier))
                    {
                        throw new InvalidValueNodeException($"{value} is not a legal identifier");
                    }

                    if (!directive.SupportsValueKind(value.ValueKind, value.IsArray))
                    {
                        throw new InvalidValueNodeException(
                            $"{value.ValueKind}{(value.IsArray ? "[]" : string.Empty)} is not of a legal kind for {directive.Kind} directives");
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

            this.RecalculateValuePattern();
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
                this.RecalculateValuePattern();
            }
        }

        public ITemplateNode Complete()
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

        private IEnumerable<ITemplateNode> ProcessTemplateText(TextSpan span)
        {
            if (ValuePattern == string.Empty)
            {
                yield break;
            }

            var t = SourceText.ToString(span);
            
            foreach (var segment in Regex.Split(SourceText.ToString(span), ValuePattern))
            {
                if (Values.TryGetValue(segment, out var value))
                {
                    yield return value;
                }
                else
                {
                    yield return new TextNode(segment);
                }
            }
        }
    }
}