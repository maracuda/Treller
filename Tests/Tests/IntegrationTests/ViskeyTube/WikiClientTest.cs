using Serialization;
using ViskeyTube.Wiki;
using Xunit;

namespace Tests.Tests.IntegrationTests.ViskeyTube
{
    public class WikiClientTest : IntegrationTest
    {
        [Fact]
        public void AbleToWiki()
        {
            var wikiClient = new WikiClient(new JsonSerializer(), credentialsService.GetWikiCredentials().AuthHeader);
            var s = wikiClient.GetPage("156696999");
            Assert.NotEmpty(s.Body.Storage.Value);
        }
    }
}