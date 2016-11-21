namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Parsing
{
    public class TokenParser : ITokenParser
    {
        private readonly SubstringParser commonParser;
        private readonly SubstringParser customParser;

        public TokenParser(string token)
        {
            commonParser = new SubstringParser($"**{token}**:", "**");
            customParser = new SubstringParser($"**{token}**:", "---");
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