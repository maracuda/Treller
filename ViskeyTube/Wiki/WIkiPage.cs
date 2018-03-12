using Newtonsoft.Json;

namespace ViskeyTube.Wiki
{
    public class WikiPage : WikiPageLight
    {
        public WikiPageBody Body { get; set; }
    }

    public class WikiPageLight
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("version")]
        public WikiPageVersion Version { get; set; }
        [JsonProperty("space")]
        public WikiPageSpace Space { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class WikiPageBody
    {
        public WikiPageBodyStorage Storage { get; set; }
        public WikiPageBodyStorage View { get; set; }
        public WikiPageBodyStorage Styled_View { get; set; }
    }

    public class WikiPageBodyStorage
    {
        public string Value { get; set; }
    }

    public class WikiPageSpace
    {
        [JsonProperty("key")]
        public string Key { get; set; }
    }

    public class WikiPageVersion
    {
        [JsonProperty("number")]
        public int Number { get; set; }
    }

    public class WikiPageSearchResult
    {
        public WikiPageLight[] Results { get; set; }
    }
}