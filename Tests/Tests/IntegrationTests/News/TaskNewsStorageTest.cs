using System;
using System.Linq;
using WebApplication.Implementation.Services.News;
using WebApplication.Implementation.Services.News.Storage;
using Xunit;
using Assert = Tests.UnitWrappers.Assert;

namespace Tests.Tests.IntegrationTests.News
{
    public class TaskNewsStorageTest : IntegrationTest
    {
        private readonly TaskNewStorage taskNewStorage;

        public TaskNewsStorageTest()
        {
            taskNewStorage = container.Get<TaskNewStorage>();
        }

        [Fact]
        public void TestDeleteAndFind()
        {
            var taskId = Guid.NewGuid().ToString();
            var taskNew = new TaskNew
            {
                TaskId = taskId,
            };

            taskNewStorage.Create(taskNew);
            var actual = taskNewStorage.Find(taskId);
            Assert.True(actual.HasValue);
            Assert.AreDeepEqual(actual.Value, taskNew);
            taskNewStorage.Delete(taskNew);
            actual = taskNewStorage.Find(taskId);
            Assert.False(actual.HasValue);
        }

        [Fact]
        public void TestDeleteAndReadAll()
        {
            var taskId = Guid.NewGuid().ToString();
            var taskNew = new TaskNew
            {
                TaskId = taskId,
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