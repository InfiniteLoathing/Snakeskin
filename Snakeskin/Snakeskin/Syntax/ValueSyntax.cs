namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class ValueSyntax
    {
        public bool IsObject;
        public string ParentObject;
        public string Identifier;
        public bool IsArray;
        public string ReplacementText;

        public ValueSyntax(
            bool isObject,
            string parentObject,
            string identifier,
            bool isArray,
            string replacementText)
        {
            IsObject = isObject;
            ParentObject = parentObject;
            Identifier = identifier;
            IsArray = isArray;
            ReplacementText = replacementText;
        }
    }
}