using Serialization;
using ViskeyTube.Wiki;
using Xunit;

namespace Tests.Tests.IntegrationTests.ViskeyTube
{
    public class WikiClientTest : IntegrationTest
    {
        [Fact]
        public void AbleToWikiGetPage()
        {
            var wikiClient = new WikiClient(new JsonSerializer(), credentialsService.GetWikiCredentials().AuthHeader);
            var page = wikiClient.GetPage("156696999");
            Assert.NotEmpty(page.Body.Storage.Value);
        }

        [Fact]
        public void AbleToWikiGetPageSource()
        {
            var wikiClient = new WikiClient(new JsonSerializer(), credentialsService.GetWikiCredentials().AuthHeader);
            var page = wikiClient.GetPageSource("156696999");
            Assert.NotEmpty(page);
        }

        [Fact]
        public void AbleToWikiGetChildren()
        {
            var wikiClient = new WikiClient(new JsonSerializer(), credentialsService.GetWikiCredentials().AuthHeader);
            var children = wikiClient.GetChildren("156696999");
            Assert.NotEmpty(children);
        }

        [Fact]
        public void AbleToWikiGetChildrenNonExists()
        {
            var wikiClient = new WikiClient(new JsonSerializer(), credentialsService.GetWikiCredentials().AuthHeader);
            var children = wikiClient.GetChildren("4242442414242421");
            Assert.Empty(children);
        }

        [Fact]
        public void AbleToWikiGetPageNotExists()
        {
            var wikiClient = new WikiClient(new JsonSerializer(), credentialsService.GetWikiCredentials().AuthHeader);
            var page = wikiClient.GetPage("4242442414242421");
            Assert.Null(page);
        }
    }
}