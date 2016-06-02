using System.Linq;
using NUnit.Framework;
using SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Kanban
{
    public class BoardServiceTest : IntegrationTest
    {
        private IBoardsService boardsService;

        public override void SetUp()
        {
            base.SetUp();

            boardsService = container.Get<IBoardsService>();
        }

        [Test]
        public void TestSelectKanbanBoards()
        {
            var expected = new[] { "Service Team", "Модификаторы", "Продажи ОБ", "Детализация счета", "Загрузка Excel в ПП" };
            var actuals = boardsService.SelectKanbanBoards(false);
            var actualNames = actuals.Select(x => x.Name).ToArray();
            CollectionAssert.AreEquivalent(expected, actualNames);
            Assert.False(actuals.Any(x => x.IsClosed));

            var actualsWithClosed = boardsService.SelectKanbanBoards(true);
            Assert.True(actualsWithClosed.Length > expected.Length);
            Assert.True(actualsWithClosed.Any(x => x.IsClosed));
        }
    }
}