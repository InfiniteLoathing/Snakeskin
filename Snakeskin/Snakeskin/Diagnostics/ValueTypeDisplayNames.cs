namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal static class ValueTypeDisplayNames
    {
        private const string StringName = "String";
        private const string ObjectName = "Object";
        private const string StringArrayName = "String[]";
        private const string ObjectArrayName = "Object[]";
        
        public static string Get(bool isObject, bool isArray)
        {
            if (isObject)
            {
                return isArray ? ObjectArrayName : ObjectName;
            }

            return isArray ? StringArrayName : StringName;
        }
    }
}