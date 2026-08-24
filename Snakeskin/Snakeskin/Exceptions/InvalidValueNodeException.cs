namespace InfiniteLoathing.Snakeskin.Exceptions
{
    internal class InvalidValueNodeException : InvalidTemplateException
    {
        public InvalidValueNodeException(string renderValueName) : base($"Invalid Value Node: {renderValueName}")
        {
            
        }
    }
}