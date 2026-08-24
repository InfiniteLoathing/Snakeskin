using System;
using System.IO;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace InfiniteLoathing.Snakeskin
{
    internal static class TemplateChecker
    {
        private const string FileExtension = ".snakeskin.cs";
        
        public static bool IsTemplate(AdditionalText additionalText) => 
            additionalText.Path.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase);
        
        public static bool IsTemplate(SyntaxTree syntaxTree) => 
            syntaxTree.FilePath.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase);
        
        public static bool IsTemplate(SyntaxNode syntaxNode, CancellationToken _) =>
            syntaxNode.IsKind(SyntaxKind.CompilationUnit)
            && syntaxNode.SyntaxTree.FilePath.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase);

        public static string GetFileNameWithoutExtension(string path)
        {
            var filename = Path.GetFileName(path);
            return filename.EndsWith(FileExtension)
                ? filename.Substring(0, filename.Length - FileExtension.Length)
                : filename;
        }
    }
}