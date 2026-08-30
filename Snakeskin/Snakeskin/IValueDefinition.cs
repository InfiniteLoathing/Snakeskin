namespace InfiniteLoathing.Snakeskin
{
    internal interface IValueDefinition
    {
        ValueType Type { get; }
        bool IsArray { get; }
    }
}