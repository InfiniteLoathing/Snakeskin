using System;

namespace InfiniteLoathing.Snakeskin.Extensions
{
    internal static class ValueDefinitionExtensions
    {
        public static string ToDiagnosticTypeName(this IValueDefinition valueDefinition)
        {
            switch (valueDefinition.Type)
            {
                case ValueType.String:
                    return valueDefinition.IsArray ? "String[]" : "String";
                case ValueType.Bool:
                    return valueDefinition.IsArray ? "Boolean[]" : "Boolean";
                case ValueType.Object:
                    return valueDefinition.IsArray ? "Object[]" : "Object";
                default:
                    throw new ArgumentOutOfRangeException(nameof(valueDefinition.Type), valueDefinition.Type, null);
            }
        }
    }
}