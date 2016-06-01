using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.Infrastructure.Sugar;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Settings
{
    public class KanbanBoardMetaInfoBuilder : IKanbanBoardMetaInfoBuilder
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly Lazy<KanbanBoardMetaInfo[]> _settings;
        private const string SettingsName = "boardSettings";

        public KanbanBoardMetaInfoBuilder(
            ICachedFileStorage cachedFileStorage, 
            ITaskManagerClient taskManagerClient,
            IErrorService errorService)
        {
            this.taskManagerClient = taskManagerClient;
            _settings = new Lazy<KanbanBoardMetaInfo[]>(() => GetBoardSettings(cachedFileStorage, taskManagerClient, errorService), true);
        }

        private static readonly HashSet<string> exceptBoardNames = new HashSet<string>(new[] { "dev", "Архив", "Стратегия 2014", "Стратегия 2015", "Стратегия 2016", "Автотесты" });
        private const string OrganizationName = "konturbilling";

        private static KanbanBoardMetaInfo[] GetBoardSettings(ICachedFileStorage cachedFileStorage, ITaskManagerClient taskManagerClient, IErrorService errorService)
        {
            KanbanBoardMetaInfo[] result;
            try
            {
                result = ParseKanbanBoardMetaInfos(taskManagerClient);
                cachedFileStorage.Write(SettingsName, result);
            }
            catch (Exception e)
            {
                errorService.SendError("Fail to parse kanban boards meta info", e);
                result = cachedFileStorage.Find<KanbanBoardMetaInfo[]>(SettingsName);
            }

            return result;
        }

        private static KanbanBoardMetaInfo[] ParseKanbanBoardMetaInfos(ITaskManagerClient taskManagerClient)
        {
            var allBoards = taskManagerClient.GetOpenBoards(OrganizationName)
                                             .Where(x => !exceptBoardNames.Contains(x.Name))
                                             .ToArray();
            return allBoards.Select(x => TryParseBoardMetaInfo(taskManagerClient, x))
                            .Where(x => x.HasValue)
                            .Select(x => x.Value)
                            .ToArray();
        }

        private static Maybe<KanbanBoardMetaInfo> TryParseBoardMetaInfo(ITaskManagerClient taskManagerClient, Board board)
        {
            var boardLists = taskManagerClient.GetBoardLists(board.Id);
            if (boardLists.Length < 7)
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

        public KanbanBoardMetaInfo[] BuildForAllOpenBoards()
        {
            return _settings.Value;
        }

        public KanbanBoardMetaInfo[] GetDevelopingBoardsWithClosed()
        {
            var boardNames = taskManagerClient.GetAllBoards(OrganizationName)
                                              .Where(x => !exceptBoardNames.Contains(x.Name));
            return boardNames.Select(x => TryParseBoardMetaInfo(taskManagerClient, x))
                             .Where(x => x.HasValue)
                             .Select(x => x.Value)
                             .ToArray();
        }
    }
}