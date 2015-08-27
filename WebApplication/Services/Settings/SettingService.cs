using System;
using System.Linq;
using SKBKontur.Treller.WebApplication.Storages;

namespace SKBKontur.Treller.WebApplication.Services.Settings
{
    public class SettingService : ISettingService
    {
        private readonly Lazy<BoardSettings[]> _settings;
        private const string SettingsName = "boardSettings";

        public SettingService(ICachedFileStorage cachedFileStorage)
        {
            _settings = new Lazy<BoardSettings[]>(() => GetBoardSettings(cachedFileStorage), true);
        }

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
                    IsDeleted = true
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
                    WaitForReleaseListName = "Wait for release"
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

        private static BoardSettings[] GetBoardSettings(ICachedFileStorage cachedFileStorage)
        {
            var result = cachedFileStorage.Find<BoardSettings[]>(SettingsName);
            if (result == null)
            {
                result = DefaultSettings;
                cachedFileStorage.Write(SettingsName, DefaultSettings);
            }

            return result;
        }

        public string[] GetDevelopingBoardIds()
        {
            return _settings.Value.Select(x => x.Id).ToArray();
        }

        public BoardSettings[] GetDevelopingBoards()
        {
            return _settings.Value;
        }
    }
}