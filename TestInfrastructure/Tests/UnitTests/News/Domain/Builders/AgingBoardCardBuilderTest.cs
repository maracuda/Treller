using System;
using NUnit.Framework;
using Rhino.Mocks;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Builders;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Models;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.News.Domain.Builders
{
    public class AgingBoardCardBuilderTest : UnitTest
    {
        private ITaskManagerClient taskManagerClient;
        private IErrorService errorService;
        private AgingBoardCardBuilder agingBoardCardBuilder;

        public override void SetUp()
        {
            base.SetUp();

            taskManagerClient = mock.Create<ITaskManagerClient>();
            errorService = mock.Create<IErrorService>();

            agingBoardCardBuilder = new AgingBoardCardBuilder(taskManagerClient, errorService);
        }

        [Test]
        public void TestyTryBuildButNeworkExceptionOccured()
        {
            var cardId = DataGenerator.GenEnglishString(15);

            using (mock.Record())
            {
                taskManagerClient.Expect(f => f.GetCard(cardId)).Throw(new Exception());
                errorService.Expect(f => f.SendError(Arg<string>.Is.NotNull, Arg<Exception>.Is.NotNull));
            }

            var actual = agingBoardCardBuilder.TryBuildModel(cardId);
            Assert.IsFalse(actual.HasValue);
        }

        [Test]
        public void TestTryBuildWhenBoardListIsUndefined()
        {
            var cardId = DataGenerator.GenEnglishString(15);
            var boardId = DataGenerator.GenEnglishString(12);
            var card = new BoardCard
            {
                Id = cardId,
                BoardId = boardId
            };

            using (mock.Record())
            {
                taskManagerClient.Expect(f => f.GetCard(cardId)).Return(card);
                taskManagerClient.Expect(f => f.GetBoardLists(Arg<string[]>.Matches(arg => arg.Length == 1 && arg[0].Equals(boardId)))).Return(new BoardList[0]);
                errorService.Expect(f => f.SendError(Arg<string>.Is.NotNull));
            }

            var actual = agingBoardCardBuilder.TryBuildModel(cardId);
            Assert.IsFalse(actual.HasValue);
        }

        [Test]
        public void TestTryBuildSuccessfully()
        {
            var cardId = DataGenerator.GenEnglishString(15);
            var boardId = DataGenerator.GenEnglishString(12);
            var boardListId = DataGenerator.GenEnglishString(14);
            var listName = DataGenerator.GenEnglishString(10);
            var lastActivity = DateTime.Now;
            var card = new BoardCard
            {
                Id = cardId,
                BoardId = boardId,
                BoardListId = boardListId,
                IsArchived = true,
                LastActivity = lastActivity
            };
            var boardList = new BoardList
            {
                Id = boardListId,
                Name = listName
            };

            using (mock.Record())
            {
                taskManagerClient.Expect(f => f.GetCard(cardId)).Return(card);
                taskManagerClient.Expect(f => f.GetBoardLists(Arg<string[]>.Matches(arg => arg.Length == 1 && arg[0].Equals(boardId)))).Return(new[] {boardList});
            }

            var expected = new AgingBoardCardModel
            {
                CardId = cardId,
                IsArchived = true,
                BoardListName = listName,
                LastActivity = lastActivity,
                ExpirationPeriod = TimeSpan.FromDays(3)
            };
            var actual = agingBoardCardBuilder.TryBuildModel(cardId);
            Assert.IsTrue(actual.HasValue);
            UnitWrappers.Assert.AreDeepEqual(actual, expected);
        }
    }
}