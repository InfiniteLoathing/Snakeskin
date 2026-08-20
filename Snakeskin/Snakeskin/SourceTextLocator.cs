using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin
{
    internal class SourceTextLocator : ILocator
    {
        private readonly string _filePath;
        private readonly SourceText _sourceText;
        private readonly int _offset;
        
        public SourceTextLocator(string filePath, SourceText sourceText, int offset = 0)
        {
            _filePath = filePath;
            _sourceText = sourceText;
            _offset = offset;
        }

        public Location Locate(TextSpan textSpan)
        {
            var offsetSpan = new TextSpan(_offset + textSpan.Start, textSpan.Length);
            return Location.Create(_filePath, offsetSpan, _sourceText.Lines.GetLinePositionSpan(offsetSpan));
        }
    }
}