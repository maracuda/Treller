namespace WebApplication.Implementation.Services.News.Content.Parsing
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
            motivationParser = new TokenParser("���������");
            analyticsParser = new TokenParser("���������");
            branchParser = new TokenParser("�����");
            publicInfoParser = new TokenParser("�������");
            techInfoParser = new TokenParser("����������� �������");
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