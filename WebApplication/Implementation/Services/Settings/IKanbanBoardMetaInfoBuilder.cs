namespace SKBKontur.Treller.WebApplication.Implementation.Services.Settings
{
    public interface IKanbanBoardMetaInfoBuilder
    {
        KanbanBoardMetaInfo[] BuildForAllOpenBoards();
        KanbanBoardMetaInfo[] GetDevelopingBoardsWithClosed();
    }
}