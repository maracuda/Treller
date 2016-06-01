using System.Linq;
using NUnit.Framework;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.Settings;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Kanban
{
    public class KanbanMetaInfoBuilderTest : IntegrationTest
    {
        private IKanbanBoardMetaInfoBuilder kanbanBoardMetaInfoBuilder;
        private IErrorService errorService;
        private ITaskManagerClient taskManagerClient;
        private ICachedFileStorage cachedFileStorage;

        public override void SetUp()
        {
            base.SetUp();

            errorService = container.Get<IErrorService>();
            taskManagerClient = container.Get<ITaskManagerClient>();
            cachedFileStorage = container.Get<ICachedFileStorage>();
            kanbanBoardMetaInfoBuilder = new KanbanBoardMetaInfoBuilder(cachedFileStorage, taskManagerClient, errorService);
        }

        [Test]
        public void TestBuildForAllOpenBoards()
        {
            var expected = new[] {"Service Team", "Модификаторы", "Продажи ОБ", "Детализация счета", "Загрузка Excel в ПП"};
            var actuals = kanbanBoardMetaInfoBuilder.BuildForAllOpenBoards();
            var actualNames = actuals.Select(x => x.Name).ToArray();
            CollectionAssert.AreEquivalent(expected, actualNames);
            Assert.IsTrue(actuals.Any(x => x.IsServiceTeamBoard));
            var serviceTeamBoard = actuals.First(x => x.IsServiceTeamBoard);
            Assert.AreEqual("Service Team", serviceTeamBoard.Name);
        }
    }
}