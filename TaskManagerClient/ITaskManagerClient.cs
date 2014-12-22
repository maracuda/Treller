using System;
using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.TaskManagerClient
{
    public interface ITaskManagerClient
    {
        Board[] GetBoards(string[] boardIds);
        BoardList[] GetBoardLists(params string[] boardIds);
        BoardCard[] GetBoardCards(string[] boardIds);
        User[] GetBoardUsers(string[] boardIds);
        CardChecklist[] GetBoardChecklists(string[] boardIds);
        CardAction[] GetActionsForBoardCards(string[] boardIds, DateTime? fromUtc = null);

        CardAction[] GetCardActions(string cardId);
        BoardCard GetCard(string cardId);
        User[] GetCardUsers(string cardId);
        CardChecklist[] GetCardChecklists(string cardId);
    }
}