using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal interface ITemplateDiagnostic
    {
        Diagnostic CreateDiagnostic(Location location, IEnumerable<Location> additionalLocations = null);
    }
}