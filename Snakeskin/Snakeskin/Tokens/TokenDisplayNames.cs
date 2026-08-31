using System;

namespace InfiniteLoathing.Snakeskin.Tokens
{
    internal static class TokenDisplayNames
    {
        public static string Get(TokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case TokenKind.At:
                    return "@";
                case TokenKind.Pound:
                    return "#";
                case TokenKind.QuestionMark:
                    return "?";
                case TokenKind.Dot:
                    return ".";
                case TokenKind.Colon:
                    return ":";
                case TokenKind.Comma:
                    return ",";
                case TokenKind.OpenBracket:
                    return "[";
                case TokenKind.Brackets:
                    return "[]";
                case TokenKind.In:
                    return "in";
                case TokenKind.String:
                    return "String";
                case TokenKind.QuotedString:
                    return "Quoted String";
                case TokenKind.OpenQuotedString:
                    return "Open Quoted String";
                case TokenKind.Invalid:
                    return "Unknown";
                case TokenKind.End:
                    return "End of Line";
                default:
                    throw new ArgumentOutOfRangeException(nameof(tokenKind), tokenKind, null);
            }
        }
    }
}