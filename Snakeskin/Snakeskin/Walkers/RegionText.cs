using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal readonly struct RegionText
    {
        private readonly SyntaxTree _syntaxTree;
        
        public readonly Location Location;
        
        public readonly string Value;
        
        public RegionText(SyntaxTree syntaxTree, Location location, string value)
        {
            _syntaxTree = syntaxTree;
            Location = location;
            Value = value;
        }

        public static implicit operator string(RegionText regionText) => regionText.Value;

        public bool IsDirective(out int directiveIndex)
        {
            for (var i = 0; i < this.Value.Length; i++)
            {
                if (char.IsWhiteSpace(this.Value[i]))
                {
                    continue;
                }

                directiveIndex = i + 1;
                return Value[i] == '@';
            }

            directiveIndex = -1;
            return false;
        }

        public RegionTextSegment GetDirective(int index)
        {
            var span = Value.AsSpan(index);
            var cursor = 0;

            while (cursor < span.Length && !char.IsWhiteSpace(span[cursor]))
            {
                cursor++;
            }
            
            return new RegionTextSegment(index + cursor, span.Slice(0, cursor).ToString());
        }

        public IEnumerable<RegionTextSegment> GetArguments(int startIndex)
        {
            var cursor = startIndex;
            while (cursor < Value.Length)
            {
                do
                {
                    if (!char.IsWhiteSpace(Value[cursor]))
                    {
                        break;
                    }

                    cursor++;
                } while (cursor < Value.Length);

                if (cursor >= Value.Length)
                {
                    yield break;
                }

                var start = cursor;
                
                while (cursor < Value.Length && Value[cursor] != ',')
                {
                    cursor++;
                }

                var end = cursor;
                while (end > start && char.IsWhiteSpace(Value[end - 1]))
                {
                    end--;
                }

                if (end > start)
                {
                    yield return new RegionTextSegment(start, Value.Substring(start, end - start));
                }

                cursor++;
            }
        }

        public Location GetSubLocation(Match match) =>
            Location.Create(_syntaxTree, new TextSpan(Location.SourceSpan.Start + match.Index, match.Length));
    }
}