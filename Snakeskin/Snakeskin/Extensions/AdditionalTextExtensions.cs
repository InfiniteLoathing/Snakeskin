using System;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Extensions
{
    internal static class AdditionalTextExtensions
    {
        private const string FileExtension = ".snakeskin.cs";

        public static bool IsSnakeskinTemplate(this AdditionalText text) =>
            text.Path.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase);
    }
}