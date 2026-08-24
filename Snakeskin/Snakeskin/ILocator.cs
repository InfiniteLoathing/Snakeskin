using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin
{
    internal interface ILocator
    {
        Location Locate(TextSpan textSpan);

        IEnumerable<Location> Locate(IEnumerable<TextSpan> textSpans);
    }
}