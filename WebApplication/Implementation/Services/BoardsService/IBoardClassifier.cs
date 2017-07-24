using TaskManagerClient.BusinessObjects.TaskManager;

namespace WebApplication.Implementation.Services.BoardsService
{
    public interface IBoardClassifier
    {
        BoardType IdentifyBoardType(Board board);
    }
}