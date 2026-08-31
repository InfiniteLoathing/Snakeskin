using System;

namespace InfiniteLoathing.Snakeskin.Extensions
{
    internal static class SpanExtensions
    {
        public static bool LowerInvariantSequenceEqual(this ReadOnlySpan<char> span, string expected)
        {
            if (span.Length != expected.Length)
            {
                return false;
            }

            for (var i = 0; i < expected.Length; i++)
            {
                if (char.ToLowerInvariant(span[i]) != expected[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}