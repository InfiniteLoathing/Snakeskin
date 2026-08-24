#if RENDER_TEMPLATE
using Snakeskin.Templates;

namespace InfiniteLoathing.Snakeskin.Sample
{
    internal class Property : TestTemplate.IProperties
    {
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
#endif