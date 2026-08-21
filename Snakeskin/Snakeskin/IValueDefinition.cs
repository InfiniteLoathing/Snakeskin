namespace InfiniteLoathing.Snakeskin
{
    internal interface IValueDefinition
    {
        bool IsObject { get; }
        bool IsArray { get; }
    }
}