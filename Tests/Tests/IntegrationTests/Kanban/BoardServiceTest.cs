using System.Linq;
using WebApplication.Implementation.Services.BoardsService;
using Xunit;

namespace Tests.Tests.IntegrationTests.Kanban
{
    public class BoardServiceTest : IntegrationTest
    {
        private readonly IBoardsService boardsService;

        public BoardServiceTest() : base()
        {
            boardsService = container.Get<IBoardsService>();
        }

        [Fact]
        public void TestSelectKanbanBoards()
        {
            var expected = new[] { "Service Team", "Бронирование по ПП", "Докупка и смена тарифа", "Миграция КЭ", "Связанные организации", "Скидончики" };
            var actuals = boardsService.SelectKanbanBoards(false);
            var actualNames = actuals.Select(x => x.Name).ToArray();
            Assert.Equal(expected, actualNames);
            Assert.False(actuals.Any(x => x.IsClosed));

            var actualsWithClosed = boardsService.SelectKanbanBoards(true);
            Assert.True(actualsWithClosed.Length > expected.Length);
            Assert.True(actualsWithClosed.Any(x => x.IsClosed));
        }
    }
}