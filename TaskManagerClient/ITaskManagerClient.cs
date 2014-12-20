using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.TaskManagerClient
{
    public interface ITaskManagerClient
    {
        IEnumerable<Board> GetBoards(string[] boardIds);
        IEnumerable<BoardList> GetBoardLists(params string[] boardIds);
        IEnumerable<BoardCard> GetBoardCards(string[] boardIds);
        IEnumerable<User> GetBoardUsers(string[] boardIds);
        IEnumerable<CardChecklist> GetBoardChecklists(string[] boardIds);
        IEnumerable<CardAction> GetCardActions(string cardId);
        IEnumerable<CardAction> GetActionsForBoardCards(string[] boardIds);

        BoardCard GetCard(string cardId);
        IEnumerable<User> GetCardUsers(string cardId);
        IEnumerable<CardChecklist> GetCardChecklists(string cardId);
    }
}