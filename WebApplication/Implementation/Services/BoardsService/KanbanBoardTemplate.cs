using System;
using System.Linq;
using TaskManagerClient.BusinessObjects.TaskManager;

namespace WebApplication.Implementation.Services.BoardsService
{
    public static class KanbanBoardTemplate
    {
        public const string IncomingListName = "Incoming";
        public const string AnalyticListName = "Analytics & Design";
        public const string DevListName = "Dev";
        public const string ReviewListName = "Review";
        public const string TestingListName = "Testing";
        public const string WaitForReleaseListName = "Wait for release";
        public const string ReleasedListName = "Released";

        public static bool IsServiceTeamBoard()
        {
            return false;
        }

        public static bool Matches(Board board, BoardList[] boardLists)
        {
            if (boardLists == null || boardLists.Length < 7)
                return false;

            return ContainsList(boardLists, IncomingListName) &&
                   ContainsList(boardLists, AnalyticListName) &&
                   ContainsList(boardLists, DevListName) &&
                   ContainsList(boardLists, ReviewListName) &&
                   ContainsList(boardLists, TestingListName) &&
                   ContainsList(boardLists, WaitForReleaseListName) &&
                   ContainsList(boardLists, ReleasedListName);
        }

        private static bool ContainsList(BoardList[] lists, string pattern)
        {
            return lists.Any(x => x.Name.Trim().StartsWith(pattern, StringComparison.OrdinalIgnoreCase));
        }
    }
}