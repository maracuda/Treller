using System;
using Xunit;
using SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Models;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.News.Domain.Models
{
    public class AgingBoardCardModelTest : UnitTest
    {
        [Fact]
        public void TestIsGrowedOldWhenCardIsArchived()
        {
            var model = GenerateAgingCardModel(true);
            Assert.True(model.IsOutdated(DateTime.UtcNow));
        }

        [Fact]
        public void TestIsGrowedOldWhenCardDoesNotReleased()
        {
            var model = GenerateAgingCardModel(false);
            Assert.False(model.IsOutdated(DateTime.UtcNow));
        }

        [Fact]
        public void TestIsGrowedOldWhenCardReleasedButNotLongTimeAgo()
        {
            var now = DateTime.Now;
            var model = GenerateAgingCardModel(false, KanbanBoardTemplate.ReleasedListName, now, TimeSpan.FromDays(3));
            Assert.False(model.IsOutdated(now));
            Assert.False(model.IsOutdated(now.AddDays(3).AddMilliseconds(-1)));
            Assert.True(model.IsOutdated(now.AddDays(3)));
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