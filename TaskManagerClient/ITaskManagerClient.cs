using System;
using System.Threading.Tasks;
using TaskManagerClient.BusinessObjects.TaskManager;

namespace TaskManagerClient
{
    public interface ITaskManagerClient
    {
        Board[] GetAllBoards(string organizationIdOrName);
        Board[] GetOpenBoards(string organizationIdOrName);

        BoardList[] GetBoardLists(params string[] boardIds);
        Task<BoardList[]> GetBoardListsAsync(params string[] boardIds);

        Task<BoardCard> GetCardAsync(string cardId);
        BoardCard GetCard(string cardId);
        Task<BoardCard[]> GetBoardCardsAsync(string[] boardIds);

        Task<User[]> GetBoardUsersAsync(string[] boardIds);
        Task<User[]> GetCardUsersAsync(string cardId);

        CardAction[] GetActionsForBoardCards(string[] boardIds, DateTime fromUtc, DateTime toUtc);
        Task<CardAction[]> GetActionsForBoardCardsAsync(string[] boardIds, DateTime? fromUtc = null, int limit = 1000);
        Task<CardAction[]> GetCardActionsAsync(string cardId);
        CardAction[] GetCardActions(string cardId);
        CardAction[] GetCardUpdateActions(string cardId);

        Task<CardChecklist[]> GetBoardChecklistsAsync(string[] boardIds);
        Task<CardChecklist[]> GetCardChecklistsAsync(string cardId);
    }
}