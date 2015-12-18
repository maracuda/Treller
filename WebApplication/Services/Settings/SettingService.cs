using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.BlocksMapping.BlockExtenssions;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Storages;

namespace SKBKontur.Treller.WebApplication.Services.Settings
{
    public class SettingService : ISettingService
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly Lazy<BoardSettings[]> _settings;
        private const string SettingsName = "boardSettings";

        public SettingService(ICachedFileStorage cachedFileStorage, ITaskManagerClient taskManagerClient)
        {
            this.taskManagerClient = taskManagerClient;
            _settings = new Lazy<BoardSettings[]>(() => GetBoardSettings(cachedFileStorage, taskManagerClient), true);
        }

        private static HashSet<string> exceptBoardNames = new HashSet<string>(new[] { "dev", "FeaturePool", "Manager Tasks", "Архив", "Оптимизация ТП", "Стратегия 2014", "Стратегия 2015", "Стратегия 2016", "Billing", "CRM", "dev_old" });
        private const string OrganizationName = "konturbilling";

        private static readonly BoardSettings[] DefaultSettings = new[]
        {
            new BoardSettings
                {
                    Id = "552ab670e01e4af28afdc2c2",
                    Name = "Биллинг продуктов",
                    DevelopListName = "Dev",
                    AnalyticListName = "Analytics & Design",
                    ReviewListName = "Review",
                    DevelopPresentationListName = "",
                    TestingListName = "Testing",
                    WaitForReleaseListName = "Wait for release",
                    IsDeleted = true,
                    IsServiceTeamBoard = true
                },
            new BoardSettings
                {
                    Id = "552aae2b8ca279a4eb485af7",
                    Name = "Услуги УЦ",
                    DevelopListName = "Dev",
                    AnalyticListName = "Analytics & Design",
                    ReviewListName = "Review",
                    DevelopPresentationListName = "",
                    TestingListName = "Testing",
                    WaitForReleaseListName = "Wait for release",
                },
            new BoardSettings
                {
                    Id = "552ab5e211d6ae9b61dbaa77",
                    Name = "Оптимизация ТП",
                    DevelopListName = "Dev",
                    AnalyticListName = "Analytics & Design",
                    ReviewListName = "Review",
                    DevelopPresentationListName = "",
                    TestingListName = "Testing",
                    WaitForReleaseListName = "Wait for release",
                    IsDeleted = true
                },
            new BoardSettings
                {
                    Id = "552ab2abc66faf3bec7915f4",
                    Name = "Инфраструктура",
                    DevelopListName = "Dev",
                    AnalyticListName = "",
                    ReviewListName = "Review",
                    DevelopPresentationListName = "",
                    TestingListName = "Testing",
                    WaitForReleaseListName = "Wait for release",
                    IsServiceTeamBoard = true
                },
            new BoardSettings
                {
                    Id = "557e531fe71fe22dd72d1348",
                    Name = "Партнерка",
                    DevelopListName = "Dev",
                    AnalyticListName = "Analytics&Desing",
                    ReviewListName = "Review",
                    DevelopPresentationListName = "",
                    TestingListName = "Testing",
                    WaitForReleaseListName = "Wait for release",
                    IsDeleted = true
                },
            new BoardSettings
                {
                    Id = "55dc679888eb0faddaf201b2",
                    Name = "Сценарий продления",
                    DevelopListName = "Dev",
                    AnalyticListName = "Analytics&Desing",
                    ReviewListName = "Review",
                    DevelopPresentationListName = "",
                    TestingListName = "Testing",
                    WaitForReleaseListName = "Wait for release"
                },
            new BoardSettings
                {
                    Id = "55d55d17e6ca1d101fe0061c",
                    Name = "Вендоры",
                    DevelopListName = "Dev",
                    AnalyticListName = "",
                    ReviewListName = "Review",
                    DevelopPresentationListName = "",
                    TestingListName = "Testing",
                    WaitForReleaseListName = "Wait for release"
                }
        };

        private static BoardSettings[] GetBoardSettings(ICachedFileStorage cachedFileStorage, ITaskManagerClient taskManagerClient)
        {
            BoardSettings[] result;
            try
            {
                result = GetBoardSettings(taskManagerClient);
                cachedFileStorage.Write(SettingsName, result);
            }
            catch (Exception)
            {
                result = cachedFileStorage.Find<BoardSettings[]>(SettingsName);
            }

            if (result == null)
            {
                result = GetBoardSettings(taskManagerClient);
                cachedFileStorage.Write(SettingsName, result);
            }

            return result;
        }

        private static BoardSettings[] GetBoardSettings(ITaskManagerClient taskManagerClient)
        {
            var allBoards =
                taskManagerClient.GetOpenBoardsAsync(OrganizationName)
                    .Result.Where(x => !exceptBoardNames.Contains(x.Name))
                    .ToArray();
            var settings = DefaultSettings.ToDictionary(x => x.Id);
            return allBoards.Select(x => BuildBoardSettings(x, settings.SafeGet(x.Id))).ToArray();
        }

        private static BoardSettings BuildBoardSettings(Board board, BoardSettings boardData)
        {
            return new BoardSettings
            {
                Id = board.Id,
                Name = board.Name,
                IsDeleted = false,
                IsServiceTeamBoard = boardData != null && boardData.IsServiceTeamBoard,
                WaitForReleaseListName = boardData != null ? boardData.WaitForReleaseListName : "Wait for release",
                AnalyticListName = boardData != null ? boardData.AnalyticListName : "Analytics & Design",
                DevelopListName = boardData != null ? boardData.DevelopListName : "Dev",
                DevelopPresentationListName = boardData != null ? boardData.DevelopPresentationListName : "",
                ReviewListName = boardData != null ? boardData.ReviewListName : "Review",
                TestingListName = boardData != null ? boardData.TestingListName : "Testing"
            };
        }

        public string[] GetDevelopingBoardIds()
        {
            return _settings.Value.Select(x => x.Id).ToArray();
        }

        public BoardSettings[] GetDevelopingBoards()
        {
            return _settings.Value;
        }

        public BoardSettings[] GetDevelopingBoardsWithClosed()
        {
            var allBoards = taskManagerClient.GetAllBoards(OrganizationName);
            var settings = DefaultSettings.ToDictionary(x => x.Id);
            return (allBoards).Where(x => !exceptBoardNames.Contains(x.Name)).Select(x => BuildBoardSettings(x, settings.SafeGet(x.Id))).ToArray();
        }
    }
}