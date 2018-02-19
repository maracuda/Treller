namespace ViskeyTube.Wiki
{
    public class WikiPage : WikiPageLight
    {
        public WikiPageBody Body { get; set; }
    }

    public class WikiPageLight
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }

    public class WikiPageBody
    {
        public WikiPageBodyStorage Storage { get; set; }
    }

    public class WikiPageBodyStorage
    {
        public string Value { get; set; }
    }

    public class WikiPageSearchResult
    {
        public WikiPageLight[] Results { get; set; }
    }
}