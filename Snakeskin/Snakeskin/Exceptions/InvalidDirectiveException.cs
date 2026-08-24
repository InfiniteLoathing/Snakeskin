namespace InfiniteLoathing.Snakeskin.Exceptions
{
    internal class InvalidDirectiveException : InvalidTemplateException
    {
        public InvalidDirectiveException(string directiveName) : base($"Invalid Directive: {directiveName}")
        {
            
        }
    }
}