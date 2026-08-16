using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin
{
    internal class SyntaxTreeLocator
    {
        private readonly string _filePath;
        private readonly SourceText _sourceText;
        private readonly int _offset;
        
        public SyntaxTreeLocator(string filePath, SourceText sourceText, int offset)
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