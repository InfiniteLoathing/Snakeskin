using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Extensions;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class InvalidArgumentDiagnostic : ITemplateDiagnostic
    {
        private readonly string _directiveName;

        private readonly IValueDefinition _valueDefinition;
        
        public InvalidArgumentDiagnostic(string directiveName, IValueDefinition valueDefinition)
        {
            _directiveName = directiveName;
            _valueDefinition = valueDefinition;
        }

        public Diagnostic CreateDiagnostic(Location location, IEnumerable<Location> additionalLocations = null)
        {

            return Diagnostic.Create(
                descriptor: DiagnosticDescriptors.InvalidArgument,
                location: location,
                additionalLocations: additionalLocations,
                _directiveName,
                _valueDefinition.ToDiagnosticTypeName());
        }
    }
}