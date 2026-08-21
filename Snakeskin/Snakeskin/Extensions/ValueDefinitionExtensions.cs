namespace InfiniteLoathing.Snakeskin.Extensions
{

    internal static class ValueDefinitionExtensions
    {
        private const string StringName = "String";
        private const string ObjectName = "Object";
        private const string StringArrayName = "String[]";
        private const string ObjectArrayName = "Object[]";

        public static string ToDiagnosticTypeName(this IValueDefinition valueDefinition)
        {
            if (valueDefinition.IsObject)
            {
                return valueDefinition.IsArray ? ObjectArrayName : ObjectName;
            }

            return valueDefinition.IsArray ? StringArrayName : StringName;
        }
    }
}