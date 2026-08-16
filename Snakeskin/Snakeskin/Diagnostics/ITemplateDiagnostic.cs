using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal interface ITemplateDiagnostic
    {
        Diagnostic CreateDiagnostic();
    }
}