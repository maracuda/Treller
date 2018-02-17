namespace ViskeyTube.Wiki
{
    public class WikiPage
    {
        public string Id { get; set; }
        public WikiPageBody Body { get; set; }
    }

    public class WikiPageBody
    {
        public WikiPageBodyStorage Storage { get; set; }
    }

    public class WikiPageBodyStorage
    {
        public string Value { get; set; }
    }
}