namespace InfiniteLoathing.Snakeskin.Templating
{
    internal interface ITemplateValue
    {
        bool IsObject { get; }
        bool IsArray { get; }
    }
}