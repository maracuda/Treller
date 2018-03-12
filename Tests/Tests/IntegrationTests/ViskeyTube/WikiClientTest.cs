using System;
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
            Assert.InRange(page.Version.Number, 1, int.MaxValue);
            Assert.NotEmpty(page.Title);
            Assert.NotEmpty(page.Space.Key);
            Assert.NotEmpty(page.Type);
        }

        [Fact]
        public void AbleToUpdateWikiGetPageTitle()
        {
            var wikiClient = new WikiClient(new JsonSerializer(), credentialsService.GetWikiCredentials().AuthHeader);

            var pageId = "156697011";

            var oldPage = wikiClient.GetPage(pageId);

            var newTitle =$"Запись на встречи. Сюда прилетало нло {DateTime.UtcNow}";

            var page = wikiClient.UpdateTitleAndGetNewPage(pageId, newTitle);

            Assert.NotEqual(oldPage.Version.Number, page.Version.Number);
            Assert.Equal(newTitle, page.Title);

            wikiClient.UpdateTitleAndGetNewPage(pageId, oldPage.Title);
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