namespace ViskeyTube.Wiki
{
    public interface IWikiClient
    {
        WikiPage GetPage(string pageId);
        WikiPageLight[] GetChildren(string pageId);
    }
}