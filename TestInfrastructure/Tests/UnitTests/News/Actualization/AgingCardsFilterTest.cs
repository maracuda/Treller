using System;
using NUnit.Framework;
using Rhino.Mocks;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Actualization;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Builders;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Models;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.News.Actualization
{
    public class AgingCardsFilterTest : UnitTest
    {
        private IAgingBoardCardBuilder agingBoardCardBuilder;
        private AgingCardsFilter agingCardsFilter;
        private IDateTimeFactory dateTimeFactory;

        public override void SetUp()
        {
            base.SetUp();

            agingBoardCardBuilder = mock.Create<IAgingBoardCardBuilder>();
            dateTimeFactory = mock.Create<IDateTimeFactory>();

            agingCardsFilter = new AgingCardsFilter(agingBoardCardBuilder, dateTimeFactory);
        }

        [Test]
        public void TestFilter()
        {
            var task1 = new TaskNew { TaskId = DataGenerator.GenEnglishString(10) };
            var agingModel1 = new AgingBoardCardModel
            {
                IsArchived = true
            };
            var task2 = new TaskNew { TaskId = DataGenerator.GenEnglishString(10) };
            var agingModel2 = new AgingBoardCardModel();
            var task3 = new TaskNew { TaskId = DataGenerator.GenEnglishString(10) };
            var agingModel3 = new AgingBoardCardModel
            {
                IsArchived = true
            };
            var now = DateTime.Now;

            using (mock.Record())
            {
                dateTimeFactory.Stub(f => f.UtcNow).Return(now);
                agingBoardCardBuilder.Stub(f => f.TryBuildModel(task1.TaskId)).Return(agingModel1);
                agingBoardCardBuilder.Stub(f => f.TryBuildModel(task2.TaskId)).Return(agingModel2);
                agingBoardCardBuilder.Stub(f => f.TryBuildModel(task3.TaskId)).Return(agingModel3);
            }

            var actual = agingCardsFilter.FilterAging(new [] { task1, task2, task3});
            Assert.AreEqual(2, actual.Length);
            CollectionAssert.AreEquivalent(new [] {task1, task3}, actual);

            actual = agingCardsFilter.FilterFresh(new[] { task1, task2, task3 });
            Assert.AreEqual(1, actual.Length);
            CollectionAssert.AreEquivalent(new[] { task2 }, actual);
        }
    }
}