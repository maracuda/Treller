using System;
using System.Linq;
using NUnit.Framework;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage;
using Assert = SKBKontur.Treller.Tests.UnitWrappers.Assert;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.News
{
    public class TaskNewsStorageTest : IntegrationTest
    {
        private TaskNewStorage taskNewStorage;

        public override void SetUp()
        {
            base.SetUp();

            taskNewStorage = container.Get<TaskNewStorage>();
        }

        [Test]
        public void TestDeleteAndFind()
        {
            var taskId = Guid.NewGuid().ToString();
            var taskNew = new TaskNew
            {
                TaskId = taskId,
                DeliveryChannel = NewDeliveryChannelType.Team
            };

            taskNewStorage.Create(taskNew);
            var actual = taskNewStorage.Find(taskId);
            Assert.True(actual.HasValue);
            Assert.AreEqual(1, actual.Value.Length);
            Assert.AreDeepEqual(actual.Value.First(), taskNew);
            taskNewStorage.Delete(taskNew);
            actual = taskNewStorage.Find(taskId);
            Assert.False(actual.HasValue);
        }

        [Test]
        public void TestDeleteAndReadAll()
        {
            var taskId = Guid.NewGuid().ToString();
            var taskNew = new TaskNew
            {
                TaskId = taskId,
                DeliveryChannel = NewDeliveryChannelType.Team
            };

            taskNewStorage.Create(taskNew);
            var actual = taskNewStorage.ReadAll();
            Assert.AreEqual(1, actual.Length);
            Assert.AreDeepEqual(actual.First(), taskNew);
            taskNewStorage.Delete(taskNew);
            actual = taskNewStorage.ReadAll();
            Assert.AreEqual(0, actual.Length);
        }
    }
}