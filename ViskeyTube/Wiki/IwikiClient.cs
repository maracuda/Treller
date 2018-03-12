namespace ViskeyTube.Wiki
{
    public interface IWikiClient
    {
        WikiPage GetPage(string pageId);
        string GetPageSource(string pageId);
        WikiPageLight[] GetChildren(string pageId);
        WikiPage UpdateTitleAndGetNewPage(string pageId, string newTitle);
    }
}