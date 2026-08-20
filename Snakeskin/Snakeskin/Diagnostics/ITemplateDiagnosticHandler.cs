using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal interface ITemplateDiagnosticHandler
    {
        void Handle(ITemplateDiagnostic diagnostic, TextSpan textSpan);
    }
}