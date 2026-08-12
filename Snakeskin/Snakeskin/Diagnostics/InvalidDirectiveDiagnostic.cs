namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class InvalidDirectiveDiagnostic : ITemplateDiagnostic
    {
        public string Name;

        public InvalidDirectiveDiagnostic(string name)
        {
            Name = name;
        }
    }
}