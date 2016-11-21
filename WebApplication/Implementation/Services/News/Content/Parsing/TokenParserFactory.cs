namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Parsing
{
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
}