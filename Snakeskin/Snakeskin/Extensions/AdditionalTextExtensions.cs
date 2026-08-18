using System;
using System.IO;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Extensions
{
    internal static class AdditionalTextExtensions
    {
        private const string FileExtension = ".snakeskin.cs";

        public static bool IsSnakeskinTemplate(this AdditionalText text) =>
            text.Path.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase);

        public static string GetSnakeskinFileName(this AdditionalText text)
        {
            var filename = Path.GetFileName(text.Path);
            return filename.EndsWith(FileExtension)
                ? filename.Substring(0, filename.Length - FileExtension.Length)
                : filename;
        }
    }
}