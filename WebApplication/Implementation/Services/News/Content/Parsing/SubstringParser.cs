using System;
using Infrastructure.Sugar;

namespace WebApplication.Implementation.Services.News.Content.Parsing
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