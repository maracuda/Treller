using System;
using System.Threading.Tasks;
using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.TaskManagerClient
{
    public interface ITaskManagerClient
    {
        Task<Board[]> GetBoardsAsync(string[] boardIds);
        Task<BoardList[]> GetBoardListsAsync(params string[] boardIds);
        Task<BoardCard[]> GetBoardCardsAsync(string[] boardIds);
        Task<User[]> GetBoardUsersAsync(string[] boardIds);
        Task<CardChecklist[]> GetBoardChecklistsAsync(string[] boardIds);
        Task<CardAction[]> GetActionsForBoardCardsAsync(string[] boardIds, DateTime? fromUtc = null);

        Task<CardAction[]> GetCardActionsAsync(string cardId);
        Task<BoardCard> GetCardAsync(string cardId);
        Task<User[]> GetCardUsersAsync(string cardId);
        Task<CardChecklist[]> GetCardChecklistsAsync(string cardId);
    }
}