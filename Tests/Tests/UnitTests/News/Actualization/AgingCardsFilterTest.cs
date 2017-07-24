using System;
using Infrastructure.Common;
using Rhino.Mocks;
using WebApplication.Implementation.Services.News;
using WebApplication.Implementation.Services.News.Domain.Builders;
using WebApplication.Implementation.Services.News.Domain.Models;
using WebApplication.Implementation.Services.News.NewsFeed;
using Xunit;
using Assert = Tests.UnitWrappers.Assert;

namespace Tests.Tests.UnitTests.News.Actualization
{
    public class AgingCardsFilterTest : UnitTest
    {
        private IOutdatedBoardCardBuilder outdatedBoardCardBuilder;
        private OutdatedNewsFilter outdatedNewsFilter;
        private IDateTimeFactory dateTimeFactory;

        public AgingCardsFilterTest() : base()
        {
            outdatedBoardCardBuilder = mockRepository.Create<IOutdatedBoardCardBuilder>();
            dateTimeFactory = mockRepository.Create<IDateTimeFactory>();

            outdatedNewsFilter = new OutdatedNewsFilter(outdatedBoardCardBuilder, dateTimeFactory);
        }

        [Fact]
        public void TestFilter()
        {
            var task1 = new TaskNew { TaskId = DataGenerator.GenEnglishString(10) };
            var agingModel1 = new OutdatedBoardCardModel
            {
                IsArchived = true
            };
            var task2 = new TaskNew { TaskId = DataGenerator.GenEnglishString(10) };
            var agingModel2 = new OutdatedBoardCardModel();
            var task3 = new TaskNew { TaskId = DataGenerator.GenEnglishString(10) };
            var agingModel3 = new OutdatedBoardCardModel
            {
                IsArchived = true
            };
            var now = DateTime.Now;

            using (mockRepository.Record())
            {
                dateTimeFactory.Stub(f => f.UtcNow).Return(now);
                outdatedBoardCardBuilder.Stub(f => f.TryBuildModel(task1.TaskId)).Return(agingModel1);
                outdatedBoardCardBuilder.Stub(f => f.TryBuildModel(task2.TaskId)).Return(agingModel2);
                outdatedBoardCardBuilder.Stub(f => f.TryBuildModel(task3.TaskId)).Return(agingModel3);
            }

            var actual = outdatedNewsFilter.FilterOutdated(new [] { task1, task2, task3});
            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual(new [] {task1, task3}, actual);

            actual = outdatedNewsFilter.FilterActual(new[] { task1, task2, task3 });
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(new[] { task2 }, actual);
        }
    }
}