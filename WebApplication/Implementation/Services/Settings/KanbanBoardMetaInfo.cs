using System;
using System.Linq;
using SKBKontur.Infrastructure.Sugar;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Settings
{
    public class KanbanBoardMetaInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DevelopListName { get; set; }
        public string AnalyticListName { get; set; }
        public string ReviewListName { get; set; }
        public string DevelopPresentationListName { get; set; }
        public string TestingListName { get; set; }
        public string WaitForReleaseListName { get; set; }
        public bool IsServiceTeamBoard { get; set; }

        public static Maybe<KanbanBoardMetaInfo> TryParse(Board board, BoardList[] boardLists)
        {
            if (boardLists == null || boardLists.Length < 7)
                return null;

            if (ContainsList(boardLists, "Incoming") &&
                ContainsList(boardLists, "Analytics & Design") &&
                ContainsList(boardLists, "Dev") &&
                ContainsList(boardLists, "Review") &&
                ContainsList(boardLists, "Testing") &&
                ContainsList(boardLists, "Wait for release") &&
                ContainsList(boardLists, "Released"))
            {
                var result = new KanbanBoardMetaInfo
                {
                    Id = board.Id,
                    Name = board.Name,
                    WaitForReleaseListName = "Wait for release",
                    AnalyticListName = "Analytics & Design",
                    DevelopListName = "Dev",
                    DevelopPresentationListName = string.Empty,
                    ReviewListName = "Review",
                    TestingListName = "Testing"
                };
                if (board.Name.Equals("Service Team"))
                {
                    result.IsServiceTeamBoard = true;
                }
                return result;
            }
            return null;
        }

        private static bool ContainsList(BoardList[] lists, string pattern)
        {
            return lists.Any(x => x.Name.Trim().StartsWith(pattern, StringComparison.OrdinalIgnoreCase));
        }
    }
}