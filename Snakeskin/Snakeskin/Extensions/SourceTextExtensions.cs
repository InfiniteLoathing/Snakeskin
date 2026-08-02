using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Extensions
{
    internal static class SourceTextExtensions
    {
        private const string NameCapturingGroup = "Name";
        private static readonly Regex TemplateExpression =
            new Regex($@"//\s*@template\s+(?<{NameCapturingGroup}>\w+)\s*$", RegexOptions.IgnoreCase);
        
        public static bool TryGetTemplateName(this SourceText sourceText, out string name)
        {
            if (sourceText.Lines.Count == 0)
            {
                name = null;
                return false;
            }

            var match = TemplateExpression.Match(sourceText.Lines[0].ToString());

            if (!match.Groups[NameCapturingGroup].Success)
            {
                name = null;
                return false;
            }

            name = match.Groups[NameCapturingGroup].Value;
            return true;
        }
    }
}