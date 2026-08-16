namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class InvalidValueTypeDiagnostic : ITemplateDiagnostic
    {
        public string DirectiveName { get; }
        
        public bool IsObject { get; }
        
        public bool IsArray { get; }
        
        public InvalidValueTypeDiagnostic(string directiveName, bool isObject, bool isArray)
        {
            this.DirectiveName = directiveName;
            this.IsObject = isObject;
            this.IsArray = isArray;
        }
    }
}