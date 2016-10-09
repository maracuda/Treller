using System;
using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Reporters
{
    internal class SubstringParser
    {
        private readonly string startToken;
        private readonly string endToken;

        public SubstringParser(string startToken, string endToken = null)
        {
            this.startToken = startToken;
            this.endToken = endToken;
        }

        public Maybe<string> TryParse(string str)
        {
            var startTokenIndex = str.IndexOf(startToken, StringComparison.OrdinalIgnoreCase);
            if (startTokenIndex == -1)
                return null;

            var newSartIndex = startTokenIndex + startToken.Length;
            if (endToken == null)
                return str.Substring(newSartIndex).Trim();

            var endTokenStartIndex = str.IndexOf(endToken, newSartIndex, StringComparison.OrdinalIgnoreCase);
            return endTokenStartIndex == -1
                ? str.Substring(newSartIndex).Trim()
                : str.Substring(newSartIndex, endTokenStartIndex - newSartIndex).Trim();
        }
    }
}