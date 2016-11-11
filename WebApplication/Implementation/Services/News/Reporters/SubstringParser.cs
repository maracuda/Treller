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

    public interface ITokenParser
    {
        string TryParse(string str, string defaultValue);
    }

    public interface ITokenParserFactory
    {
        ITokenParser GetMotivationParser();
        ITokenParser GetAnalyticsParser();
        ITokenParser GetBranchParser();
        ITokenParser GetPublicInfoParser();
        ITokenParser GetTechInfoParser();
    }

    public class TokenParserFactory : ITokenParserFactory
    {
        private readonly TokenParser motivationParser;
        private readonly TokenParser analyticsParser;
        private readonly TokenParser branchParser;
        private readonly TokenParser publicInfoParser;
        private readonly TokenParser techInfoParser;

        public TokenParserFactory()
        {
            motivationParser = new TokenParser("Мотивация");
            analyticsParser = new TokenParser("Аналитика");
            branchParser = new TokenParser("Ветка");
            publicInfoParser = new TokenParser("Новости");
            techInfoParser = new TokenParser("Технические новости");
        }

        public ITokenParser GetMotivationParser()
        {
            return motivationParser;
        }

        public ITokenParser GetAnalyticsParser()
        {
            return analyticsParser;
        }

        public ITokenParser GetBranchParser()
        {
            return branchParser;
        }

        public ITokenParser GetPublicInfoParser()
        {
            return publicInfoParser;
        }

        public ITokenParser GetTechInfoParser()
        {
            return techInfoParser;
        }
    }

    public class TokenParser : ITokenParser
    {
        private readonly SubstringParser commonParser;
        private readonly SubstringParser customParser;

        public TokenParser(string token)
        {
            commonParser = new SubstringParser($"**{token}**:", "**");
            customParser = new SubstringParser($"**{token}**:", "---");
        }

        public Maybe<string> TryParse(string str)
        {
            var commonResult = commonParser.TryParse(str);
            var customResult = customParser.TryParse(str);

            if (customResult.HasValue && customResult.HasValue && customResult.Value.Length < commonResult.Value.Length)
                return customResult;
            return commonResult;
        }

        public string TryParse(string str, string defaultValue)
        {
            var commonResult = commonParser.TryParse(str);
            var customResult = customParser.TryParse(str);

            if (customResult.HasValue && customResult.HasValue && customResult.Value.Length < commonResult.Value.Length)
                return customResult.Value;
            return commonResult.HasValue ? commonResult.Value : defaultValue;
        }
    }
}