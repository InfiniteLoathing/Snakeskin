namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal interface ITemplateDiagnosticHandler
    {
        void Handle(ITemplateDiagnostic diagnostic);
    }
}