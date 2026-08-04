using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal ref struct RegionTextParser
    {
        private const char DirectivePrefix = '@';
        private const char ValuePlaceholderPrefix = ':';
        private const char ValueSeparator = ',';
        private const char Quote = '"';
        
        private readonly int _regionStart;
        private readonly ReadOnlySpan<char> _span;
        private int _cursor;
        
        public bool Complete => _cursor >= _span.Length;
        
        private char Current => _span[_cursor];

        private void Advance(int count = 1) => _cursor += count;

        private bool TryPeek(out char value)
        {
            if (_cursor + 1 < _span.Length)
            {
                value = _span[_cursor + 1];
                return true;
            }

            value = '\0';
            return false;
        }
        
        public RegionTextParser(SyntaxTriviaList list)
        {
            _regionStart = list.First().Span.Start;
            _span = list.ToFullString().AsSpan();
            _cursor = 0;
        }

        public RegionTextElementKind GetNextKind()
        {
            this.SkipWhitespace();
            switch (this.Current)
            {
                case DirectivePrefix:
                    return RegionTextElementKind.Directive;
                case ValuePlaceholderPrefix:
                    return RegionTextElementKind.ValuePlaceholder;
                case ValueSeparator:
                    return RegionTextElementKind.ValueSeparator;
                default:
                    return RegionTextElementKind.ValueIdentifier;
            }
        }

        public RegionTextElement TakeDirective()
        {
            Debug.Assert(this.Current == DirectivePrefix);
            this.Advance();
            var start = _cursor;

            while (!this.Complete && !char.IsWhiteSpace(this.Current))
            {
                this.Advance();
            }
            
            return new RegionTextElement
            {
                Kind = RegionTextElementKind.Directive,
                LocationSpan = TextSpan.FromBounds(_regionStart + start, _regionStart + _cursor),
                Span = _span.Slice(start, _cursor - start)
            };
        }

        public RegionTextElement TakeValueIdentifier()
        {
            var start = _cursor;

            while (!this.Complete
                   && (this.Current == ValueSeparator
                       || this.Current == ValuePlaceholderPrefix
                       || !char.IsWhiteSpace(this.Current)))
            {
                this.Advance();
            }

            return new RegionTextElement
            {
                Kind = RegionTextElementKind.ValueIdentifier,
                LocationSpan = TextSpan.FromBounds(_regionStart + start, _regionStart + _cursor),
                Span = _span.Slice(start, _cursor - start)
            };
        }

        public RegionTextElement TakeValuePlaceholder()
        {
            Debug.Assert(this.Current == ValuePlaceholderPrefix);
            this.Advance();
            var start = _cursor;
            
            // Advance passed PlaceholderPrefix
            this.Advance();

            if (this.Current == Quote)
            {
                this.Advance();

                while (!this.Complete)
                {
                    if (this.Current == Quote)
                    {
                        if (this.TryPeek(out var next) && next == Quote)
                        {
                            // Quote mark is escaped
                            this.Advance(2);
                            continue;
                        }
                        
                        // Quotes are closed
                        this.Advance();
                        break;
                    }
                    
                    this.Advance();
                }
            }
            else
            {
                while (!this.Complete
                       && (this.Current == ValueSeparator
                           || !char.IsWhiteSpace(this.Current)))
                {
                    this.Advance();
                }
            }

            return new RegionTextElement
            {
                Kind = RegionTextElementKind.ValuePlaceholder,
                LocationSpan = TextSpan.FromBounds(_regionStart + start, _regionStart + _cursor),
                Span = _span.Slice(start, _cursor - start)
            };
        }

        public void SkipValueSeparator()
        {
            Debug.Assert(this.Current == ValueSeparator);
            this.Advance();
        }

        private void SkipWhitespace()
        {
            while (!this.Complete || char.IsWhiteSpace(this.Current))
            {
                this.Advance();
            }
        }
    }
}