using System;
using System.Linq;
using SKBKontur.HttpInfrastructure.Clients;
using Xunit;
using SKBKontur.TaskManagerClient;
using Assert = SKBKontur.Treller.Tests.UnitWrappers.Assert;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.ClientTests
{
    public class TrelloClientTest : IntegrationTest
    {
        private const string testOrgId = "konturbilling";
        private readonly ITaskManagerClient trelloClient;

        public TrelloClientTest()
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
        public void ItCanLoadParticularCard()
        {
            var actual = trelloClient.GetCard("lpDZvlGm");
            Assert.IsNotNull(actual);
        }

        [Fact]
        public void ItCanLoadAllCardActionsAndFilterListMovements()
        {
            var actions = AsyncHelpers.RunSync(() => trelloClient.GetCardActionsAsync("9JRBHBaL"));
            var actual = actions.Where(x => x.ToList != null && x.FromList != null)
                .Select(x => new CardMovementModel
                {
                    FromList = x.FromList.Name,
                    ToList = x.ToList.Name,
                    Date = x.Date
                })
                .OrderBy(x => x.Date)
                .ToArray();
            Assert.True(actual.Length > 0);
        }

        class CardMovementModel
        {
            public string FromList { get; set; }
            public string ToList { get; set; }
            public DateTime Date { get; set; }
        }
    }
}