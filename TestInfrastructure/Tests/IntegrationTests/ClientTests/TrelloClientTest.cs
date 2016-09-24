using System;
using System.Linq;
using NUnit.Framework;
using SKBKontur.TaskManagerClient;
using Assert = SKBKontur.Treller.Tests.UnitWrappers.Assert;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.ClientTests
{
    public class TrelloClientTest : IntegrationTest
    {
        private const string testOrgId = "konturbilling";
        private ITaskManagerClient trelloClient;

        public override void SetUp()
        {
            base.SetUp();

            trelloClient = container.Get<ITaskManagerClient>();
        }

        [Test]
        public void TestGetAllBoards()
        {
            var actualBoards = trelloClient.GetAllBoards(testOrgId);
            Assert.True(actualBoards.Length > 2);
            var actualOpenBoards = trelloClient.GetOpenBoards(testOrgId);
            Assert.True(actualOpenBoards.Length > 2);
            CollectionAssert.IsSubsetOf(actualOpenBoards.Select(x => x.Name), actualBoards.Select(x => x.Name));
        }

        [Test]
        public void TestGelAllBoardsFromCache()
        {
            var originalActuals = trelloClient.GetAllBoards(testOrgId);
            var actuals = trelloClient.GetAllBoards(testOrgId);
            Assert.AreEqual(originalActuals, actuals);
            actuals = trelloClient.GetAllBoards(testOrgId);
            Assert.AreEqual(originalActuals, actuals);
        }

        [Test]
        public void TestGetClosedBoard()
        {
            Assert.True(trelloClient.GetAllBoards(testOrgId).Any(x => x.IsClosed));
        }

        [Test]
        public void TestReadAllCardsForBoard()
        {
            var actualBoards = trelloClient.GetAllBoards(testOrgId);
            var firstBoard = actualBoards.First();
            var boardCards = trelloClient.GetBoardCardsAsync(new[] {firstBoard.Id}).Result;
            Assert.True(boardCards.Length > 1);
        }

        [Test]
        public void TestReadListsForBoard()
        {
            var actualBoards = trelloClient.GetAllBoards(testOrgId);
            var firstBoard = actualBoards.First();
            var boardLists = trelloClient.GetBoardLists(firstBoard.Id);
            Assert.True(boardLists.Length >= 1);

            Console.WriteLine(actualBoards.Stringify());
            Console.WriteLine(boardLists.Stringify());
        }
    }
}