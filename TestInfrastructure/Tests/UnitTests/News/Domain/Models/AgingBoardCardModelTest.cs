using System;
using NUnit.Framework;
using SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Models;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.News.Domain.Models
{
    public class AgingBoardCardModelTest : UnitTest
    {
        [Test]
        public void TestIsGrowedOldWhenCardIsArchived()
        {
            var model = GenerateAgingCardModel(true);
            Assert.IsTrue(model.IsGrowedOld());
        }

        [Test]
        public void TestIsGrowedOldWhenCardDoesNotReleased()
        {
            var model = GenerateAgingCardModel(false);
            Assert.IsFalse(model.IsGrowedOld());
        }

        [Test]
        public void TestIsGrowedOldWhenCardReleasedButNotLongTimeAgo()
        {
            var now = DateTime.Now;
            var model = GenerateAgingCardModel(false, KanbanBoardTemplate.ReleasedListName, now, now);
            Assert.IsFalse(model.IsGrowedOld());
        }

        [Test]
        public void TestIsGrowedOldWhenCardReleasedLongTimeAgo()
        {
            var now = DateTime.Now;
            var model = GenerateAgingCardModel(false, KanbanBoardTemplate.ReleasedListName, now.AddMilliseconds(-1), now);
            Assert.IsTrue(model.IsGrowedOld());
        }

        private AgingBoardCardModel GenerateAgingCardModel(bool isArchived = false, string boardListName = null, DateTime? lastActivity = null, DateTime? expirationTime = null )
        {
            return new AgingBoardCardModel
            {
                CardId = DataGenerator.GenEnglishString(10),
                BoardListName = boardListName ?? DataGenerator.GenEnglishString(10),
                ExpirationTime = expirationTime ?? DateTime.Now,
                LastActivity = lastActivity ?? DateTime.Now,
                IsArchived = isArchived,
            };
        }
    }
}