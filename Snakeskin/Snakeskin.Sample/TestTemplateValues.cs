#if RENDER_TEMPLATE
using Snakeskin.Templates;

namespace InfiniteLoathing.Snakeskin.Sample
{
    internal class TestTemplateValues : TestTemplate.ITemplateValues
    {
        public TestTemplate.ITestObj TestObj { get; set; }
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        
        public string[] Strings { get; set; }
        
        public TestTemplate.IProperties[] Properties { get; set; }
    }
}
#endif