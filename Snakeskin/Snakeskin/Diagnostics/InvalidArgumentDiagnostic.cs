using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class InvalidArgumentDiagnostic : ITemplateDiagnostic
    {
        public readonly string DirectiveName;

        public readonly bool IsObject;

        public readonly bool IsArray;

        public readonly Location Location;
        
        public InvalidArgumentDiagnostic(string directiveName, bool isObject, bool isArray, Location location)
        {
            DirectiveName = directiveName;
            IsObject = isObject;
            IsArray = isArray;
            Location = location;
        }

        public Diagnostic CreateDiagnostic()
        {
            var type = IsObject ? "Object" : "String";
            if (IsArray)
            {
                type += "[]";
            }

            return Diagnostic.Create(
                descriptor: DiagnosticDescriptors.InvalidArgument,
                location: Location,
                DirectiveName, type);
        }
    }
}