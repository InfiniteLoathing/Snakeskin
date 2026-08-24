using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin
{
    internal class SourceTextLocator : ILocator
    {
        private readonly string _filePath;
        private readonly SourceText _sourceText;
        
        public SourceTextLocator(string filePath, SourceText sourceText)
        {
            _filePath = filePath;
            _sourceText = sourceText;
        }

        public Location Locate(TextSpan textSpan)
        {
            var offsetSpan = new TextSpan(textSpan.Start, textSpan.Length);
            return Location.Create(_filePath, offsetSpan, _sourceText.Lines.GetLinePositionSpan(offsetSpan));
        }

        public IEnumerable<Location> Locate(IEnumerable<TextSpan> textSpans) => textSpans.Select(this.Locate);
    }
}