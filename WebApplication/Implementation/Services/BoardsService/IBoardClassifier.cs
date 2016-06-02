using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService
{
    public interface IBoardClassifier
    {
        BoardType IdentifyBoardType(Board board);
    }
}