using System;
using System.Linq;
using Xunit;
using SKBKontur.TaskManagerClient;
using Assert = SKBKontur.Treller.Tests.UnitWrappers.Assert;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.ClientTests
{
    public class TrelloClientTest : IntegrationTest
    {
        private const string testOrgId = "konturbilling";
        private ITaskManagerClient trelloClient;

        public TrelloClientTest() : base()
        {
            trelloClient = container.Get<ITaskManagerClient>();
        }

        [Fact]
        public void TestGetAllBoards()
        {
            var actualBoards = trelloClient.GetAllBoards(testOrgId);
            Assert.True(actualBoards.Length > 2);
            var actualOpenBoards = trelloClient.GetOpenBoards(testOrgId);
            var query1 = actualBoards.Select(x => x.Name);
            var query2 = actualOpenBoards.Select(x => x.Name);
            Assert.True(query2.All(i => query1.Contains(i)));
        }

        [Fact]
        public void TestGelAllBoardsFromCache()
        {
            var originalActuals = trelloClient.GetAllBoards(testOrgId);
            var actuals = trelloClient.GetAllBoards(testOrgId);
            Assert.AreEqual(originalActuals, actuals);
            actuals = trelloClient.GetAllBoards(testOrgId);
            Assert.AreEqual(originalActuals, actuals);
        }

        [Fact]
        public void TestGetClosedBoard()
        {
            Assert.True(trelloClient.GetAllBoards(testOrgId).Any(x => x.IsClosed));
        }

        [Fact]
        public void TestReadAllCardsForBoard()
        {
            var actualBoards = trelloClient.GetAllBoards(testOrgId);
            var firstBoard = actualBoards.First();
            var boardCards = trelloClient.GetBoardCardsAsync(new[] {firstBoard.Id}).Result;
            Assert.True(boardCards.Length > 1);
        }

        [Fact]
        public void TestReadListsForBoard()
        {
            var actualBoards = trelloClient.GetAllBoards(testOrgId);
            var firstBoard = actualBoards.First();
            var boardLists = trelloClient.GetBoardLists(firstBoard.Id);
            Assert.True(boardLists.Length >= 1);

            Console.WriteLine(actualBoards.Stringify());
            Console.WriteLine(boardLists.Stringify());
        }

        [Fact]
        public void TestGetCardById()
        {
            var actual = trelloClient.GetCard("lpDZvlGm");
            var x = 1;
        }
    }
}