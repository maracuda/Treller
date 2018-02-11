using ViskeyTube.Wiki;
using Xunit;

namespace Tests.Tests.IntegrationTests.ViskeyTube
{
    public class WikiClientTest
    {
        [Fact]
        public void AbleToWiki()
        {
            var wikiClient = new WikiClient();
            var s = wikiClient.GetPage();
        }
    }
}