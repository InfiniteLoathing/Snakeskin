using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class InvalidArgumentDiagnostic : ITemplateDiagnostic
    {
        public readonly string DirectiveName;

        public readonly bool IsObject;

        public readonly bool IsArray;
        
        public InvalidArgumentDiagnostic(string directiveName, bool isObject, bool isArray)
        {
            DirectiveName = directiveName;
            IsObject = isObject;
            IsArray = isArray;
        }

        public Diagnostic CreateDiagnostic(Location location, IEnumerable<Location> additionalLocations = null)
        {
            var type = IsObject ? "Object" : "String";
            if (IsArray)
            {
                type += "[]";
            }

            return Diagnostic.Create(
                descriptor: DiagnosticDescriptors.InvalidArgument,
                location: location,
                additionalLocations: additionalLocations,
                DirectiveName, type);
        }
    }
}