namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal class UnexpectedTokenDiagnostic : ITemplateDiagnostic
    {
        public TokenKind ActualKind;
        
        public TokenKind UnexpectedKind;

        public UnexpectedTokenDiagnostic(TokenKind actualKind, TokenKind unexpectedKind)
        {
            ActualKind = actualKind;
            UnexpectedKind = unexpectedKind;
        }
    }
}