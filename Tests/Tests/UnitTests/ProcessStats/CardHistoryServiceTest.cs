using System;
using ProcessStats.Dev;
using Rhino.Mocks;
using TaskManagerClient;
using TaskManagerClient.BusinessObjects.TaskManager;
using TaskManagerClient.Trello.BusinessObjects.Actions;
using Xunit;

namespace Tests.Tests.UnitTests.ProcessStats
{
    public class CardHistoryServiceTest :  UnitTest
    {
        private readonly ITaskManagerClient taskMamanagerClient;
        private readonly CardHistoryService cardHistoryService;

        public CardHistoryServiceTest()
        {
            taskMamanagerClient = mockRepository.Create<ITaskManagerClient>();

            cardHistoryService = new CardHistoryService(taskMamanagerClient);
        }

        [Fact]
        public void ServiceBuildNoMovementsWhenNoMovementActionsAtCard()
        {
            var actionWithNoFromAndTo = new CardAction();
            var actionWithNoFrom = new CardAction {ToList = new ActionList()};
            var actionWithNoTo = new CardAction {FromList = new ActionList()};
            var cardId = DataGenerator.GenEnglishString(10);

            using (mockRepository.Record())
            {
                taskMamanagerClient.Expect(f => f.GetCardUpdateActions(cardId)).Return(new[] { actionWithNoFrom, actionWithNoFromAndTo, actionWithNoTo });
            }

            var actual = cardHistoryService.Get(cardId);
            Assert.Equal(0, actual.Movements.Length);
        }

        [Fact]
        public void ServiceCorrectlyBuildSingleMovement()
        {
            var actionDate = DateTime.Now;
            var action = new CardAction { FromList = new ActionList { Id = "id1", Name = "fromList"}, ToList = new ActionList { Id = "Id2", Name = "toList"}, Date = actionDate};
            var cardId = DataGenerator.GenEnglishString(10);

            using (mockRepository.Record())
            {
                taskMamanagerClient.Expect(f => f.GetCardUpdateActions(cardId)).Return(new[] { action });
            }

            var actual = cardHistoryService.Get(cardId);
            Assert.Equal(1, actual.Movements.Length);
            Assert.Equal("id1", actual.Movements[0].FromListId);
            Assert.Equal("id2", actual.Movements[0].ToListId);
            Assert.Equal(actionDate, actual.Movements[0].Date);
        }

        [Fact]
        public void ServiceOrderMovementsByMovementsDate()
        {
            var firstActionDate = DateTime.Now;
            var secondActionDate = DateTime.Now.AddDays(-1);
            var firstAction = new CardAction { FromList = new ActionList { Name = "fromList" }, ToList = new ActionList { Name = "toList" }, Date = firstActionDate };
            var secondAction = new CardAction { FromList = new ActionList { Name = "fromList" }, ToList = new ActionList { Name = "toList" }, Date = secondActionDate };
            var cardId = DataGenerator.GenEnglishString(10);

            using (mockRepository.Record())
            {
                taskMamanagerClient.Expect(f => f.GetCardUpdateActions(cardId)).Return(new[] { firstAction, secondAction });
            }

            var actual = cardHistoryService.Get(cardId);
            Assert.Equal(2, actual.Movements.Length);
            Assert.Equal(secondActionDate, actual.Movements[0].Date);
            Assert.Equal(firstActionDate, actual.Movements[1].Date);
        }
    }
}
