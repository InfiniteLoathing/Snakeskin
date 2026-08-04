using System;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal ref struct RegionTextElement
    {
        public RegionTextElementKind Kind;

        public TextSpan LocationSpan;

        public ReadOnlySpan<char> Span;

        public RegionTextElement(RegionTextElementKind kind, TextSpan locationSpan, ReadOnlySpan<char> span)
        {
            Kind = kind;
            LocationSpan = locationSpan;
            Span = span;
        }
    }
}