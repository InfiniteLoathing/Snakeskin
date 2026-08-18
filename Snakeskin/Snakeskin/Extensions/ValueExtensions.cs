using InfiniteLoathing.Snakeskin.Templating;

namespace InfiniteLoathing.Snakeskin.Extensions
{
    // todo: finalize name
    internal static class ValueExtensions
    {
        private const string StringName = "String";
        private const string ObjectName = "Object";
        private const string StringArrayName = "String[]";
        private const string ObjectArrayName = "Object[]";

        public static string ToDiagnosticTypeName(this ITemplateValue templateValue)
        {
            if (templateValue.IsObject)
            {
                return templateValue.IsArray ? ObjectArrayName : ObjectName;
            }

            return templateValue.IsArray ? StringArrayName : StringName;
        }
    }
}