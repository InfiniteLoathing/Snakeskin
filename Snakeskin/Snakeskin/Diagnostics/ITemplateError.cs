using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal interface ITemplateError
    {
        Diagnostic CreateDiagnostic();
    }
}