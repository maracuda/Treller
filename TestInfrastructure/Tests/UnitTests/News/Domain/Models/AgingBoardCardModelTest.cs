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
            Assert.IsTrue(model.IsOutdated(DateTime.UtcNow));
        }

        [Test]
        public void TestIsGrowedOldWhenCardDoesNotReleased()
        {
            var model = GenerateAgingCardModel(false);
            Assert.IsFalse(model.IsOutdated(DateTime.UtcNow));
        }

        [Test]
        public void TestIsGrowedOldWhenCardReleasedButNotLongTimeAgo()
        {
            var now = DateTime.Now;
            var model = GenerateAgingCardModel(false, KanbanBoardTemplate.ReleasedListName, now, TimeSpan.FromDays(3));
            Assert.IsFalse(model.IsOutdated(now));
            Assert.IsFalse(model.IsOutdated(now.AddDays(3).AddMilliseconds(-1)));
            Assert.IsTrue(model.IsOutdated(now.AddDays(3)));
        }

        private static OutdatedBoardCardModel GenerateAgingCardModel(bool isArchived = false, string boardListName = null, DateTime? lastActivity = null, TimeSpan? expirationPeriod = null )
        {
            return new OutdatedBoardCardModel
            {
                CardId = DataGenerator.GenEnglishString(10),
                BoardListName = boardListName ?? DataGenerator.GenEnglishString(10),
                ExpirationPeriod = expirationPeriod ?? TimeSpan.Zero,
                LastActivity = lastActivity ?? DateTime.Now,
                IsArchived = isArchived,
            };
        }
    }
}