namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Parsing
{
    public interface ITokenParserFactory
    {
        ITokenParser GetMotivationParser();
        ITokenParser GetAnalyticsParser();
        ITokenParser GetBranchParser();
        ITokenParser GetPublicInfoParser();
        ITokenParser GetTechInfoParser();
    }
}