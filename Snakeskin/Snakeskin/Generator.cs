using System.Text;
using InfiniteLoathing.Snakeskin.Exceptions;
using InfiniteLoathing.Snakeskin.Extensions;
using InfiniteLoathing.Snakeskin.Walkers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace InfiniteLoathing.Snakeskin
{
    [Generator]
    public class Generator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var templates = context.AdditionalTextsProvider
                .Where(x => x.IsSnakeskinTemplate());
            
            context.RegisterSourceOutput(templates, WriteTemplate);
        }

        private static void WriteTemplate(SourceProductionContext context, AdditionalText template)
        {
            var sourceText = template.GetText(context.CancellationToken);


            if (sourceText is null || !sourceText.TryGetTemplateName(out var name))
            {
                return;
            }

            if (!CSharpSyntaxTree
                .ParseText(sourceText, cancellationToken: context.CancellationToken)
                .TryGetRoot(out var root))
            {
                return;
            }

            if (!root.RegionsAreValid())
            {
                return;
            }
            
            var visitor = new GeneratorTemplateWalker(sourceText, sourceText.Lines[0].EndIncludingLineBreak, name);

            try
            {
                visitor.Visit(root);
                var res = visitor.Complete();
                var text = res.Render(new StringBuilder()).ToString();
            }
            catch (InvalidTemplateException)
            {
                return;
            }
        }
    }
}