using System;
using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Import
{
    internal class SubstringParser
    {
        private readonly string startToken;
        private readonly string endToken;

        public SubstringParser(string startToken, string endToken)
        {
            this.startToken = startToken;
            this.endToken = endToken;
        }

        public Maybe<string> TryParse(string str)
        {
            var newStartIndex = str.IndexOf(startToken, StringComparison.OrdinalIgnoreCase);
            if (newStartIndex == -1)
                return null;

            var newEndIndex = str.IndexOf(endToken, newStartIndex, StringComparison.OrdinalIgnoreCase);
            return newEndIndex == -1
                ? str.Substring(newStartIndex + startToken.Length).Trim()
                : str.Substring(newStartIndex + startToken.Length, newEndIndex - (newStartIndex + startToken.Length)).Trim();
        }
    }
}